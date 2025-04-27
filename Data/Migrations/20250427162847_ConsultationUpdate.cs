using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialWelfarre.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConsultationUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Consulatation_Date",
                table: "Consultations",
                newName: "Consultation_Date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Consultation_Date",
                table: "Consultations",
                newName: "Consulatation_Date");
        }
    }
}
