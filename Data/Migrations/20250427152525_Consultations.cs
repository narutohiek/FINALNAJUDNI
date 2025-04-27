using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialWelfarre.Data.Migrations
{
    /// <inheritdoc />
    public partial class Consultations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BeneficiaryId",
                table: "Consultations");

            migrationBuilder.RenameColumn(
                name: "consultation_status",
                table: "Consultations",
                newName: "Consultation_status");

            migrationBuilder.AlterColumn<int>(
                name: "Consultation_status",
                table: "Consultations",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Birth_Cert",
                table: "Consultations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Birth_Cert_Path",
                table: "Consultations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Brgy_Cert",
                table: "Consultations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Brgy_Cert_Path",
                table: "Consultations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Proof_SoloParent",
                table: "Consultations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Proof_SoloParent_Path",
                table: "Consultations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Valid_ID",
                table: "Consultations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Valid_ID_Path",
                table: "Consultations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "x1_Pic",
                table: "Consultations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "x1_Pic_Path",
                table: "Consultations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Birth_Cert",
                table: "Consultations");

            migrationBuilder.DropColumn(
                name: "Birth_Cert_Path",
                table: "Consultations");

            migrationBuilder.DropColumn(
                name: "Brgy_Cert",
                table: "Consultations");

            migrationBuilder.DropColumn(
                name: "Brgy_Cert_Path",
                table: "Consultations");

            migrationBuilder.DropColumn(
                name: "Proof_SoloParent",
                table: "Consultations");

            migrationBuilder.DropColumn(
                name: "Proof_SoloParent_Path",
                table: "Consultations");

            migrationBuilder.DropColumn(
                name: "Valid_ID",
                table: "Consultations");

            migrationBuilder.DropColumn(
                name: "Valid_ID_Path",
                table: "Consultations");

            migrationBuilder.DropColumn(
                name: "x1_Pic",
                table: "Consultations");

            migrationBuilder.DropColumn(
                name: "x1_Pic_Path",
                table: "Consultations");

            migrationBuilder.RenameColumn(
                name: "Consultation_status",
                table: "Consultations",
                newName: "consultation_status");

            migrationBuilder.AlterColumn<string>(
                name: "consultation_status",
                table: "Consultations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BeneficiaryId",
                table: "Consultations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
