using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniClubDataAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedClubsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Clubs",
                columns: new[] { "Id", "CreatedDate", "Description", "Email", "FacebookUrl", "InstagramUrl", "LogoUrl", "Name", "TwitterUrl", "UniversityId", "UpdatedDate", "WebsiteUrl" },
                values: new object[] { 1, new DateTime(2023, 2, 16, 22, 0, 2, 249, DateTimeKind.Local).AddTicks(1778), "Test description", "asd@asd.com", "facebook.com", "instagram.com", "logo.jpg", "Test Club", "twitter.com", 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "test.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Clubs",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
