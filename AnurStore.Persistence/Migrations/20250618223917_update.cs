using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnurStore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "ProductPurchases");

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "RecieptItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "RecieptItems");

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethod",
                table: "ProductPurchases",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
