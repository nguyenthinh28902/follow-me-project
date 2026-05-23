using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class GlobalIdentifierAddIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_GlobalIdentifier_Email",
                table: "GlobalIdentifiers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GlobalIdentifier_UserName",
                table: "GlobalIdentifiers",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GlobalIdentifier_Email",
                table: "GlobalIdentifiers");

            migrationBuilder.DropIndex(
                name: "IX_GlobalIdentifier_UserName",
                table: "GlobalIdentifiers");
        }
    }
}
