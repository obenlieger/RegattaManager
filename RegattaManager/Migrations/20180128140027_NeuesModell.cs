using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RegattaManager.Migrations
{
    public partial class NeuesModell : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
         /*   migrationBuilder.DropForeignKey(
                name: "FK_Competitions_Boatclasses_BoatclassId",
                table: "Competitions");

            migrationBuilder.DropForeignKey(
                name: "FK_Competitions_Raceclasses_RaceclassId",
                table: "Competitions");

            migrationBuilder.DropForeignKey(
                name: "FK_Competitions_Regattas_RegattaId",
                table: "Competitions");*/

            migrationBuilder.DropForeignKey(
                name: "FK_StartingFees_Regattas_RegattaId",
                table: "StartingFees");

            migrationBuilder.DropIndex(
                name: "IX_StartingFees_RegattaId",
                table: "StartingFees");

          /*  migrationBuilder.DropUniqueConstraint(
                name: "AK_Competitions_CompetitionId",
                table: "Competitions");*/

          /*  migrationBuilder.DropPrimaryKey(
                name: "PK_Competitions",
                table: "Competitions");

            migrationBuilder.DropIndex(
                name: "IX_Competitions_RegattaId",
                table: "Competitions");*/

            migrationBuilder.DropColumn(
                name: "RegattaId",
                table: "StartingFees");

        /*    migrationBuilder.DropColumn(
                name: "RegattaId",
                table: "Competitions");*/

      /*      migrationBuilder.AlterColumn<int>(
                name: "CompetitionId",
                table: "Competitions",
                type: "int",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);*/

            migrationBuilder.AddPrimaryKey(
                name: "PK_Competitions",
                table: "Competitions",
                column: "CompetitionId");

            migrationBuilder.CreateTable(
                name: "RegattaCompetitions",
                columns: table => new
                {
                    CompetitionId = table.Column<int>(type: "int", nullable: false),
                    RegattaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegattaCompetitions", x => new { x.CompetitionId, x.RegattaId });
                    table.ForeignKey(
                        name: "FK_RegattaCompetitions_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "CompetitionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegattaCompetitions_Regattas_RegattaId",
                        column: x => x.RegattaId,
                        principalTable: "Regattas",
                        principalColumn: "RegattaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegattaStartingFees",
                columns: table => new
                {
                    StartingFeeId = table.Column<int>(type: "int", nullable: false),
                    RegattaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegattaStartingFees", x => new { x.StartingFeeId, x.RegattaId });
                    table.ForeignKey(
                        name: "FK_RegattaStartingFees_Regattas_RegattaId",
                        column: x => x.RegattaId,
                        principalTable: "Regattas",
                        principalColumn: "RegattaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegattaStartingFees_StartingFees_StartingFeeId",
                        column: x => x.StartingFeeId,
                        principalTable: "StartingFees",
                        principalColumn: "StartingFeeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_BoatclassId",
                table: "Competitions",
                column: "BoatclassId");

            migrationBuilder.CreateIndex(
                name: "IX_RegattaCompetitions_RegattaId",
                table: "RegattaCompetitions",
                column: "RegattaId");

            migrationBuilder.CreateIndex(
                name: "IX_RegattaStartingFees_RegattaId",
                table: "RegattaStartingFees",
                column: "RegattaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Competitions_Boatclasses_BoatclassId",
                table: "Competitions",
                column: "BoatclassId",
                principalTable: "Boatclasses",
                principalColumn: "BoatclassId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Competitions_Raceclasses_RaceclassId",
                table: "Competitions",
                column: "RaceclassId",
                principalTable: "Raceclasses",
                principalColumn: "RaceclassId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
    /*        migrationBuilder.DropForeignKey(
                name: "FK_Competitions_Boatclasses_BoatclassId",
                table: "Competitions");

            migrationBuilder.DropForeignKey(
                name: "FK_Competitions_Raceclasses_RaceclassId",
                table: "Competitions");*/

            migrationBuilder.DropTable(
                name: "RegattaCompetitions");

            migrationBuilder.DropTable(
                name: "RegattaStartingFees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Competitions",
                table: "Competitions");

            migrationBuilder.DropIndex(
                name: "IX_Competitions_BoatclassId",
                table: "Competitions");

            migrationBuilder.AddColumn<int>(
                name: "RegattaId",
                table: "StartingFees",
                nullable: false,
                defaultValue: 0);

        /*    migrationBuilder.AlterColumn<int>(
                name: "CompetitionId",
                table: "Competitions",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);*/

            migrationBuilder.AddColumn<int>(
                name: "RegattaId",
                table: "Competitions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Competitions_CompetitionId",
                table: "Competitions",
                column: "CompetitionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Competitions",
                table: "Competitions",
                columns: new[] { "BoatclassId", "RaceclassId", "RegattaId" });

            migrationBuilder.CreateIndex(
                name: "IX_StartingFees_RegattaId",
                table: "StartingFees",
                column: "RegattaId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_RegattaId",
                table: "Competitions",
                column: "RegattaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Competitions_Boatclasses_BoatclassId",
                table: "Competitions",
                column: "BoatclassId",
                principalTable: "Boatclasses",
                principalColumn: "BoatclassId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Competitions_Raceclasses_RaceclassId",
                table: "Competitions",
                column: "RaceclassId",
                principalTable: "Raceclasses",
                principalColumn: "RaceclassId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Competitions_Regattas_RegattaId",
                table: "Competitions",
                column: "RegattaId",
                principalTable: "Regattas",
                principalColumn: "RegattaId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StartingFees_Regattas_RegattaId",
                table: "StartingFees",
                column: "RegattaId",
                principalTable: "Regattas",
                principalColumn: "RegattaId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
