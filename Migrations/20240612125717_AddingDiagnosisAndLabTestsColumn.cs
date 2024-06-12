using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LealthyHospitalApplicationSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddingDiagnosisAndLabTestsColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PatientName",
                table: "LabTests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StaffName",
                table: "LabTests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PatientName",
                table: "Diagnosis",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StaffName",
                table: "Diagnosis",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PatientName",
                table: "LabTests");

            migrationBuilder.DropColumn(
                name: "StaffName",
                table: "LabTests");

            migrationBuilder.DropColumn(
                name: "PatientName",
                table: "Diagnosis");

            migrationBuilder.DropColumn(
                name: "StaffName",
                table: "Diagnosis");
        }
    }
}
