using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class InfoDatabaseAddedCompositeKeyswork : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "connectionTestModels",
                columns: table => new
                {
                    dateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ResolvedIP = table.Column<string>(type: "TEXT", nullable: false),
                    ConnectionTest = table.Column<string>(type: "TEXT", nullable: false),
                    ResolvedName = table.Column<string>(type: "TEXT", nullable: false),
                    NSLookupStatus = table.Column<string>(type: "TEXT", nullable: false),
                    DNSResolutionStatus = table.Column<string>(type: "TEXT", nullable: false),
                    NSLookupOutput = table.Column<string>(type: "TEXT", nullable: false),
                    Server = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_connectionTestModels", x => x.dateTime);
                });

            migrationBuilder.CreateTable(
                name: "rebootRequests",
                columns: table => new
                {
                    dateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ServerName = table.Column<string>(type: "TEXT", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rebootRequests", x => x.dateTime);
                });

            migrationBuilder.CreateTable(
                name: "serverHealths",
                columns: table => new
                {
                    dateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ServerName = table.Column<string>(type: "TEXT", nullable: false),
                    CPUUsage = table.Column<double>(type: "REAL", nullable: false),
                    MemoryUsage = table.Column<double>(type: "REAL", nullable: false),
                    DiskUsage = table.Column<double>(type: "REAL", nullable: false),
                    PingStatus = table.Column<string>(type: "TEXT", nullable: false),
                    DNSResolution = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_serverHealths", x => x.dateTime);
                });

            migrationBuilder.CreateTable(
                name: "systemInfos",
                columns: table => new
                {
                    dateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ServerName = table.Column<string>(type: "TEXT", nullable: false),
                    TotalCPUUsage = table.Column<double>(type: "REAL", nullable: false),
                    MemoryUsagePercentage = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_systemInfos", x => new { x.dateTime, x.ServerName });
                });

            migrationBuilder.CreateTable(
                name: "processInfos",
                columns: table => new
                {
                    PID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CPUUsagePercentage = table.Column<double>(type: "REAL", nullable: false),
                    MemoryUsageMB = table.Column<double>(type: "REAL", nullable: false),
                    dateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ServerName = table.Column<string>(type: "TEXT", nullable: false),
                    SystemInfoServerName = table.Column<string>(type: "TEXT", nullable: true),
                    SystemInfoServerName1 = table.Column<string>(type: "TEXT", nullable: true),
                    SystemInfodateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    SystemInfodateTime1 = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_processInfos", x => x.PID);
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
                        name: "FK_processInfos_systemInfos_dateTime_ServerName",
                        columns: x => new { x.dateTime, x.ServerName },
                        principalTable: "systemInfos",
                        principalColumns: new[] { "dateTime", "ServerName" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_processInfos_dateTime_ServerName",
                table: "processInfos",
                columns: new[] { "dateTime", "ServerName" });

            migrationBuilder.CreateIndex(
                name: "IX_processInfos_SystemInfodateTime_SystemInfoServerName",
                table: "processInfos",
                columns: new[] { "SystemInfodateTime", "SystemInfoServerName" });

            migrationBuilder.CreateIndex(
                name: "IX_processInfos_SystemInfodateTime1_SystemInfoServerName1",
                table: "processInfos",
                columns: new[] { "SystemInfodateTime1", "SystemInfoServerName1" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "connectionTestModels");

            migrationBuilder.DropTable(
                name: "processInfos");

            migrationBuilder.DropTable(
                name: "rebootRequests");

            migrationBuilder.DropTable(
                name: "serverHealths");

            migrationBuilder.DropTable(
                name: "systemInfos");
        }
    }
}
