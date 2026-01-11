using AiAppointmentAgent.Models;
using AiAppointmentAgent.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AiAppointmentAgent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly AiService _aiService;
        private readonly AppointmentService _appointmentService;

        public ChatController(AiService aiService, AppointmentService appointmentService)
        {
            _aiService = aiService;
            _appointmentService = appointmentService;
        }

        [HttpPost("book")]
        public async Task<IActionResult> Book([FromBody] string message)
        {
            // AI parses user input
            var appointmentDto = await _aiService.ParseAppointmentAsync(message);

            // Validate against real DB data
            var validation = await _appointmentService.ValidateAppointmentAsync(appointmentDto);
            if (validation != "VALID")
                return BadRequest(validation);

            // Save appointment
            await _appointmentService.SaveAppointmentAsync(appointmentDto);

            return Ok("Appointment booked successfully.");
        }
    }
}
