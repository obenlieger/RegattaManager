using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RegattaManager.Migrations
{
    public partial class RegattaRaceclass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegattaRaceclass",
                columns: table => new
                {
                    RegattaId = table.Column<int>(type: "int", nullable: false),
                    RaceclassId = table.Column<int>(type: "int", nullable: false),
                    RegattaRaceclassId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegattaRaceclass", x => new { x.RegattaId, x.RaceclassId });
                    table.UniqueConstraint("AK_RegattaRaceclass_RegattaRaceclassId", x => x.RegattaRaceclassId);
                    table.ForeignKey(
                        name: "FK_RegattaRaceclass_Raceclasses_RaceclassId",
                        column: x => x.RaceclassId,
                        principalTable: "Raceclasses",
                        principalColumn: "RaceclassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegattaRaceclass_Regattas_RegattaId",
                        column: x => x.RegattaId,
                        principalTable: "Regattas",
                        principalColumn: "RegattaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegattaRaceclass_RaceclassId",
                table: "RegattaRaceclass",
                column: "RaceclassId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegattaRaceclass");
        }
    }
}
