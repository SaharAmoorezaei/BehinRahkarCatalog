using Microsoft.EntityFrameworkCore.Migrations;

namespace BehinRahkar_Test.Data.Sql.Data.Migrations
{
    public partial class CreatedbAddIsConfirm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmed",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsConfirmed",
                table: "Products");
        }
    }
}
