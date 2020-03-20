using Microsoft.EntityFrameworkCore.Migrations;

namespace WilderExperience.Data.Migrations
{
    public partial class LocationsModelsFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "WildLocations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "WildLocations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Locations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "Locations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Lat",
                table: "Locations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Lng",
                table: "Locations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Locations",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "WildLocations");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "WildLocations");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Lat",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Lng",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Locations");
        }
    }
}
