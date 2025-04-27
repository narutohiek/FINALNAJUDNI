using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialWelfarre.Data.Migrations
{
    /// <inheritdoc />
    public partial class FoodPackNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Brgy_Cert_Path",
                table: "ApplicationFoodPack",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Valid_ID_Path",
                table: "ApplicationFoodPack",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brgy_Cert_Path",
                table: "ApplicationFoodPack");

            migrationBuilder.DropColumn(
                name: "Valid_ID_Path",
                table: "ApplicationFoodPack");
        }
    }
}
