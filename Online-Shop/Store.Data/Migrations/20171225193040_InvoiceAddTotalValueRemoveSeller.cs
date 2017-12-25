namespace Store.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class InvoiceAddTotalValueRemoveSeller : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_AspNetUsers_SellerId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_SellerId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "Invoices");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SellerId",
                table: "Invoices",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_SellerId",
                table: "Invoices",
                column: "SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_AspNetUsers_SellerId",
                table: "Invoices",
                column: "SellerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
