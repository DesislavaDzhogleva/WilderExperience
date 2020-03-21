namespace WilderExperience.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ChangedExperienceModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experiences_AspNetUsers_AuthorId1",
                table: "Experiences");

            migrationBuilder.DropIndex(
                name: "IX_Experiences_AuthorId1",
                table: "Experiences");

            migrationBuilder.DropColumn(
                name: "AuthorId1",
                table: "Experiences");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfVisit",
                table: "Experiences",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Experiences",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Experiences_AuthorId",
                table: "Experiences",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Experiences_AspNetUsers_AuthorId",
                table: "Experiences",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experiences_AspNetUsers_AuthorId",
                table: "Experiences");

            migrationBuilder.DropIndex(
                name: "IX_Experiences_AuthorId",
                table: "Experiences");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfVisit",
                table: "Experiences",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "Experiences",
                type: "int",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "AuthorId1",
                table: "Experiences",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Experiences_AuthorId1",
                table: "Experiences",
                column: "AuthorId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Experiences_AspNetUsers_AuthorId1",
                table: "Experiences",
                column: "AuthorId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
