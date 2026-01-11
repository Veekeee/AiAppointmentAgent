using AiAppointmentAgent.Data;
using AiAppointmentAgent.Models;
using Microsoft.EntityFrameworkCore;

namespace AiAppointmentAgent.Services
{
    public class KnowledgeService
    {
        private readonly AppDbContext _db;

        public KnowledgeService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<string>> GetActiveDoctorsAsync()
        {
            return await _db.Doctors
                .Where(d => d.IsActive)
                .Select(d => d.Specialty)
                .ToListAsync();
        }


        public async Task<ClinicRule> GetClinicRulesAsync()
        {
            return await _db.ClinicRules.FirstAsync();
        }
    }

}
