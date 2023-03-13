using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalCompany.Infrastructure.Migrations
{
    public partial class rentalstoreandavailablecarsupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AvailableCars_RentalStores_RentalStoreId",
                table: "AvailableCars");

            migrationBuilder.AddColumn<int>(
                name: "AvailableCarId",
                table: "RentalStores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "RentalStoreId",
                table: "AvailableCars",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AvailableCars_RentalStores_RentalStoreId",
                table: "AvailableCars",
                column: "RentalStoreId",
                principalTable: "RentalStores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AvailableCars_RentalStores_RentalStoreId",
                table: "AvailableCars");

            migrationBuilder.DropColumn(
                name: "AvailableCarId",
                table: "RentalStores");

            migrationBuilder.AlterColumn<int>(
                name: "RentalStoreId",
                table: "AvailableCars",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AvailableCars_RentalStores_RentalStoreId",
                table: "AvailableCars",
                column: "RentalStoreId",
                principalTable: "RentalStores",
                principalColumn: "Id");
        }
    }
}
