using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC_Lab_1.Migrations
{
    /// <inheritdoc />
    public partial class departmentstatusadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Departments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DeptId",
                keyValue: 10,
                column: "Status",
                value: true);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DeptId",
                keyValue: 20,
                column: "Status",
                value: true);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DeptId",
                keyValue: 30,
                column: "Status",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Departments");
        }
    }
}
