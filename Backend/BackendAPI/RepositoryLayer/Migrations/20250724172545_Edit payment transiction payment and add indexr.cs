using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepositoryLayer.Migrations
{
    /// <inheritdoc />
    public partial class Editpaymenttransictionpaymentandaddindexr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "orderId",
                table: "paymentTransactions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_paymentTransactions_orderId",
                table: "paymentTransactions",
                column: "orderId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_paymentTransactions_orderId",
                table: "paymentTransactions");

            migrationBuilder.DropColumn(
                name: "orderId",
                table: "paymentTransactions");
        }
    }
}
