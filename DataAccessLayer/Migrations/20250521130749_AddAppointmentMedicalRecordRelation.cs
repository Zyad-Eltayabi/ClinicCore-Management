using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddAppointmentMedicalRecordRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MedicalRecordID",
                table: "Appointment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_MedicalRecordID",
                table: "Appointment",
                column: "MedicalRecordID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_MedicalRecords_MedicalRecordID",
                table: "Appointment",
                column: "MedicalRecordID",
                principalTable: "MedicalRecords",
                principalColumn: "MedicalRecordID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_MedicalRecords_MedicalRecordID",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_MedicalRecordID",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "MedicalRecordID",
                table: "Appointment");
        }
    }
}
