using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnurStore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeQuanttyname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuantityAvailable",
                table: "Inventories",
                newName: "TotalPiecesAvailable");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalPiecesAvailable",
                table: "Inventories",
                newName: "QuantityAvailable");
        }
    }
}
