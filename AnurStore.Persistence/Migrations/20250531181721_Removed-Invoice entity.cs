using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnurStore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemovedInvoiceentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSales_Invoices_InvoiceId",
                table: "ProductSales");

            migrationBuilder.DropForeignKey(
                name: "FK_Reciepts_ProductSales_ProductSaleId",
                table: "Reciepts");

            migrationBuilder.DropTable(
                name: "InvoiceItems");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Reciepts_ProductSaleId",
                table: "Reciepts");

            migrationBuilder.DropColumn(
                name: "CustomerCare",
                table: "Reciepts");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Reciepts");

            migrationBuilder.DropColumn(
                name: "ProductSaleId",
                table: "Reciepts");

            migrationBuilder.RenameColumn(
                name: "InvoiceId",
                table: "ProductSales",
                newName: "ReceiptId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSales_InvoiceId",
                table: "ProductSales",
                newName: "IX_ProductSales_ReceiptId");

            migrationBuilder.AddColumn<string>(
                name: "Batch",
                table: "ProductPurchases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSales_Reciepts_ReceiptId",
                table: "ProductSales",
                column: "ReceiptId",
                principalTable: "Reciepts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSales_Reciepts_ReceiptId",
                table: "ProductSales");

            migrationBuilder.DropColumn(
                name: "Batch",
                table: "ProductPurchases");

            migrationBuilder.RenameColumn(
                name: "ReceiptId",
                table: "ProductSales",
                newName: "InvoiceId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSales_ReceiptId",
                table: "ProductSales",
                newName: "IX_ProductSales_InvoiceId");

            migrationBuilder.AddColumn<string>(
                name: "CustomerCare",
                table: "Reciepts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Reciepts",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductSaleId",
                table: "Reciepts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomerCare = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Discount = table.Column<decimal>(type: "money", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NetAmount = table.Column<decimal>(type: "money", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceItems",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InvoiceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceItems_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reciepts_ProductSaleId",
                table: "Reciepts",
                column: "ProductSaleId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_InvoiceId",
                table: "InvoiceItems",
                column: "InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSales_Invoices_InvoiceId",
                table: "ProductSales",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reciepts_ProductSales_ProductSaleId",
                table: "Reciepts",
                column: "ProductSaleId",
                principalTable: "ProductSales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
