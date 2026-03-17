using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetSystemInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSheduleWorkdayRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Schedules__Emplo__5FB337D6",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "ScheduleDate",
                table: "Schedules");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Schedules",
                newName: "WorkdayId");

            migrationBuilder.AddForeignKey(
                name: "FK__Schedules__Emplo__5FB337D6",
                table: "Schedules",
                column: "WorkdayId",
                principalTable: "Workdays",
                principalColumn: "WorkdayId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Schedules__Emplo__5FB337D6",
                table: "Schedules");

            migrationBuilder.RenameColumn(
                name: "WorkdayId",
                table: "Schedules",
                newName: "EmployeeId");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Schedules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateOnly>(
                name: "ScheduleDate",
                table: "Schedules",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddForeignKey(
                name: "FK__Schedules__Emplo__5FB337D6",
                table: "Schedules",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId");
        }
    }
}
