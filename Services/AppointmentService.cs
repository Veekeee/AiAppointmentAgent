using AiAppointmentAgent.Data;
using AiAppointmentAgent.Models;
using Microsoft.EntityFrameworkCore;

namespace AiAppointmentAgent.Services
{
    public class AppointmentService
    {
        private readonly AppDbContext _db;
        private readonly KnowledgeService _knowledgeService;

        public AppointmentService(AppDbContext db, KnowledgeService knowledgeService)
        {
            _db = db;
            _knowledgeService = knowledgeService;
        }

        public async Task<string> ValidateAppointmentAsync(AppointmentDto dto)
        {
            // Get active doctors from KnowledgeService
            var activeDoctors = await _knowledgeService.GetActiveDoctorsAsync();

            // Check if doctor exists
            if (!activeDoctors.Contains(dto.Doctor))
                return "Doctor not available.";

            // Get clinic rules from KnowledgeService
            var rules = await _knowledgeService.GetClinicRulesAsync();

            // Check for Sunday closure
            if (rules.IsSundayClosed && dto.Date.DayOfWeek == DayOfWeek.Sunday)
                return "Clinic is closed on Sunday.";

            // Check clinic hours
            if (dto.Time < rules.OpenTime || dto.Time > rules.CloseTime)
                return "Appointment time is outside clinic hours.";

            // Check for conflicts in database
            var doctorEntity = await _db.Doctors.FirstAsync(d => d.Specialty == dto.Doctor);
            var conflict = await _db.Appointments.AnyAsync(a =>
                a.DoctorId == doctorEntity.Id &&
                a.Date.Date == dto.Date.Date &&
                a.Time == dto.Time);

            if (conflict)
                return "Time slot already booked.";

            // Valid appointment
            return "VALID";
        }


        public async Task SaveAppointmentAsync(AppointmentDto dto)
        {
            var doctor = await _db.Doctors
                .FirstAsync(d => d.Specialty == dto.Doctor);

            var appointment = new Appointment
            {
                DoctorId = doctor.Id,
                Date = dto.Date.Date,
                Time = dto.Time
            };

            _db.Appointments.Add(appointment);
            await _db.SaveChangesAsync();
        }
    }
}
