using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalCompany.Infrastructure.Migrations
{
    public partial class addedrentsummary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RentSummaries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalCost = table.Column<double>(type: "float", nullable: false),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    RentHeaderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentSummaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RentSummaries_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RentSummaries_RentHeaders_RentHeaderId",
                        column: x => x.RentHeaderId,
                        principalTable: "RentHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RentSummaries_CarId",
                table: "RentSummaries",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_RentSummaries_RentHeaderId",
                table: "RentSummaries",
                column: "RentHeaderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RentSummaries");
        }
    }
}
