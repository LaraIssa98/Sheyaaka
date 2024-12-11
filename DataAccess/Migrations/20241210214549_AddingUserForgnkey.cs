using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddingUserForgnkey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Stores_StoreID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_StoreID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "StoreID",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Stores",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stores_UserID",
                table: "Stores",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Users_UserID",
                table: "Stores",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Users_UserID",
                table: "Stores");

            migrationBuilder.DropIndex(
                name: "IX_Stores_UserID",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Stores");

            migrationBuilder.AddColumn<int>(
                name: "StoreID",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_StoreID",
                table: "Users",
                column: "StoreID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Stores_StoreID",
                table: "Users",
                column: "StoreID",
                principalTable: "Stores",
                principalColumn: "StoreID");
        }
    }
}
