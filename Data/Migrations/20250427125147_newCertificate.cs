using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialWelfarre.Data.Migrations
{
    /// <inheritdoc />
    public partial class newCertificate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Brgy_Cert_Path",
                table: "Certificate_Of_Indigencies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Valid_ID_Path",
                table: "Certificate_Of_Indigencies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brgy_Cert_Path",
                table: "Certificate_Of_Indigencies");

            migrationBuilder.DropColumn(
                name: "Valid_ID_Path",
                table: "Certificate_Of_Indigencies");
        }
    }
}
