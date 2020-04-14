using Microsoft.EntityFrameworkCore.Migrations;

namespace WilderExperience.Data.Migrations
{
    public partial class ImageModelAddAuthor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ExperienceImages",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExperienceImages_UserId",
                table: "ExperienceImages",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExperienceImages_AspNetUsers_UserId",
                table: "ExperienceImages",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExperienceImages_AspNetUsers_UserId",
                table: "ExperienceImages");

            migrationBuilder.DropIndex(
                name: "IX_ExperienceImages_UserId",
                table: "ExperienceImages");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ExperienceImages");
        }
    }
}
