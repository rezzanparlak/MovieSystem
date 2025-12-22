using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.APP.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFavourite",
                table: "UserMovies");

            migrationBuilder.DropColumn(
                name: "MovieName",
                table: "UserMovies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFavourite",
                table: "UserMovies",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MovieName",
                table: "UserMovies",
                type: "TEXT",
                nullable: true);
        }
    }
}
