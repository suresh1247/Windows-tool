using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class Data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "diskInfos",
                columns: table => new
                {
                    dateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Server = table.Column<string>(type: "TEXT", nullable: false),
                    CPUUsage = table.Column<string>(type: "TEXT", nullable: false),
                    DiskUsage = table.Column<string>(type: "TEXT", nullable: false),
                    MemoryUsage = table.Column<string>(type: "TEXT", nullable: false),
                    PingStatus = table.Column<string>(type: "TEXT", nullable: false),
                    DNSResolution = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_diskInfos", x => x.dateTime);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "diskInfos");
        }
    }
}
