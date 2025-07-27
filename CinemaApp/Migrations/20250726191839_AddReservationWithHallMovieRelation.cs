using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaApp.Migrations
{
    /// <inheritdoc />
    public partial class AddReservationWithHallMovieRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reservations_HallId",
                table: "Reservations");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_HallId_MovieId",
                table: "Reservations",
                columns: new[] { "HallId", "MovieId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_HallMovies_HallId_MovieId",
                table: "Reservations",
                columns: new[] { "HallId", "MovieId" },
                principalTable: "HallMovies",
                principalColumns: new[] { "HallId", "MovieId" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_HallMovies_HallId_MovieId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_HallId_MovieId",
                table: "Reservations");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_HallId",
                table: "Reservations",
                column: "HallId");
        }
    }
}
