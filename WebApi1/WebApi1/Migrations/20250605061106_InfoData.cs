using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class InfoData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_processInfos",
                table: "processInfos");

            migrationBuilder.DropIndex(
                name: "IX_processInfos_date_ServerName",
                table: "processInfos");

            migrationBuilder.AlterColumn<int>(
                name: "PID",
                table: "processInfos",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_processInfos",
                table: "processInfos",
                columns: new[] { "date", "ServerName", "PID" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_processInfos",
                table: "processInfos");

            migrationBuilder.AlterColumn<int>(
                name: "PID",
                table: "processInfos",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_processInfos",
                table: "processInfos",
                column: "PID");

            migrationBuilder.CreateIndex(
                name: "IX_processInfos_date_ServerName",
                table: "processInfos",
                columns: new[] { "date", "ServerName" });
        }
    }
}
