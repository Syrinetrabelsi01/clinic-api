using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ClinicAPI.Models;

namespace ClinicAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<MedicalProfile> MedicalProfiles { get; set; }
        public DbSet<Appointment> Appointments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔗 One-to-One: Patient → MedicalProfile
            modelBuilder.Entity<Patient>()
                .HasOne(p => p.MedicalProfile)
                .WithOne(mp => mp.Patient)
                .HasForeignKey<MedicalProfile>(mp => mp.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔗 One-to-Many: Doctor → Appointments
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔗 One-to-Many: Patient → Appointments
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
