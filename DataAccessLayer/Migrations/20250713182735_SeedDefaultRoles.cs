using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class SeedDefaultRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3eb9590e-d019-4191-beee-29056960db7b", "6758d1a9-b871-4b5d-934f-a9bb7e199700", "MedicalAdmin", "MEDICALADMIN" },
                    { "a830e884-bf5a-4c4b-96d6-9bd84ee22405", "8cfca4d5-c605-4739-8f9e-2a6209df66e0", "Receptionist ", "RECEPTIONIST " },
                    { "e2b4238e-5145-4729-97b7-222f2168a84b", "77d9ae83-3e99-43a8-9466-4426092b1d06", "SuperAdmin", "SUPERADMIN" },
                    { "f4ca3253-89c5-4b8b-ad4b-dabae4c67d15", "d8a57416-ddf1-4ec1-b051-1f4884b9be59", "ClinicManager", "CLINICMANAGER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3eb9590e-d019-4191-beee-29056960db7b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a830e884-bf5a-4c4b-96d6-9bd84ee22405");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e2b4238e-5145-4729-97b7-222f2168a84b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f4ca3253-89c5-4b8b-ad4b-dabae4c67d15");
        }
    }
}
