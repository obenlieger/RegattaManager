using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RegattaManager.Migrations
{
    public partial class RaceDraw : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RaceDraws",
                columns: table => new
                {
                    RaceDrawId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EndlaufCount = table.Column<int>(type: "int", nullable: false),
                    HoffnungslaufCount = table.Column<int>(type: "int", nullable: false),
                    ReportedSBCountFrom = table.Column<int>(type: "int", nullable: false),
                    ReportedSBCountTo = table.Column<int>(type: "int", nullable: false),
                    VorlaufCount = table.Column<int>(type: "int", nullable: false),
                    ZwischenlaufCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaceDraws", x => x.RaceDrawId);
                });

            migrationBuilder.CreateTable(
                name: "RaceTyps",
                columns: table => new
                {
                    RaceTypId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isFinal = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaceTyps", x => x.RaceTypId);
                });

            migrationBuilder.CreateTable(
                name: "RaceDrawRules",
                columns: table => new
                {
                    RaceDrawRuleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PlacementFrom = table.Column<int>(type: "int", nullable: false),
                    PlacementTo = table.Column<int>(type: "int", nullable: false),
                    RaceDrawId = table.Column<int>(type: "int", nullable: false),
                    RaceSequence = table.Column<int>(type: "int", nullable: false),
                    RaceTypId = table.Column<int>(type: "int", nullable: false),
                    ToRaceSequence = table.Column<int>(type: "int", nullable: false),
                    ToRaceTypId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaceDrawRules", x => x.RaceDrawRuleId);
                    table.ForeignKey(
                        name: "FK_RaceDrawRules_RaceDraws_RaceDrawId",
                        column: x => x.RaceDrawId,
                        principalTable: "RaceDraws",
                        principalColumn: "RaceDrawId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RaceDrawRules_RaceTyps_RaceTypId",
                        column: x => x.RaceTypId,
                        principalTable: "RaceTyps",
                        principalColumn: "RaceTypId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RaceDrawRules_RaceDrawId",
                table: "RaceDrawRules",
                column: "RaceDrawId");

            migrationBuilder.CreateIndex(
                name: "IX_RaceDrawRules_RaceTypId",
                table: "RaceDrawRules",
                column: "RaceTypId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RaceDrawRules");

            migrationBuilder.DropTable(
                name: "RaceDraws");

            migrationBuilder.DropTable(
                name: "RaceTyps");
        }
    }
}
