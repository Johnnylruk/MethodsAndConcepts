using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LealthyHospitalApplicationSystem.Migrations
{
    /// <inheritdoc />
    public partial class RoleAccessTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Access",
                table: "Staffs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Access",
                table: "Staffs");
        }
    }
}
