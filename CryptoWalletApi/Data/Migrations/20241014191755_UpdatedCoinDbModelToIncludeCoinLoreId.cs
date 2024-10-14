using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoWalletApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedCoinDbModelToIncludeCoinLoreId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoinLoreId",
                table: "Coins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoinLoreId",
                table: "Coins");
        }
    }
}
