using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CakeSystem.Infrastructure.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class FixComponentsType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Components_Type",
                table: "Components");

            migrationBuilder.CreateIndex(
                name: "IX_Components_Type",
                table: "Components",
                column: "Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Components_Type",
                table: "Components");

            migrationBuilder.CreateIndex(
                name: "IX_Components_Type",
                table: "Components",
                column: "Type",
                unique: true);
        }
    }
}
