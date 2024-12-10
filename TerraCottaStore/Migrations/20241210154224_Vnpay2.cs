using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TerraCottaStore.Migrations
{
    /// <inheritdoc />
    public partial class Vnpay2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderMethob",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderMethob",
                table: "Orders");
        }
    }
}
