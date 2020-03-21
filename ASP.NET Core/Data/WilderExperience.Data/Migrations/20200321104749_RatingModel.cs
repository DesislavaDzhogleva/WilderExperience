namespace WilderExperience.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class RatingModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RatingNumber",
                table: "Ratings",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RatingNumber",
                table: "Ratings");
        }
    }
}
