using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LealthyHospitalApplicationSystem.Migrations
{
    /// <inheritdoc />
    public partial class ColumnStaffName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StaffName",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StaffName",
                table: "Appointments");
        }
    }
}
