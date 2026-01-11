using AiAppointmentAgent.Models;
using System.Text;
using System.Text.Json;


namespace AiAppointmentAgent.Services
{
    public class AiService
    {
        private readonly HttpClient _client;

        public AiService(HttpClient client)
        {
            _client = client;
        }

        public async Task<AppointmentDto> ParseAppointmentAsync(string input)
        {
            var request = new
            {
                model = "llama3.2:1b",
                prompt = BuildPrompt(input),
                stream = false
            };

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            var response = await _client.PostAsync("/api/generate", content);
            response.EnsureSuccessStatusCode();

            var raw = await response.Content.ReadAsStringAsync();

            var ollama = JsonSerializer.Deserialize<OllamaResponse>(
                raw,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (ollama?.Response == null)
                throw new Exception("Empty AI response");

            var json = ExtractJson(ollama.Response);

            return JsonSerializer.Deserialize<AppointmentDto>(
                json, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        private string BuildPrompt(string userInput)
        {
            var today = DateTime.UtcNow.ToString("yyyy-MM-dd");

            return
                $@"Today date is {today}.

                Extract appointment details from the text.
                
                Rules:
                - If user says ""tomorrow"", calculate date based on today.
                - Respond ONLY with valid JSON.
                - No markdown.
                - No explanation.
                
                Format:
                {{
                  ""date"": ""yyyy-MM-dd"",
                  ""time"": ""HH:mm"",
                  ""doctor"": ""string"",
                  ""reason"": ""string""
                }}
                
                Text:
                {userInput}";
        }


        //Extract JSON from AI response
        private static string ExtractJson(string text)
        {
            var start = text.IndexOf('{');
            var end = text.LastIndexOf('}');

            if (start == -1 || end == -1 || end <= start)
                throw new Exception("AI response does not contain valid JSON.");

            return text.Substring(start, end - start + 1);
        }
    }
}
