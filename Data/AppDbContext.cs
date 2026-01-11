using AiAppointmentAgent.Models;
using Microsoft.EntityFrameworkCore;

namespace AiAppointmentAgent.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Doctor> Doctors => Set<Doctor>();
        public DbSet<ClinicRule> ClinicRules => Set<ClinicRule>();
        public DbSet<Appointment> Appointments => Set<Appointment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed Doctors
            modelBuilder.Entity<Doctor>().HasData(
                new Doctor { Id = 1, Name = "Dr. Kumar", Specialty = "Dentist", IsActive = true },
                new Doctor { Id = 2, Name = "Dr. Priya", Specialty = "Cardiologist", IsActive = true }
            );

            // Seed Clinic Rules
            modelBuilder.Entity<ClinicRule>().HasData(
                new ClinicRule
                {
                    Id = 1,
                    OpenTime = new TimeSpan(9, 0, 0),
                    CloseTime = new TimeSpan(18, 0, 0),
                    IsSundayClosed = true
                }
            );
        }
    }
}
