using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class InfoDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_processInfos_systemInfos_dateTime_ServerName",
                table: "processInfos");

            migrationBuilder.RenameColumn(
                name: "dateTime",
                table: "processInfos",
                newName: "date");

            migrationBuilder.RenameIndex(
                name: "IX_processInfos_dateTime_ServerName",
                table: "processInfos",
                newName: "IX_processInfos_date_ServerName");

            migrationBuilder.AddForeignKey(
                name: "FK_processInfos_systemInfos_date_ServerName",
                table: "processInfos",
                columns: new[] { "date", "ServerName" },
                principalTable: "systemInfos",
                principalColumns: new[] { "dateTime", "ServerName" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_processInfos_systemInfos_date_ServerName",
                table: "processInfos");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "processInfos",
                newName: "dateTime");

            migrationBuilder.RenameIndex(
                name: "IX_processInfos_date_ServerName",
                table: "processInfos",
                newName: "IX_processInfos_dateTime_ServerName");

            migrationBuilder.AddForeignKey(
                name: "FK_processInfos_systemInfos_dateTime_ServerName",
                table: "processInfos",
                columns: new[] { "dateTime", "ServerName" },
                principalTable: "systemInfos",
                principalColumns: new[] { "dateTime", "ServerName" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
