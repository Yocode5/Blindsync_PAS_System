using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blindsync_PAS_System.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignedAtDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AssignedAt",
                table: "Projects",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEttODngrG85GttERO6AjO9KgT37sPNWou+iquPledkfhmreG0FcmEGKp2lCCyp6Dw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEfn+Al3BpwuRBE25RxFMUsxj2xE2XA/a1Ej5jkFMqw1Nc46TxOBwoI0Xpz2rHk+Jg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPtfiBTIq5tME0Z2DISITCNN/ZBy+nIt9uzSux6uql+z4TBo54PNGXK285j2YQ3TsQ==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedAt",
                table: "Projects");

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
    }
}
