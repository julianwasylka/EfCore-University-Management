using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversitySystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProwadzacyId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProwadzacyId",
                table: "Kursy",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Kursy_ProwadzacyId",
                table: "Kursy",
                column: "ProwadzacyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Kursy_Profesorowie_ProwadzacyId",
                table: "Kursy",
                column: "ProwadzacyId",
                principalTable: "Profesorowie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kursy_Profesorowie_ProwadzacyId",
                table: "Kursy");

            migrationBuilder.DropIndex(
                name: "IX_Kursy_ProwadzacyId",
                table: "Kursy");

            migrationBuilder.DropColumn(
                name: "ProwadzacyId",
                table: "Kursy");
        }
    }
}
