using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChirayuHospitalMVC.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHospitalSettingModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppointmentDuration",
                table: "HospitalSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "DefaultConsultationFee",
                table: "HospitalSettings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DefaultTreatmentCharge",
                table: "HospitalSettings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "GSTPercentage",
                table: "HospitalSettings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "HospitalSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxAppointmentsPerDay",
                table: "HospitalSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "HospitalSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkingHours",
                table: "HospitalSettings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentDuration",
                table: "HospitalSettings");

            migrationBuilder.DropColumn(
                name: "DefaultConsultationFee",
                table: "HospitalSettings");

            migrationBuilder.DropColumn(
                name: "DefaultTreatmentCharge",
                table: "HospitalSettings");

            migrationBuilder.DropColumn(
                name: "GSTPercentage",
                table: "HospitalSettings");

            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "HospitalSettings");

            migrationBuilder.DropColumn(
                name: "MaxAppointmentsPerDay",
                table: "HospitalSettings");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "HospitalSettings");

            migrationBuilder.DropColumn(
                name: "WorkingHours",
                table: "HospitalSettings");
        }
    }
}
