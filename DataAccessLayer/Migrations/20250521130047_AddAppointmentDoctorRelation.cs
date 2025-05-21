using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddAppointmentDoctorRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DoctorID",
                table: "Appointment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_DoctorID",
                table: "Appointment",
                column: "DoctorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Doctors_DoctorID",
                table: "Appointment",
                column: "DoctorID",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Doctors_DoctorID",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_DoctorID",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "DoctorID",
                table: "Appointment");
        }
    }
}
