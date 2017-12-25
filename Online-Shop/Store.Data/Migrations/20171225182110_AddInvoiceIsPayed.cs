namespace Store.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddInvoiceIsPayed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPayed",
                table: "Invoices",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPayed",
                table: "Invoices");
        }
    }
}
