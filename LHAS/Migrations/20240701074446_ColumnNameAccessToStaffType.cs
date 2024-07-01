using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LealthyHospitalApplicationSystem.Migrations
{
    /// <inheritdoc />
    public partial class ColumnNameAccessToStaffType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Access",
                table: "Staffs",
                newName: "StaffType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StaffType",
                table: "Staffs",
                newName: "Access");
        }
    }
}
