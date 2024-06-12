﻿// <auto-generated />
using System;
using Lealthy_Hospital_Application_System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LealthyHospitalApplicationSystem.Migrations
{
    [DbContext(typeof(LHASDB))]
    [Migration("20240612095924_ColumnPatientName")]
    partial class ColumnPatientName
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Lealthy_Hospital_Application_System.Models.AppointmentModel", b =>
                {
                    b.Property<int>("AppointmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AppointmentId"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<string>("PatientName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StaffId")
                        .HasColumnType("int");

                    b.Property<string>("StaffName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AppointmentId");

                    b.HasIndex("PatientId");

                    b.HasIndex("StaffId");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("Lealthy_Hospital_Application_System.Models.DiagnosisModel", b =>
                {
                    b.Property<int>("DiagnosisId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DiagnosisId"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<int>("StaffId")
                        .HasColumnType("int");

                    b.Property<string>("Treatment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TypeOfDiagnosis")
                        .HasColumnType("int");

                    b.HasKey("DiagnosisId");

                    b.HasIndex("PatientId");

                    b.HasIndex("StaffId");

                    b.ToTable("Diagnosis");
                });

            modelBuilder.Entity("Lealthy_Hospital_Application_System.Models.LabTestsModel", b =>
                {
                    b.Property<int>("LabTestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LabTestId"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<bool?>("Result")
                        .HasColumnType("bit");

                    b.Property<int>("StaffId")
                        .HasColumnType("int");

                    b.Property<int>("TypeOfTests")
                        .HasColumnType("int");

                    b.HasKey("LabTestId");

                    b.HasIndex("PatientId");

                    b.HasIndex("StaffId");

                    b.ToTable("LabTests");
                });

            modelBuilder.Entity("Lealthy_Hospital_Application_System.Models.PatientModel", b =>
                {
                    b.Property<int>("PatientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PatientId"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Mobile")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PatientId");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("Lealthy_Hospital_Application_System.Models.StaffModel", b =>
                {
                    b.Property<int>("StaffId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StaffId"));

                    b.Property<int>("Access")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Mobile")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StaffId");

                    b.ToTable("Staffs");
                });

            modelBuilder.Entity("Lealthy_Hospital_Application_System.Models.AppointmentModel", b =>
                {
                    b.HasOne("Lealthy_Hospital_Application_System.Models.PatientModel", "Patient")
                        .WithMany("Appointments")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Lealthy_Hospital_Application_System.Models.StaffModel", "Staff")
                        .WithMany("Appointments")
                        .HasForeignKey("StaffId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Patient");

                    b.Navigation("Staff");
                });

            modelBuilder.Entity("Lealthy_Hospital_Application_System.Models.DiagnosisModel", b =>
                {
                    b.HasOne("Lealthy_Hospital_Application_System.Models.PatientModel", "Patient")
                        .WithMany("Diagnosis")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Lealthy_Hospital_Application_System.Models.StaffModel", "Staff")
                        .WithMany("Diagnosis")
                        .HasForeignKey("StaffId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Patient");

                    b.Navigation("Staff");
                });

            modelBuilder.Entity("Lealthy_Hospital_Application_System.Models.LabTestsModel", b =>
                {
                    b.HasOne("Lealthy_Hospital_Application_System.Models.PatientModel", "Patient")
                        .WithMany("LabTests")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Lealthy_Hospital_Application_System.Models.StaffModel", "Staff")
                        .WithMany("LabTests")
                        .HasForeignKey("StaffId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Patient");

                    b.Navigation("Staff");
                });

            modelBuilder.Entity("Lealthy_Hospital_Application_System.Models.PatientModel", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Diagnosis");

                    b.Navigation("LabTests");
                });

            modelBuilder.Entity("Lealthy_Hospital_Application_System.Models.StaffModel", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Diagnosis");

                    b.Navigation("LabTests");
                });
#pragma warning restore 612, 618
        }
    }
}
