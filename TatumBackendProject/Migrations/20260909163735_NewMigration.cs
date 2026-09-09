using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TatumBackendProject.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RegisrationOtp",
                table: "Users",
                newName: "RegistrationOtp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RegistrationOtp",
                table: "Users",
                newName: "RegisrationOtp");
        }
    }
}
