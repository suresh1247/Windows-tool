using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class DataMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "freespacepercent",
                table: "diskInfos",
                newName: "FreeSpacePercent");

            migrationBuilder.RenameColumn(
                name: "driveletter",
                table: "diskInfos",
                newName: "DriveLetter");

            migrationBuilder.RenameColumn(
                name: "usedspaceingb",
                table: "diskInfos",
                newName: "UsedSpaceGB");

            migrationBuilder.RenameColumn(
                name: "freespaceingb",
                table: "diskInfos",
                newName: "TotalCapacityGB");

            migrationBuilder.RenameColumn(
                name: "Totalcapacityingb",
                table: "diskInfos",
                newName: "FreeSpaceGB");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FreeSpacePercent",
                table: "diskInfos",
                newName: "freespacepercent");

            migrationBuilder.RenameColumn(
                name: "DriveLetter",
                table: "diskInfos",
                newName: "driveletter");

            migrationBuilder.RenameColumn(
                name: "UsedSpaceGB",
                table: "diskInfos",
                newName: "usedspaceingb");

            migrationBuilder.RenameColumn(
                name: "TotalCapacityGB",
                table: "diskInfos",
                newName: "freespaceingb");

            migrationBuilder.RenameColumn(
                name: "FreeSpaceGB",
                table: "diskInfos",
                newName: "Totalcapacityingb");
        }
    }
}
