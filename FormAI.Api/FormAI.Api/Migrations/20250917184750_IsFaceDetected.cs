using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FormAI.Api.Migrations
{
    /// <inheritdoc />
    public partial class IsFaceDetected : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFaceDetected",
                table: "Photo",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFaceDetected",
                table: "Photo");
        }
    }
}
