using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bulletingboard.Migrations
{
    /// <inheritdoc />
    public partial class change_user_entity_field_name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResetTokenExpireIn",
                table: "Users",
                newName: "ResetTokenExpireAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResetTokenExpireAt",
                table: "Users",
                newName: "ResetTokenExpireIn");
        }
    }
}
