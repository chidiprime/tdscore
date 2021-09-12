using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TDS.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransactionLedgers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Hash = table.Column<string>(nullable: true),
                    ReferenceId = table.Column<string>(nullable: true),
                    Paging_Token = table.Column<string>(nullable: true),
                    Account = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Created_at = table.Column<string>(nullable: true),
                    Type_I = table.Column<string>(nullable: true),
                    Asset_Type = table.Column<string>(nullable: true),
                    Amount = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionLedgers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Account = table.Column<string>(nullable: true),
                    Secret = table.Column<string>(nullable: true),
                    IsActivated = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WalletTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Hash = table.Column<string>(nullable: true),
                    SourceAccount = table.Column<string>(nullable: true),
                    DestinationAccount = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Narration = table.Column<string>(nullable: true),
                    IsLedgerProcessed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletTransactions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_Account",
                table: "Wallets",
                column: "Account",
                unique: true,
                filter: "[Account] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionLedgers");

            migrationBuilder.DropTable(
                name: "Wallets");

            migrationBuilder.DropTable(
                name: "WalletTransactions");
        }
    }
}
