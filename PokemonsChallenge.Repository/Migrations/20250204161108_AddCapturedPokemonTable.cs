using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonsChallenge.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddCapturedPokemonTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "CapturedPokemons");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "CapturedPokemons",
                newName: "CapturedAt");

            migrationBuilder.AddColumn<int>(
                name: "PokemonId",
                table: "CapturedPokemons",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TrainerId",
                table: "CapturedPokemons",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Pokemons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pokemons", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CapturedPokemons_PokemonId",
                table: "CapturedPokemons",
                column: "PokemonId");

            migrationBuilder.CreateIndex(
                name: "IX_CapturedPokemons_TrainerId",
                table: "CapturedPokemons",
                column: "TrainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CapturedPokemons_Pokemons_PokemonId",
                table: "CapturedPokemons",
                column: "PokemonId",
                principalTable: "Pokemons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CapturedPokemons_Trainers_TrainerId",
                table: "CapturedPokemons",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CapturedPokemons_Pokemons_PokemonId",
                table: "CapturedPokemons");

            migrationBuilder.DropForeignKey(
                name: "FK_CapturedPokemons_Trainers_TrainerId",
                table: "CapturedPokemons");

            migrationBuilder.DropTable(
                name: "Pokemons");

            migrationBuilder.DropIndex(
                name: "IX_CapturedPokemons_PokemonId",
                table: "CapturedPokemons");

            migrationBuilder.DropIndex(
                name: "IX_CapturedPokemons_TrainerId",
                table: "CapturedPokemons");

            migrationBuilder.DropColumn(
                name: "PokemonId",
                table: "CapturedPokemons");

            migrationBuilder.DropColumn(
                name: "TrainerId",
                table: "CapturedPokemons");

            migrationBuilder.RenameColumn(
                name: "CapturedAt",
                table: "CapturedPokemons",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "CapturedPokemons",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
