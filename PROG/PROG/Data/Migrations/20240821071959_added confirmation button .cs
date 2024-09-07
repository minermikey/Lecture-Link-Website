using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROG.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedconfirmationbutton : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "Information",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "Information");
        }
    }
}
