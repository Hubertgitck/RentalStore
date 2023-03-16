using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalCompany.Infrastructure.Migrations
{
    public partial class addedpaymentrelatedpreopertiestorentheader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "RentHeaders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PaymentIntendId",
                table: "RentHeaders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "RentHeaders",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "RentHeaders");

            migrationBuilder.DropColumn(
                name: "PaymentIntendId",
                table: "RentHeaders");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "RentHeaders");
        }
    }
}
