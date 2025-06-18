using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class Info : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_processInfos",
                table: "processInfos");

            migrationBuilder.AddColumn<string>(
                name: "typeofentry",
                table: "processInfos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_processInfos",
                table: "processInfos",
                columns: new[] { "date", "ServerName", "PID", "typeofentry" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_processInfos",
                table: "processInfos");

            migrationBuilder.DropColumn(
                name: "typeofentry",
                table: "processInfos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_processInfos",
                table: "processInfos",
                columns: new[] { "date", "ServerName", "PID" });
        }
    }
}
