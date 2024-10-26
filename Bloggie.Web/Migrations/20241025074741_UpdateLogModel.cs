using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bloggie.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLogModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Exception",
                table: "Logs");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Logs",
                newName: "LogLevel");

            migrationBuilder.RenameColumn(
                name: "Level",
                table: "Logs",
                newName: "ExceptionDetails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LogLevel",
                table: "Logs",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ExceptionDetails",
                table: "Logs",
                newName: "Level");

            migrationBuilder.AddColumn<string>(
                name: "Exception",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
