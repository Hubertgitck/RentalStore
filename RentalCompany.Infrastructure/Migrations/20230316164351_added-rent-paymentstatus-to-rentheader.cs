using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalCompany.Infrastructure.Migrations
{
    public partial class addedrentpaymentstatustorentheader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RentPaymentStatus",
                table: "RentHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RentPaymentStatus",
                table: "RentHeaders");
        }
    }
}
