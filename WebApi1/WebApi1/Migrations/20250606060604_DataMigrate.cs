using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class DataMigrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PingStatus",
                table: "diskInfos",
                newName: "usedspaceingb");

            migrationBuilder.RenameColumn(
                name: "MemoryUsage",
                table: "diskInfos",
                newName: "freespacepercent");

            migrationBuilder.RenameColumn(
                name: "DiskUsage",
                table: "diskInfos",
                newName: "freespaceingb");

            migrationBuilder.RenameColumn(
                name: "DNSResolution",
                table: "diskInfos",
                newName: "driveletter");

            migrationBuilder.RenameColumn(
                name: "CPUUsage",
                table: "diskInfos",
                newName: "Totalcapacityingb");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "usedspaceingb",
                table: "diskInfos",
                newName: "PingStatus");

            migrationBuilder.RenameColumn(
                name: "freespacepercent",
                table: "diskInfos",
                newName: "MemoryUsage");

            migrationBuilder.RenameColumn(
                name: "freespaceingb",
                table: "diskInfos",
                newName: "DiskUsage");

            migrationBuilder.RenameColumn(
                name: "driveletter",
                table: "diskInfos",
                newName: "DNSResolution");

            migrationBuilder.RenameColumn(
                name: "Totalcapacityingb",
                table: "diskInfos",
                newName: "CPUUsage");
        }
    }
}
