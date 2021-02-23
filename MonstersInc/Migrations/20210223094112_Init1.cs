using Microsoft.EntityFrameworkCore.Migrations;

namespace MonstersAPI.Migrations
{
    public partial class Init1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepletedDoor_WorkDays_WorkDayId",
                table: "DepletedDoor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DepletedDoor",
                table: "DepletedDoor");

            migrationBuilder.RenameTable(
                name: "DepletedDoor",
                newName: "DepletedDoors");

            migrationBuilder.RenameIndex(
                name: "IX_DepletedDoor_WorkDayId",
                table: "DepletedDoors",
                newName: "IX_DepletedDoors_WorkDayId");

            migrationBuilder.AlterColumn<string>(
                name: "WorkDayId",
                table: "DepletedDoors",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DepletedDoors",
                table: "DepletedDoors",
                column: "DepletedDoorId");

            migrationBuilder.AddForeignKey(
                name: "FK_DepletedDoors_WorkDays_WorkDayId",
                table: "DepletedDoors",
                column: "WorkDayId",
                principalTable: "WorkDays",
                principalColumn: "WorkDayId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepletedDoors_WorkDays_WorkDayId",
                table: "DepletedDoors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DepletedDoors",
                table: "DepletedDoors");

            migrationBuilder.RenameTable(
                name: "DepletedDoors",
                newName: "DepletedDoor");

            migrationBuilder.RenameIndex(
                name: "IX_DepletedDoors_WorkDayId",
                table: "DepletedDoor",
                newName: "IX_DepletedDoor_WorkDayId");

            migrationBuilder.AlterColumn<string>(
                name: "WorkDayId",
                table: "DepletedDoor",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_DepletedDoor",
                table: "DepletedDoor",
                column: "DepletedDoorId");

            migrationBuilder.AddForeignKey(
                name: "FK_DepletedDoor_WorkDays_WorkDayId",
                table: "DepletedDoor",
                column: "WorkDayId",
                principalTable: "WorkDays",
                principalColumn: "WorkDayId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
