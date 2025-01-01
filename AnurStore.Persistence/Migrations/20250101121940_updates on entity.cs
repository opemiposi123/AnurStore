using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AnurStore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updatesonentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPurchases_AspNetUsers_UserId",
                table: "ProductPurchases");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPurchases_Inventories_InventoryId",
                table: "ProductPurchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSales_AspNetUsers_UserId",
                table: "ProductSales");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSales_Inventories_InventoryId",
                table: "ProductSales");

            migrationBuilder.DropIndex(
                name: "IX_ProductSales_InventoryId",
                table: "ProductSales");

            migrationBuilder.DropIndex(
                name: "IX_ProductSales_UserId",
                table: "ProductSales");

            migrationBuilder.DropIndex(
                name: "IX_ProductPurchases_InventoryId",
                table: "ProductPurchases");

            migrationBuilder.DropIndex(
                name: "IX_ProductPurchases_UserId",
                table: "ProductPurchases");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "29d56834-9a84-4b7c-a399-7218b823a045", "0bb9c2fb-331b-417b-a5f4-6203259e50c7" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "88ce56b0-ad9f-49f7-bd9c-16908c36b385", "326e324d-b21b-4c4d-8665-2a55b3338b44" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "29d56834-9a84-4b7c-a399-7218b823a045");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "88ce56b0-ad9f-49f7-bd9c-16908c36b385");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0bb9c2fb-331b-417b-a5f4-6203259e50c7");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "326e324d-b21b-4c4d-8665-2a55b3338b44");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ProductSizes");

            migrationBuilder.DropColumn(
                name: "InventoryId",
                table: "ProductSales");

            migrationBuilder.DropColumn(
                name: "ReceiptNumber",
                table: "ProductSales");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ProductSales");

            migrationBuilder.DropColumn(
                name: "CostPrice",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MFTDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "InventoryId",
                table: "ProductPurchases");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ProductPurchases");

            migrationBuilder.RenameColumn(
                name: "TotalCost",
                table: "ProductPurchases",
                newName: "Total");

            migrationBuilder.RenameColumn(
                name: "PacksPurchased",
                table: "ProductPurchaseItems",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "CostPerPack",
                table: "ProductPurchaseItems",
                newName: "Rate");

            migrationBuilder.AddColumn<double>(
                name: "Size",
                table: "ProductSizes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<decimal>(
                name: "Discount",
                table: "ProductSales",
                type: "money",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "money");

            migrationBuilder.AlterColumn<string>(
                name: "BrandId",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<decimal>(
                name: "PackPriceMarkup",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPriceMarkup",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<int>(
                name: "PaymentMethod",
                table: "ProductPurchases",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Discount",
                table: "ProductPurchases",
                type: "money",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "money");

            migrationBuilder.AddColumn<bool>(
                name: "IsAddedToInventory",
                table: "ProductPurchases",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "BatchNumber",
                table: "Inventories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "Inventories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "Inventories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StockDate",
                table: "Inventories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "61a2c80b-34ec-4199-8570-f2e0ebe6127a", null, "Cashier", "CASHIER" },
                    { "8a26d0a2-d65c-4bd0-b902-6abf32cc401e", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "CreatedBy", "CreatedOn", "DeletedBy", "DeletedOn", "Email", "EmailConfirmed", "FirstName", "Gender", "IsDeleted", "LastModifiedBy", "LastModifiedOn", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Role", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "737a0c10-3799-45d0-901f-fad3a371222f", 0, "No 5 Manchester Liberty Estate ", "f25198f1-a60e-4695-982f-390627a29f54", null, null, null, null, "cashier@gmail.com", true, "Cashier", 2, null, null, null, "Ameerah", false, null, "CASHIER@GMAIL.COM", "CASHIER", "AQAAAAIAAYagAAAAEA8aEU41Aa43b2hzjqNVwzDsA1Xk/WjuQ0p8GTzyvU/qzGhfyE6SQs05fwB+K9Xbmw==", null, false, 2, "", false, "Cashier" },
                    { "98d7e795-878d-4159-b360-0047fcf895d5", 0, "No 4 Unity Str Aboru", "979b9caa-7319-46a9-8be5-5f26ea2b382f", null, null, null, null, "admin@gmail.com", true, "Admin", 2, null, null, null, "AnurStore", false, null, "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEPSuFGgNdEGe5RSHxHzMcFMhdJ4TdsXqb8BIQFIKO+1Z3kfh9MpepRbt1LahbZeFYQ==", null, false, 1, "", false, "Admin" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "61a2c80b-34ec-4199-8570-f2e0ebe6127a", "737a0c10-3799-45d0-901f-fad3a371222f" },
                    { "8a26d0a2-d65c-4bd0-b902-6abf32cc401e", "98d7e795-878d-4159-b360-0047fcf895d5" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "61a2c80b-34ec-4199-8570-f2e0ebe6127a", "737a0c10-3799-45d0-901f-fad3a371222f" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "8a26d0a2-d65c-4bd0-b902-6abf32cc401e", "98d7e795-878d-4159-b360-0047fcf895d5" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "61a2c80b-34ec-4199-8570-f2e0ebe6127a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8a26d0a2-d65c-4bd0-b902-6abf32cc401e");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "737a0c10-3799-45d0-901f-fad3a371222f");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "98d7e795-878d-4159-b360-0047fcf895d5");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "ProductSizes");

            migrationBuilder.DropColumn(
                name: "PackPriceMarkup",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UnitPriceMarkup",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsAddedToInventory",
                table: "ProductPurchases");

            migrationBuilder.DropColumn(
                name: "BatchNumber",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "StockDate",
                table: "Inventories");

            migrationBuilder.RenameColumn(
                name: "Total",
                table: "ProductPurchases",
                newName: "TotalCost");

            migrationBuilder.RenameColumn(
                name: "Rate",
                table: "ProductPurchaseItems",
                newName: "CostPerPack");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "ProductPurchaseItems",
                newName: "PacksPurchased");

            migrationBuilder.AddColumn<decimal>(
                name: "Quantity",
                table: "ProductSizes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "Discount",
                table: "ProductSales",
                type: "money",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "money",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InventoryId",
                table: "ProductSales",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReceiptNumber",
                table: "ProductSales",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ProductSales",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "BrandId",
                table: "Products",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CostPrice",
                table: "Products",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "MFTDate",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "PaymentMethod",
                table: "ProductPurchases",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Discount",
                table: "ProductPurchases",
                type: "money",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "money",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InventoryId",
                table: "ProductPurchases",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ProductPurchases",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "29d56834-9a84-4b7c-a399-7218b823a045", null, "Admin", "ADMIN" },
                    { "88ce56b0-ad9f-49f7-bd9c-16908c36b385", null, "Cashier", "CASHIER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "CreatedBy", "CreatedOn", "DeletedBy", "DeletedOn", "Email", "EmailConfirmed", "FirstName", "Gender", "IsDeleted", "LastModifiedBy", "LastModifiedOn", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Role", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "0bb9c2fb-331b-417b-a5f4-6203259e50c7", 0, "No 4 Unity Str Aboru", "62d48764-49cd-455b-8a33-b7c701c495c7", null, null, null, null, "admin@gmail.com", true, "Admin", 0, null, null, null, "AnurStore", false, null, "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEDm8wjZgx+9vIXPPiIX2Uw9FtDl3YSvcYTNb18pz/1CUazwJQsWvwwtnYP6Ts7OoIg==", null, false, 1, "", false, "Admin" },
                    { "326e324d-b21b-4c4d-8665-2a55b3338b44", 0, "No 5 Manchester Liberty Estate ", "0d8761cd-d3a7-4ef5-a7a1-b61261082b97", null, null, null, null, "cashier@gmail.com", true, "Cashier", 0, null, null, null, "Ameerah", false, null, "CASHIER@GMAIL.COM", "CASHIER", "AQAAAAIAAYagAAAAENYoX66gOJSJxlfrjK0ptGzSoG0zssUkNF8neJZAkla9bwNLavMuy7KEdl11kMw/qw==", null, false, 2, "", false, "Cashier" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "29d56834-9a84-4b7c-a399-7218b823a045", "0bb9c2fb-331b-417b-a5f4-6203259e50c7" },
                    { "88ce56b0-ad9f-49f7-bd9c-16908c36b385", "326e324d-b21b-4c4d-8665-2a55b3338b44" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductSales_InventoryId",
                table: "ProductSales",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSales_UserId",
                table: "ProductSales",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPurchases_InventoryId",
                table: "ProductPurchases",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPurchases_UserId",
                table: "ProductPurchases",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPurchases_AspNetUsers_UserId",
                table: "ProductPurchases",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPurchases_Inventories_InventoryId",
                table: "ProductPurchases",
                column: "InventoryId",
                principalTable: "Inventories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSales_AspNetUsers_UserId",
                table: "ProductSales",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSales_Inventories_InventoryId",
                table: "ProductSales",
                column: "InventoryId",
                principalTable: "Inventories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
