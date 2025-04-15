using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookTrader.Migrations
{
    /// <inheritdoc />
    public partial class SeagregoentidadedeestadoPublicacionademaspropenlibro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EstadoPublicacion",
                table: "Libros",
                newName: "EstadoPublicacionId");

            migrationBuilder.CreateTable(
                name: "EstadoPublicacionEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaPublicacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaAgregado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdUsuario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoPublicacionEntity", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Libros_EstadoPublicacionId",
                table: "Libros",
                column: "EstadoPublicacionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Libros_EstadoPublicacionEntity_EstadoPublicacionId",
                table: "Libros",
                column: "EstadoPublicacionId",
                principalTable: "EstadoPublicacionEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Libros_EstadoPublicacionEntity_EstadoPublicacionId",
                table: "Libros");

            migrationBuilder.DropTable(
                name: "EstadoPublicacionEntity");

            migrationBuilder.DropIndex(
                name: "IX_Libros_EstadoPublicacionId",
                table: "Libros");

            migrationBuilder.RenameColumn(
                name: "EstadoPublicacionId",
                table: "Libros",
                newName: "EstadoPublicacion");
        }
    }
}
