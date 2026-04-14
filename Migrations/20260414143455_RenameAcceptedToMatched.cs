using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blindsync_PAS_System.Migrations
{
    /// <inheritdoc />
    public partial class RenameAcceptedToMatched : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDzoTLf/Ki8UBW8aQxACDxMu4parDLvSR6xpCN/U43PYmk7nfsmFMqK1UfS5YWLgkw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELd+wuFgQqhQfivKtRvzQC1ymjBy/vm65AtIKMSVyT+DeM7PVUxxcnsxjaLliivCpQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBD/Gf7sUq3HOFOjEL10iqzr/exSAaktZT/yTdS+v7pd3AvkwPbQM3eghbeQs+Srdw==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
