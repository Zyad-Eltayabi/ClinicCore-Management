using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddMedicalRecordTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MedicalRecordId",
                table: "Prescriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MedicalRecords",
                columns: table => new
                {
                    MedicalRecordID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisitDescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Diagnosis = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AdditionalNotes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalRecords", x => x.MedicalRecordID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_MedicalRecordId",
                table: "Prescriptions",
                column: "MedicalRecordId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_MedicalRecords_MedicalRecordId",
                table: "Prescriptions",
                column: "MedicalRecordId",
                principalTable: "MedicalRecords",
                principalColumn: "MedicalRecordID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_MedicalRecords_MedicalRecordId",
                table: "Prescriptions");

            migrationBuilder.DropTable(
                name: "MedicalRecords");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_MedicalRecordId",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "MedicalRecordId",
                table: "Prescriptions");
        }
    }
}
