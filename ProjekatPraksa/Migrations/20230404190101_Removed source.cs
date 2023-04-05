using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjekatPraksa.Migrations
{
    /// <inheritdoc />
    public partial class Removedsource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Source",
                table: "Students");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
