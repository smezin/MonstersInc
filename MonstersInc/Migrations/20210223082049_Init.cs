using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MonstersAPI.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkDays",
                columns: table => new
                {
                    WorkDayId = table.Column<string>(nullable: false),
                    IntimidatorId = table.Column<string>(nullable: false),
                    Begin = table.Column<DateTime>(nullable: false),
                    End = table.Column<DateTime>(nullable: false),
                    EnergyGoal = table.Column<int>(nullable: false),
                    EnergyCollected = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkDays", x => x.WorkDayId);
                });

            migrationBuilder.CreateTable(
                name: "DepletedDoor",
                columns: table => new
                {
                    DepletedDoorId = table.Column<string>(nullable: false),
                    DoorId = table.Column<string>(nullable: false),
                    OpenedAt = table.Column<DateTime>(nullable: false),
                    ClosedAt = table.Column<DateTime>(nullable: false),
                    WorkDayId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepletedDoor", x => x.DepletedDoorId);
                    table.ForeignKey(
                        name: "FK_DepletedDoor_WorkDays_WorkDayId",
                        column: x => x.WorkDayId,
                        principalTable: "WorkDays",
                        principalColumn: "WorkDayId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Doors",
                columns: table => new
                {
                    DoorId = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Energy = table.Column<int>(nullable: false),
                    LastUsed = table.Column<DateTime>(nullable: false),
                    IsOpen = table.Column<bool>(nullable: false),
                    WorkDayId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doors", x => x.DoorId);
                    table.ForeignKey(
                        name: "FK_Doors_WorkDays_WorkDayId",
                        column: x => x.WorkDayId,
                        principalTable: "WorkDays",
                        principalColumn: "WorkDayId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepletedDoor_WorkDayId",
                table: "DepletedDoor",
                column: "WorkDayId");

            migrationBuilder.CreateIndex(
                name: "IX_Doors_WorkDayId",
                table: "Doors",
                column: "WorkDayId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepletedDoor");

            migrationBuilder.DropTable(
                name: "Doors");

            migrationBuilder.DropTable(
                name: "WorkDays");
        }
    }
}
