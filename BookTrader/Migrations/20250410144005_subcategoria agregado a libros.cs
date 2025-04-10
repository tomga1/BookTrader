using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookTrader.Migrations
{
    /// <inheritdoc />
    public partial class subcategoriaagregadoalibros : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubCategoriasId",
                table: "Libros",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Libros_SubCategoriasId",
                table: "Libros",
                column: "SubCategoriasId");

            migrationBuilder.AddForeignKey(
                name: "FK_Libros_SubCategorias_SubCategoriasId",
                table: "Libros",
                column: "SubCategoriasId",
                principalTable: "SubCategorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Libros_SubCategorias_SubCategoriasId",
                table: "Libros");

            migrationBuilder.DropIndex(
                name: "IX_Libros_SubCategoriasId",
                table: "Libros");

            migrationBuilder.DropColumn(
                name: "SubCategoriasId",
                table: "Libros");
        }
    }
}
