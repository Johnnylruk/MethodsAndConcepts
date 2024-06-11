using Lealthy_Hospital_Application_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Lealthy_Hospital_Application_System.Data;

public class LHASDB : DbContext
{
    public LHASDB(DbContextOptions<LHASDB> options) : base(options)
    {
    }

    public DbSet<StaffModel> Staffs { get; set; }
    public DbSet<AppointmentModel> Appointments { get; set; }
    public DbSet<DiagnosisModel> Diagnosis { get; set; }
    public DbSet<PatientModel> Patients { get; set; }
    public DbSet<LabTestsModel> LabTests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      
        modelBuilder.Entity<AppointmentModel>()
            .HasOne(t => t.Patient)
            .WithMany(t => t.Appointments)
            .HasForeignKey(t => t.PatientId);

        modelBuilder.Entity<AppointmentModel>()
            .HasOne(t => t.Staffs)
            .WithMany(t => t.Appointments)
            .HasForeignKey(t => t.StaffId);

        modelBuilder.Entity<DiagnosisModel>()
            .HasOne(t => t.Patient)
            .WithMany(t => t.Diagnosis)
            .HasForeignKey(t => t.PatientId);

        modelBuilder.Entity<DiagnosisModel>()
            .HasOne(t => t.Staff)
            .WithMany(t => t.Diagnosis)
            .HasForeignKey(t => t.StaffId);

        modelBuilder.Entity<LabTestsModel>()
            .HasOne(t => t.Patient)
            .WithMany(t => t.LabTests)
            .HasForeignKey(t => t.PatientId);

        modelBuilder.Entity<LabTestsModel>()
            .HasOne(t => t.Staff)
            .WithMany(t => t.LabTests)
            .HasForeignKey(t => t.StaffId);
    }
}

    