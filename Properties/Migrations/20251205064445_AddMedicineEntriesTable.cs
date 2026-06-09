using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChirayuHospitalMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddMedicineEntriesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicineEntry_Treatments_TreatmentId",
                table: "MedicineEntry");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MedicineEntry",
                table: "MedicineEntry");

            migrationBuilder.RenameTable(
                name: "MedicineEntry",
                newName: "MedicineEntries");

            migrationBuilder.RenameIndex(
                name: "IX_MedicineEntry_TreatmentId",
                table: "MedicineEntries",
                newName: "IX_MedicineEntries_TreatmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MedicineEntries",
                table: "MedicineEntries",
                column: "MedicineEntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineEntries_Treatments_TreatmentId",
                table: "MedicineEntries",
                column: "TreatmentId",
                principalTable: "Treatments",
                principalColumn: "TreatmentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicineEntries_Treatments_TreatmentId",
                table: "MedicineEntries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MedicineEntries",
                table: "MedicineEntries");

            migrationBuilder.RenameTable(
                name: "MedicineEntries",
                newName: "MedicineEntry");

            migrationBuilder.RenameIndex(
                name: "IX_MedicineEntries_TreatmentId",
                table: "MedicineEntry",
                newName: "IX_MedicineEntry_TreatmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MedicineEntry",
                table: "MedicineEntry",
                column: "MedicineEntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineEntry_Treatments_TreatmentId",
                table: "MedicineEntry",
                column: "TreatmentId",
                principalTable: "Treatments",
                principalColumn: "TreatmentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
