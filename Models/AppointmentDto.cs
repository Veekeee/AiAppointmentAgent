namespace AiAppointmentAgent.Models
{
    public class AppointmentDto
    {
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string Doctor { get; set; } = "";
        public string Reason { get; set; } = "";
    }
}
