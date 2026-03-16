using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetSystemInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkdaysIsDeletedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Workdays",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Workdays");
        }
    }
}
