using Microsoft.EntityFrameworkCore.Migrations;

namespace QuotesAPI.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuotesMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyValueRangeFrom = table.Column<int>(type: "int", nullable: false),
                    PropertyValueRangeTo = table.Column<int>(type: "int", nullable: false),
                    BusinessValueRangeFrom = table.Column<int>(type: "int", nullable: false),
                    BusinessValueRangeTo = table.Column<int>(type: "int", nullable: false),
                    PropertyType = table.Column<int>(type: "int", nullable: false),
                    Quote = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotesMaster", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuotesMaster");
        }
    }
}
