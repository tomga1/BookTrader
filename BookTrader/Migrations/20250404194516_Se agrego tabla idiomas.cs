using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookTrader.Migrations
{
    /// <inheritdoc />
    public partial class Seagregotablaidiomas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdiomaId",
                table: "Libros",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Idiomas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaAgregado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdUsuario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Idiomas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Libros_IdiomaId",
                table: "Libros",
                column: "IdiomaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Libros_Idiomas_IdiomaId",
                table: "Libros",
                column: "IdiomaId",
                principalTable: "Idiomas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Libros_Idiomas_IdiomaId",
                table: "Libros");

            migrationBuilder.DropTable(
                name: "Idiomas");

            migrationBuilder.DropIndex(
                name: "IX_Libros_IdiomaId",
                table: "Libros");

            migrationBuilder.DropColumn(
                name: "IdiomaId",
                table: "Libros");
        }
    }
}
