using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class SeedRolesFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "aa19c2e5-3de4-471e-a31e-bbb344567899", "efcc233c-cffa-4777-8d15-333344445555", "MedicalAdmin", "MEDICALADMIN" },
                    { "bb95a2dc-cba4-4fcb-9432-ccc455667788", "aaad1111-bbdd-4567-ae88-444455556666", "Receptionist", "RECEPTIONIST" },
                    { "d1f488a3-6730-47cb-a0e1-aaa2342a1bc1", "cf7c214e-6bc4-4ef5-9cd6-111122223333", "SuperAdmin", "SUPERADMIN" },
                    { "f6a822c6-2ad8-4d32-a0d1-eee23453bc22", "442899dd-c394-4a76-87f0-222233334444", "ClinicManager", "CLINICMANAGER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aa19c2e5-3de4-471e-a31e-bbb344567899");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bb95a2dc-cba4-4fcb-9432-ccc455667788");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d1f488a3-6730-47cb-a0e1-aaa2342a1bc1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f6a822c6-2ad8-4d32-a0d1-eee23453bc22");
        }
    }
}
