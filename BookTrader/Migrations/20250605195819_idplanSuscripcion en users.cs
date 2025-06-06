using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookTrader.Migrations
{
    /// <inheritdoc />
    public partial class idplanSuscripcionenusers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdPlanSuscripcion",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_IdPlanSuscripcion",
                table: "AspNetUsers",
                column: "IdPlanSuscripcion");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_PlanesSuscripcion_IdPlanSuscripcion",
                table: "AspNetUsers",
                column: "IdPlanSuscripcion",
                principalTable: "PlanesSuscripcion",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_PlanesSuscripcion_IdPlanSuscripcion",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_IdPlanSuscripcion",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IdPlanSuscripcion",
                table: "AspNetUsers");
        }
    }
}
