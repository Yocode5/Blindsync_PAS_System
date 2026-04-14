using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blindsync_PAS_System.Migrations
{
    /// <inheritdoc />
    public partial class SecurePasswordsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHAloj1lfEtg4SNchRvQCBcjzNVxPzp8gPm8GajL11V8E6n5KYzK5wBuclhsiXDeeQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENnTfu+jmYHUCwVqX/YAeEPKiBvdGpy0tQAzcWm+rCFcNGeQlIpSPOfLTmFSZ80zpg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIPJWwmOtQC+Vy2hNFSiX9aF4QD/0xvefnXWiwuc1UmPYwEbGr6n0/W7QxhhuN8fGA==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "admin123");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "student123");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "super123");
        }
    }
}
