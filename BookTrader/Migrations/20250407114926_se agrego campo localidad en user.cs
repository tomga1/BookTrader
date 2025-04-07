using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookTrader.Migrations
{
    /// <inheritdoc />
    public partial class seagregocampolocalidadenuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocalidadId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LocalidadId",
                table: "AspNetUsers",
                column: "LocalidadId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Localidades_LocalidadId",
                table: "AspNetUsers",
                column: "LocalidadId",
                principalTable: "Localidades",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Localidades_LocalidadId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_LocalidadId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LocalidadId",
                table: "AspNetUsers");
        }
    }
}
