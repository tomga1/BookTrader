using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookTrader.Migrations
{
    /// <inheritdoc />
    public partial class publicadoridagregadoalibros : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicadorId",
                table: "Libros",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Libros_PublicadorId",
                table: "Libros",
                column: "PublicadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Libros_AspNetUsers_PublicadorId",
                table: "Libros",
                column: "PublicadorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Libros_AspNetUsers_PublicadorId",
                table: "Libros");

            migrationBuilder.DropIndex(
                name: "IX_Libros_PublicadorId",
                table: "Libros");

            migrationBuilder.DropColumn(
                name: "PublicadorId",
                table: "Libros");
        }
    }
}
