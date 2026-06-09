using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChirayuHospitalMVC.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHospitalSetting_New : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentDuration",
                table: "HospitalSettings");

            migrationBuilder.DropColumn(
                name: "DefaultTreatmentCharge",
                table: "HospitalSettings");

            migrationBuilder.DropColumn(
                name: "MaxAppointmentsPerDay",
                table: "HospitalSettings");

            migrationBuilder.RenameColumn(
                name: "WorkingHours",
                table: "HospitalSettings",
                newName: "Tagline");

            migrationBuilder.RenameColumn(
                name: "GSTPercentage",
                table: "HospitalSettings",
                newName: "DefaultTreatmentFee");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "HospitalSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContactNumber",
                table: "HospitalSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "HospitalSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tagline",
                table: "HospitalSettings",
                newName: "WorkingHours");

            migrationBuilder.RenameColumn(
                name: "DefaultTreatmentFee",
                table: "HospitalSettings",
                newName: "GSTPercentage");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "HospitalSettings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ContactNumber",
                table: "HospitalSettings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "HospitalSettings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "AppointmentDuration",
                table: "HospitalSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "DefaultTreatmentCharge",
                table: "HospitalSettings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "MaxAppointmentsPerDay",
                table: "HospitalSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
