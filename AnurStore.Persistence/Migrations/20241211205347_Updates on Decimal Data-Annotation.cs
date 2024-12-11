using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AnurStore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatesonDecimalDataAnnotation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItem_Invoices_InvoiceId",
                table: "InvoiceItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoiceItem",
                table: "InvoiceItem");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "fc60e126-819f-49c2-95f2-0c1cb053c05d", "959e663c-2ae9-4ba5-ba4b-1cff321e2563" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "ee776064-3bde-42f6-80b0-c3fbab9a3d5f", "c47eb573-95f1-4d90-8dd9-dceebba7c90d" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ee776064-3bde-42f6-80b0-c3fbab9a3d5f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fc60e126-819f-49c2-95f2-0c1cb053c05d");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "959e663c-2ae9-4ba5-ba4b-1cff321e2563");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c47eb573-95f1-4d90-8dd9-dceebba7c90d");

            migrationBuilder.RenameTable(
                name: "InvoiceItem",
                newName: "InvoiceItems");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceItem_InvoiceId",
                table: "InvoiceItems",
                newName: "IX_InvoiceItems_InvoiceId");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAmount",
                table: "ProductSales",
                type: "money",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Discount",
                table: "ProductSales",
                type: "money",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SubTotal",
                table: "ProductSaleItems",
                type: "money",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "Products",
                type: "money",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PricePerPack",
                table: "Products",
                type: "money",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CostPrice",
                table: "Products",
                type: "money",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalCost",
                table: "ProductPurchases",
                type: "money",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Discount",
                table: "ProductPurchases",
                type: "money",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Payments",
                type: "money",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAmount",
                table: "Invoices",
                type: "money",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "NetAmount",
                table: "Invoices",
                type: "money",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Discount",
                table: "Invoices",
                type: "money",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "Accounts",
                type: "money",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "AccountEntries",
                type: "money",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoiceItems",
                table: "InvoiceItems",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "352e54ea-37c3-4731-b187-af5ba2e5a03d", null, "Cashier", "CASHIER" },
                    { "e3082bef-b7b8-4022-9e81-7fd49fabb58b", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "CreatedBy", "DeletedBy", "DeletedOn", "Email", "EmailConfirmed", "FirstName", "Gender", "IsDeleted", "LastModifiedBy", "LastModifiedOn", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Role", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "221261b9-25f2-4a83-a8f0-237f453419b5", 0, "No 4 Unity Str Aboru", "b02fda35-0b4e-44e8-84a5-f78d2843764e", null, null, null, "admin@gmail.com", true, "Admin", 0, null, null, null, "AnurStore", false, null, "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEDgVIuPkC8uNdTo5ZfmX8hKCvMa1iNJtGevH88etvayEyiGKEhu7iCsE8jyv+zQ1ww==", null, false, 1, "", false, "Admin" },
                    { "db2dd9ba-c28e-480b-ae81-52fdeeee9107", 0, "No 5 Manchester Liberty Estate ", "0adc7fa5-ef29-448b-a8f9-b9a8da581f8d", null, null, null, "cashier@gmail.com", true, "Cashier", 0, null, null, null, "Ameerah", false, null, "CASHIER@GMAIL.COM", "CASHIER", "AQAAAAIAAYagAAAAEBtjWywb7N2rRuZZsG2e95bDkeI1uiX/7lDhtNtbUKfdqbppwSqJJy9ZPgAPefLdlQ==", null, false, 2, "", false, "Cashier" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "e3082bef-b7b8-4022-9e81-7fd49fabb58b", "221261b9-25f2-4a83-a8f0-237f453419b5" },
                    { "352e54ea-37c3-4731-b187-af5ba2e5a03d", "db2dd9ba-c28e-480b-ae81-52fdeeee9107" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceItems_Invoices_InvoiceId",
                table: "InvoiceItems",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItems_Invoices_InvoiceId",
                table: "InvoiceItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoiceItems",
                table: "InvoiceItems");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "e3082bef-b7b8-4022-9e81-7fd49fabb58b", "221261b9-25f2-4a83-a8f0-237f453419b5" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "352e54ea-37c3-4731-b187-af5ba2e5a03d", "db2dd9ba-c28e-480b-ae81-52fdeeee9107" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "352e54ea-37c3-4731-b187-af5ba2e5a03d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e3082bef-b7b8-4022-9e81-7fd49fabb58b");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "221261b9-25f2-4a83-a8f0-237f453419b5");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "db2dd9ba-c28e-480b-ae81-52fdeeee9107");

            migrationBuilder.RenameTable(
                name: "InvoiceItems",
                newName: "InvoiceItem");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceItems_InvoiceId",
                table: "InvoiceItem",
                newName: "IX_InvoiceItem_InvoiceId");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAmount",
                table: "ProductSales",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money");

            migrationBuilder.AlterColumn<decimal>(
                name: "Discount",
                table: "ProductSales",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money");

            migrationBuilder.AlterColumn<decimal>(
                name: "SubTotal",
                table: "ProductSaleItems",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money");

            migrationBuilder.AlterColumn<decimal>(
                name: "PricePerPack",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money");

            migrationBuilder.AlterColumn<decimal>(
                name: "CostPrice",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalCost",
                table: "ProductPurchases",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money");

            migrationBuilder.AlterColumn<decimal>(
                name: "Discount",
                table: "ProductPurchases",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Payments",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAmount",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money");

            migrationBuilder.AlterColumn<decimal>(
                name: "NetAmount",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money");

            migrationBuilder.AlterColumn<decimal>(
                name: "Discount",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money");

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "Accounts",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "AccountEntries",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoiceItem",
                table: "InvoiceItem",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "ee776064-3bde-42f6-80b0-c3fbab9a3d5f", null, "Cashier", "CASHIER" },
                    { "fc60e126-819f-49c2-95f2-0c1cb053c05d", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "CreatedBy", "DeletedBy", "DeletedOn", "Email", "EmailConfirmed", "FirstName", "Gender", "IsDeleted", "LastModifiedBy", "LastModifiedOn", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Role", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "959e663c-2ae9-4ba5-ba4b-1cff321e2563", 0, "No 4 Unity Str Aboru", "a9cff666-d070-434f-9fb2-d6787512c066", null, null, null, "admin@gmail.com", true, "Admin", 0, null, null, null, "AnurStore", false, null, "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEJwkr/Vy0CkBriOsBALo3MsvfmUwAQ4N3ZHBPRRGkylB7VMgCQves9vMJ7kSfheyPA==", null, false, 1, "", false, "Admin" },
                    { "c47eb573-95f1-4d90-8dd9-dceebba7c90d", 0, "No 5 Manchester Liberty Estate ", "8de71a42-bb4f-4129-99cf-307165c402c1", null, null, null, "cashier@gmail.com", true, "Cashier", 0, null, null, null, "Ameerah", false, null, "CASHIER@GMAIL.COM", "CASHIER", "AQAAAAIAAYagAAAAENZWqXwGJ0ao6fCaGGj+8CqQcMF2oB7QdIFedcjQZp1oxsH3O7G7L6SxUco6Sm6gNw==", null, false, 2, "", false, "Cashier" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "fc60e126-819f-49c2-95f2-0c1cb053c05d", "959e663c-2ae9-4ba5-ba4b-1cff321e2563" },
                    { "ee776064-3bde-42f6-80b0-c3fbab9a3d5f", "c47eb573-95f1-4d90-8dd9-dceebba7c90d" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceItem_Invoices_InvoiceId",
                table: "InvoiceItem",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
