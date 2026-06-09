using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChirayuHospitalMVC.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBillingModelV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Billings",
                newName: "TreatmentCost");

            migrationBuilder.AddColumn<decimal>(
                name: "AdditionalCharges",
                table: "Billings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ConsultationFee",
                table: "Billings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MedicineCost",
                table: "Billings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "Billings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalCharges",
                table: "Billings");

            migrationBuilder.DropColumn(
                name: "ConsultationFee",
                table: "Billings");

            migrationBuilder.DropColumn(
                name: "MedicineCost",
                table: "Billings");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "Billings");

            migrationBuilder.RenameColumn(
                name: "TreatmentCost",
                table: "Billings",
                newName: "Amount");
        }
    }
}
