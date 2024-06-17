using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoworkingSpace_Manager_ManagerId",
                table: "CoworkingSpace");

            migrationBuilder.AlterColumn<int>(
                name: "ManagerId",
                table: "CoworkingSpace",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_CoworkingSpace_Manager_ManagerId",
                table: "CoworkingSpace",
                column: "ManagerId",
                principalTable: "Manager",
                principalColumn: "ManagerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoworkingSpace_Manager_ManagerId",
                table: "CoworkingSpace");

            migrationBuilder.AlterColumn<int>(
                name: "ManagerId",
                table: "CoworkingSpace",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CoworkingSpace_Manager_ManagerId",
                table: "CoworkingSpace",
                column: "ManagerId",
                principalTable: "Manager",
                principalColumn: "ManagerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
