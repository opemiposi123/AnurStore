using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AnurStore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedCreatedonfield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "CreatedOn",
                table: "AspNetUsers");

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
        }
    }
}
