using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetSystemInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAppointmentWorkdayTableAndAddWorkdayOverrideTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Schedules",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK__Appointme__Servi__5EBF139D",
                table: "Appointments");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropColumn(
                name: "WorkDate",
                table: "Workdays");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "ServiceId",
                table: "Appointments",
                newName: "EmployeeId");

            migrationBuilder.AddColumn<string>(
                name: "DayOfWeek",
                table: "Workdays",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsMain",
                table: "AppointmentServices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateOnly>(
                name: "AppointmentDate",
                table: "Appointments",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "EndTime",
                table: "Appointments",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "StartTime",
                table: "Appointments",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.CreateTable(
                name: "WorkdayOverrides",
                columns: table => new
                {
                    WorkdayOverrideId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    WorkdayOverrideDate = table.Column<DateOnly>(type: "date", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkdayOverrides", x => x.WorkdayOverrideId);
                    table.ForeignKey(
                        name: "FK_WorkdayOverrides_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkdayOverrides_EmployeeId",
                table: "WorkdayOverrides",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Employees_EmployeeId",
                table: "Appointments",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Employees_EmployeeId",
                table: "Appointments");

            migrationBuilder.DropTable(
                name: "WorkdayOverrides");

            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "Workdays");

            migrationBuilder.DropColumn(
                name: "IsMain",
                table: "AppointmentServices");

            migrationBuilder.DropColumn(
                name: "AppointmentDate",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Appointments",
                newName: "ServiceId");

            migrationBuilder.AddColumn<DateOnly>(
                name: "WorkDate",
                table: "Workdays",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "ScheduleId",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    ScheduleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkdayId = table.Column<int>(type: "int", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Schedule__9C8A5B4938FBDC9D", x => x.ScheduleId);
                    table.ForeignKey(
                        name: "FK__Schedules__Emplo__5FB337D6",
                        column: x => x.WorkdayId,
                        principalTable: "Workdays",
                        principalColumn: "WorkdayId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_WorkdayId",
                table: "Schedules",
                column: "WorkdayId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Schedules",
                table: "Appointments",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK__Appointme__Servi__5EBF139D",
                table: "Appointments",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ServiceId");
        }
    }
}
