using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepositoryLayer.Migrations
{
    /// <inheritdoc />
    public partial class editTransectionPaymenteditprametare : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "paymentTransactions");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "paymentTransactions");

            migrationBuilder.DropColumn(
                name: "SubscriptionEnd",
                table: "paymentTransactions");

            migrationBuilder.DropColumn(
                name: "SubscriptionStart",
                table: "paymentTransactions");

            migrationBuilder.RenameColumn(
                name: "orderId",
                table: "paymentTransactions",
                newName: "SubscriptionPaypalId");

            migrationBuilder.RenameIndex(
                name: "IX_paymentTransactions_orderId",
                table: "paymentTransactions",
                newName: "IX_paymentTransactions_SubscriptionPaypalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubscriptionPaypalId",
                table: "paymentTransactions",
                newName: "orderId");

            migrationBuilder.RenameIndex(
                name: "IX_paymentTransactions_SubscriptionPaypalId",
                table: "paymentTransactions",
                newName: "IX_paymentTransactions_orderId");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "paymentTransactions",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "paymentTransactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "SubscriptionEnd",
                table: "paymentTransactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "SubscriptionStart",
                table: "paymentTransactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
