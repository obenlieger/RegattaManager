using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RegattaManager.Migrations
{
    public partial class Meldung : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RentYear",
                table: "Members",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RentedToClubId",
                table: "Members",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "isRented",
                table: "Members",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ReportedRace",
                columns: table => new
                {
                    ReportedRaceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CompetitionId = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldclassId = table.Column<int>(type: "int", nullable: false),
                    RaceCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportedRace", x => x.ReportedRaceId);
                    table.ForeignKey(
                        name: "FK_ReportedRace_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "CompetitionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportedRace_Oldclasses_OldclassId",
                        column: x => x.OldclassId,
                        principalTable: "Oldclasses",
                        principalColumn: "OldclassId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportedStartboat",
                columns: table => new
                {
                    ReportedStartboatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClubId = table.Column<int>(type: "int", nullable: false),
                    CompetitionId = table.Column<int>(type: "int", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegattaId = table.Column<int>(type: "int", nullable: false),
                    ReportedRaceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportedStartboat", x => x.ReportedStartboatId);
                    table.ForeignKey(
                        name: "FK_ReportedStartboat_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "ClubId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportedStartboat_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "CompetitionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportedStartboat_Regattas_RegattaId",
                        column: x => x.RegattaId,
                        principalTable: "Regattas",
                        principalColumn: "RegattaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportedStartboat_ReportedRace_ReportedRaceId",
                        column: x => x.ReportedRaceId,
                        principalTable: "ReportedRace",
                        principalColumn: "ReportedRaceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportedStartboatMember",
                columns: table => new
                {
                    ReportedStartboatId = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    Seatnumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportedStartboatMember", x => new { x.ReportedStartboatId, x.MemberId });
                    table.ForeignKey(
                        name: "FK_ReportedStartboatMember_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportedStartboatMember_ReportedStartboat_ReportedStartboatId",
                        column: x => x.ReportedStartboatId,
                        principalTable: "ReportedStartboat",
                        principalColumn: "ReportedStartboatId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReportedStartboatStandby",
                columns: table => new
                {
                    ReportedStartboatId = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    Standbynumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportedStartboatStandby", x => new { x.ReportedStartboatId, x.MemberId });
                    table.ForeignKey(
                        name: "FK_ReportedStartboatStandby_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportedStartboatStandby_ReportedStartboat_ReportedStartboatId",
                        column: x => x.ReportedStartboatId,
                        principalTable: "ReportedStartboat",
                        principalColumn: "ReportedStartboatId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportedRace_CompetitionId",
                table: "ReportedRace",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportedRace_OldclassId",
                table: "ReportedRace",
                column: "OldclassId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportedStartboat_ClubId",
                table: "ReportedStartboat",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportedStartboat_CompetitionId",
                table: "ReportedStartboat",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportedStartboat_RegattaId",
                table: "ReportedStartboat",
                column: "RegattaId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportedStartboat_ReportedRaceId",
                table: "ReportedStartboat",
                column: "ReportedRaceId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportedStartboatMember_MemberId",
                table: "ReportedStartboatMember",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportedStartboatStandby_MemberId",
                table: "ReportedStartboatStandby",
                column: "MemberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportedStartboatMember");

            migrationBuilder.DropTable(
                name: "ReportedStartboatStandby");

            migrationBuilder.DropTable(
                name: "ReportedStartboat");

            migrationBuilder.DropTable(
                name: "ReportedRace");

            migrationBuilder.DropColumn(
                name: "RentYear",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "RentedToClubId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "isRented",
                table: "Members");
        }
    }
}
