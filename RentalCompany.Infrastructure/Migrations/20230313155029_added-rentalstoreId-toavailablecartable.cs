using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalCompany.Infrastructure.Migrations
{
    public partial class addedrentalstoreIdtoavailablecartable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableCarId",
                table: "RentalStores");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvailableCarId",
                table: "RentalStores",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
