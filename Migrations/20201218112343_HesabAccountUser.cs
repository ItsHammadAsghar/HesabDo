using Microsoft.EntityFrameworkCore.Migrations;

namespace HesabDo.Migrations
{
    public partial class HesabAccountUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HesabAccountUser_HesabAccounts_HesabAccountID",
                table: "HesabAccountUser");

            migrationBuilder.DropForeignKey(
                name: "FK_HesabAccountUser_AspNetUsers_UserID",
                table: "HesabAccountUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HesabAccountUser",
                table: "HesabAccountUser");

            migrationBuilder.RenameTable(
                name: "HesabAccountUser",
                newName: "HesabAccountUsers");

            migrationBuilder.RenameIndex(
                name: "IX_HesabAccountUser_HesabAccountID",
                table: "HesabAccountUsers",
                newName: "IX_HesabAccountUsers_HesabAccountID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HesabAccountUsers",
                table: "HesabAccountUsers",
                columns: new[] { "UserID", "HesabAccountID" });

            migrationBuilder.AddForeignKey(
                name: "FK_HesabAccountUsers_HesabAccounts_HesabAccountID",
                table: "HesabAccountUsers",
                column: "HesabAccountID",
                principalTable: "HesabAccounts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HesabAccountUsers_AspNetUsers_UserID",
                table: "HesabAccountUsers",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HesabAccountUsers_HesabAccounts_HesabAccountID",
                table: "HesabAccountUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_HesabAccountUsers_AspNetUsers_UserID",
                table: "HesabAccountUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HesabAccountUsers",
                table: "HesabAccountUsers");

            migrationBuilder.RenameTable(
                name: "HesabAccountUsers",
                newName: "HesabAccountUser");

            migrationBuilder.RenameIndex(
                name: "IX_HesabAccountUsers_HesabAccountID",
                table: "HesabAccountUser",
                newName: "IX_HesabAccountUser_HesabAccountID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HesabAccountUser",
                table: "HesabAccountUser",
                columns: new[] { "UserID", "HesabAccountID" });

            migrationBuilder.AddForeignKey(
                name: "FK_HesabAccountUser_HesabAccounts_HesabAccountID",
                table: "HesabAccountUser",
                column: "HesabAccountID",
                principalTable: "HesabAccounts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HesabAccountUser_AspNetUsers_UserID",
                table: "HesabAccountUser",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
