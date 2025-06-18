using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class DataBaseMultiple : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "processInfos");

            migrationBuilder.CreateTable(
                name: "cpuProcessInfos",
                columns: table => new
                {
                    PID = table.Column<int>(type: "INTEGER", nullable: false),
                    date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ServerName = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CPUUsagePercentage = table.Column<double>(type: "REAL", nullable: false),
                    MemoryUsageMB = table.Column<double>(type: "REAL", nullable: false),
                    SystemInfoServerName = table.Column<string>(type: "TEXT", nullable: true),
                    SystemInfodateTime = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cpuProcessInfos", x => new { x.date, x.ServerName, x.PID });
                    table.ForeignKey(
                        name: "FK_cpuProcessInfos_systemInfos_SystemInfodateTime_SystemInfoServerName",
                        columns: x => new { x.SystemInfodateTime, x.SystemInfoServerName },
                        principalTable: "systemInfos",
                        principalColumns: new[] { "dateTime", "ServerName" });
                    table.ForeignKey(
                        name: "FK_cpuProcessInfos_systemInfos_date_ServerName",
                        columns: x => new { x.date, x.ServerName },
                        principalTable: "systemInfos",
                        principalColumns: new[] { "dateTime", "ServerName" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemoryProcessInfos",
                columns: table => new
                {
                    PID = table.Column<int>(type: "INTEGER", nullable: false),
                    date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ServerName = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CPUUsagePercentage = table.Column<double>(type: "REAL", nullable: false),
                    MemoryUsageMB = table.Column<double>(type: "REAL", nullable: false),
                    SystemInfoServerName = table.Column<string>(type: "TEXT", nullable: true),
                    SystemInfodateTime = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemoryProcessInfos", x => new { x.date, x.ServerName, x.PID });
                    table.ForeignKey(
                        name: "FK_MemoryProcessInfos_systemInfos_SystemInfodateTime_SystemInfoServerName",
                        columns: x => new { x.SystemInfodateTime, x.SystemInfoServerName },
                        principalTable: "systemInfos",
                        principalColumns: new[] { "dateTime", "ServerName" });
                    table.ForeignKey(
                        name: "FK_MemoryProcessInfos_systemInfos_date_ServerName",
                        columns: x => new { x.date, x.ServerName },
                        principalTable: "systemInfos",
                        principalColumns: new[] { "dateTime", "ServerName" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cpuProcessInfos_SystemInfodateTime_SystemInfoServerName",
                table: "cpuProcessInfos",
                columns: new[] { "SystemInfodateTime", "SystemInfoServerName" });

            migrationBuilder.CreateIndex(
                name: "IX_MemoryProcessInfos_SystemInfodateTime_SystemInfoServerName",
                table: "MemoryProcessInfos",
                columns: new[] { "SystemInfodateTime", "SystemInfoServerName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cpuProcessInfos");

            migrationBuilder.DropTable(
                name: "MemoryProcessInfos");

            migrationBuilder.CreateTable(
                name: "processInfos",
                columns: table => new
                {
                    date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ServerName = table.Column<string>(type: "TEXT", nullable: false),
                    PID = table.Column<int>(type: "INTEGER", nullable: false),
                    typeofentry = table.Column<string>(type: "TEXT", nullable: false),
                    CPUUsagePercentage = table.Column<double>(type: "REAL", nullable: false),
                    MemoryUsageMB = table.Column<double>(type: "REAL", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    SystemInfoServerName = table.Column<string>(type: "TEXT", nullable: true),
                    SystemInfoServerName1 = table.Column<string>(type: "TEXT", nullable: true),
                    SystemInfodateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    SystemInfodateTime1 = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_processInfos", x => new { x.date, x.ServerName, x.PID, x.typeofentry });
                    table.ForeignKey(
                        name: "FK_processInfos_systemInfos_SystemInfodateTime1_SystemInfoServerName1",
                        columns: x => new { x.SystemInfodateTime1, x.SystemInfoServerName1 },
                        principalTable: "systemInfos",
                        principalColumns: new[] { "dateTime", "ServerName" });
                    table.ForeignKey(
                        name: "FK_processInfos_systemInfos_SystemInfodateTime_SystemInfoServerName",
                        columns: x => new { x.SystemInfodateTime, x.SystemInfoServerName },
                        principalTable: "systemInfos",
                        principalColumns: new[] { "dateTime", "ServerName" });
                    table.ForeignKey(
                        name: "FK_processInfos_systemInfos_date_ServerName",
                        columns: x => new { x.date, x.ServerName },
                        principalTable: "systemInfos",
                        principalColumns: new[] { "dateTime", "ServerName" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_processInfos_SystemInfodateTime_SystemInfoServerName",
                table: "processInfos",
                columns: new[] { "SystemInfodateTime", "SystemInfoServerName" });

            migrationBuilder.CreateIndex(
                name: "IX_processInfos_SystemInfodateTime1_SystemInfoServerName1",
                table: "processInfos",
                columns: new[] { "SystemInfodateTime1", "SystemInfoServerName1" });
        }
    }
}
