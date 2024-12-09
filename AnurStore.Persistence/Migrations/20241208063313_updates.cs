using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AnurStore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1c85fcd0-5d75-4c17-a7e5-7bf83024901d", "a1d0fb9c-6efc-4003-9844-77f2a5ea8b69" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "b34b60c5-2622-429d-88be-50d450438ebf", "ae7c27f1-4bbe-4aea-b3a5-38de3702171b" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1c85fcd0-5d75-4c17-a7e5-7bf83024901d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b34b60c5-2622-429d-88be-50d450438ebf");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1d0fb9c-6efc-4003-9844-77f2a5ea8b69");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ae7c27f1-4bbe-4aea-b3a5-38de3702171b");

            migrationBuilder.AddColumn<string>(
                name: "CustomerCare",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InvoiceItem",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InvoiceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceItem_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItem_InvoiceId",
                table: "InvoiceItem",
                column: "InvoiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceItem");

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

            migrationBuilder.DropColumn(
                name: "CustomerCare",
                table: "Invoices");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1c85fcd0-5d75-4c17-a7e5-7bf83024901d", null, "Admin", "ADMIN" },
                    { "b34b60c5-2622-429d-88be-50d450438ebf", null, "Cashier", "CASHIER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "CreatedBy", "DeletedBy", "DeletedOn", "Email", "EmailConfirmed", "FirstName", "Gender", "IsDeleted", "LastModifiedBy", "LastModifiedOn", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Role", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "a1d0fb9c-6efc-4003-9844-77f2a5ea8b69", 0, "No 4 Unity Str Aboru", "b1dfa863-5941-48d6-b85c-49d48852e245", null, null, null, "admin@gmail.com", true, "Admin", 0, null, null, null, "AnurStore", false, null, "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEEgK+H8XMExiMcGEm8punz7/6hWCcV4MfIHWfmw3a8nyEWo2z8QsXyLreNFQePcUfg==", null, false, 1, "", false, "Admin" },
                    { "ae7c27f1-4bbe-4aea-b3a5-38de3702171b", 0, "No 5 Manchester Liberty Estate ", "fe32676c-35c5-4ac1-b775-7414f2238f30", null, null, null, "cashier@gmail.com", true, "Cashier", 0, null, null, null, "Ameerah", false, null, "CASHIER@GMAIL.COM", "CASHIER", "AQAAAAIAAYagAAAAEOhGZDiry7VrKdr28LQdCewZOg7JUhRXdZUNXtjzl207bE9rRWAo7perr9o2nron/Q==", null, false, 2, "", false, "Cashier" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "1c85fcd0-5d75-4c17-a7e5-7bf83024901d", "a1d0fb9c-6efc-4003-9844-77f2a5ea8b69" },
                    { "b34b60c5-2622-429d-88be-50d450438ebf", "ae7c27f1-4bbe-4aea-b3a5-38de3702171b" }
                });
        }
    }
}
