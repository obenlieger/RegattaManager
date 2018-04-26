using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RegattaManager.Migrations
{
    public partial class raceracetyp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportedRace_Competitions_CompetitionId",
                table: "ReportedRace");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportedRace_Oldclasses_OldclassId",
                table: "ReportedRace");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportedRace_Regattas_RegattaId",
                table: "ReportedRace");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportedStartboat_Clubs_ClubId",
                table: "ReportedStartboat");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportedStartboat_Competitions_CompetitionId",
                table: "ReportedStartboat");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportedStartboat_Regattas_RegattaId",
                table: "ReportedStartboat");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportedStartboat_ReportedRace_ReportedRaceId",
                table: "ReportedStartboat");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportedStartboatMember_Members_MemberId",
                table: "ReportedStartboatMember");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportedStartboatMember_ReportedStartboat_ReportedStartboatId",
                table: "ReportedStartboatMember");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportedStartboatStandby_Members_MemberId",
                table: "ReportedStartboatStandby");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportedStartboatStandby_ReportedStartboat_ReportedStartboatId",
                table: "ReportedStartboatStandby");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReportedStartboatStandby",
                table: "ReportedStartboatStandby");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReportedStartboatMember",
                table: "ReportedStartboatMember");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReportedStartboat",
                table: "ReportedStartboat");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReportedRace",
                table: "ReportedRace");

            migrationBuilder.RenameTable(
                name: "ReportedStartboatStandby",
                newName: "ReportedStartboatStandbys");

            migrationBuilder.RenameTable(
                name: "ReportedStartboatMember",
                newName: "ReportedStartboatMembers");

            migrationBuilder.RenameTable(
                name: "ReportedStartboat",
                newName: "ReportedStartboats");

            migrationBuilder.RenameTable(
                name: "ReportedRace",
                newName: "ReportedRaces");

            migrationBuilder.RenameIndex(
                name: "IX_ReportedStartboatStandby_MemberId",
                table: "ReportedStartboatStandbys",
                newName: "IX_ReportedStartboatStandbys_MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_ReportedStartboatMember_MemberId",
                table: "ReportedStartboatMembers",
                newName: "IX_ReportedStartboatMembers_MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_ReportedStartboat_ReportedRaceId",
                table: "ReportedStartboats",
                newName: "IX_ReportedStartboats_ReportedRaceId");

            migrationBuilder.RenameIndex(
                name: "IX_ReportedStartboat_RegattaId",
                table: "ReportedStartboats",
                newName: "IX_ReportedStartboats_RegattaId");

            migrationBuilder.RenameIndex(
                name: "IX_ReportedStartboat_CompetitionId",
                table: "ReportedStartboats",
                newName: "IX_ReportedStartboats_CompetitionId");

            migrationBuilder.RenameIndex(
                name: "IX_ReportedStartboat_ClubId",
                table: "ReportedStartboats",
                newName: "IX_ReportedStartboats_ClubId");

            migrationBuilder.RenameIndex(
                name: "IX_ReportedRace_RegattaId",
                table: "ReportedRaces",
                newName: "IX_ReportedRaces_RegattaId");

            migrationBuilder.RenameIndex(
                name: "IX_ReportedRace_OldclassId",
                table: "ReportedRaces",
                newName: "IX_ReportedRaces_OldclassId");

            migrationBuilder.RenameIndex(
                name: "IX_ReportedRace_CompetitionId",
                table: "ReportedRaces",
                newName: "IX_ReportedRaces_CompetitionId");

            migrationBuilder.AddColumn<int>(
                name: "RaceTypId",
                table: "Races",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReportedStartboatStandbys",
                table: "ReportedStartboatStandbys",
                columns: new[] { "ReportedStartboatId", "MemberId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReportedStartboatMembers",
                table: "ReportedStartboatMembers",
                columns: new[] { "ReportedStartboatId", "MemberId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReportedStartboats",
                table: "ReportedStartboats",
                column: "ReportedStartboatId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReportedRaces",
                table: "ReportedRaces",
                column: "ReportedRaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Races_RaceTypId",
                table: "Races",
                column: "RaceTypId");

            migrationBuilder.AddForeignKey(
                name: "FK_Races_RaceTyps_RaceTypId",
                table: "Races",
                column: "RaceTypId",
                principalTable: "RaceTyps",
                principalColumn: "RaceTypId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedRaces_Competitions_CompetitionId",
                table: "ReportedRaces",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "CompetitionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedRaces_Oldclasses_OldclassId",
                table: "ReportedRaces",
                column: "OldclassId",
                principalTable: "Oldclasses",
                principalColumn: "OldclassId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedRaces_Regattas_RegattaId",
                table: "ReportedRaces",
                column: "RegattaId",
                principalTable: "Regattas",
                principalColumn: "RegattaId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedStartboatMembers_Members_MemberId",
                table: "ReportedStartboatMembers",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "MemberId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedStartboatMembers_ReportedStartboats_ReportedStartboatId",
                table: "ReportedStartboatMembers",
                column: "ReportedStartboatId",
                principalTable: "ReportedStartboats",
                principalColumn: "ReportedStartboatId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedStartboats_Clubs_ClubId",
                table: "ReportedStartboats",
                column: "ClubId",
                principalTable: "Clubs",
                principalColumn: "ClubId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedStartboats_Competitions_CompetitionId",
                table: "ReportedStartboats",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "CompetitionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedStartboats_Regattas_RegattaId",
                table: "ReportedStartboats",
                column: "RegattaId",
                principalTable: "Regattas",
                principalColumn: "RegattaId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedStartboats_ReportedRaces_ReportedRaceId",
                table: "ReportedStartboats",
                column: "ReportedRaceId",
                principalTable: "ReportedRaces",
                principalColumn: "ReportedRaceId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedStartboatStandbys_Members_MemberId",
                table: "ReportedStartboatStandbys",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "MemberId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedStartboatStandbys_ReportedStartboats_ReportedStartboatId",
                table: "ReportedStartboatStandbys",
                column: "ReportedStartboatId",
                principalTable: "ReportedStartboats",
                principalColumn: "ReportedStartboatId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Races_RaceTyps_RaceTypId",
                table: "Races");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportedRaces_Competitions_CompetitionId",
                table: "ReportedRaces");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportedRaces_Oldclasses_OldclassId",
                table: "ReportedRaces");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportedRaces_Regattas_RegattaId",
                table: "ReportedRaces");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportedStartboatMembers_Members_MemberId",
                table: "ReportedStartboatMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportedStartboatMembers_ReportedStartboats_ReportedStartboatId",
                table: "ReportedStartboatMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportedStartboats_Clubs_ClubId",
                table: "ReportedStartboats");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportedStartboats_Competitions_CompetitionId",
                table: "ReportedStartboats");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportedStartboats_Regattas_RegattaId",
                table: "ReportedStartboats");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportedStartboats_ReportedRaces_ReportedRaceId",
                table: "ReportedStartboats");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportedStartboatStandbys_Members_MemberId",
                table: "ReportedStartboatStandbys");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportedStartboatStandbys_ReportedStartboats_ReportedStartboatId",
                table: "ReportedStartboatStandbys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReportedStartboatStandbys",
                table: "ReportedStartboatStandbys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReportedStartboats",
                table: "ReportedStartboats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReportedStartboatMembers",
                table: "ReportedStartboatMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReportedRaces",
                table: "ReportedRaces");

            migrationBuilder.DropIndex(
                name: "IX_Races_RaceTypId",
                table: "Races");

            migrationBuilder.DropColumn(
                name: "RaceTypId",
                table: "Races");

            migrationBuilder.RenameTable(
                name: "ReportedStartboatStandbys",
                newName: "ReportedStartboatStandby");

            migrationBuilder.RenameTable(
                name: "ReportedStartboats",
                newName: "ReportedStartboat");

            migrationBuilder.RenameTable(
                name: "ReportedStartboatMembers",
                newName: "ReportedStartboatMember");

            migrationBuilder.RenameTable(
                name: "ReportedRaces",
                newName: "ReportedRace");

            migrationBuilder.RenameIndex(
                name: "IX_ReportedStartboatStandbys_MemberId",
                table: "ReportedStartboatStandby",
                newName: "IX_ReportedStartboatStandby_MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_ReportedStartboats_ReportedRaceId",
                table: "ReportedStartboat",
                newName: "IX_ReportedStartboat_ReportedRaceId");

            migrationBuilder.RenameIndex(
                name: "IX_ReportedStartboats_RegattaId",
                table: "ReportedStartboat",
                newName: "IX_ReportedStartboat_RegattaId");

            migrationBuilder.RenameIndex(
                name: "IX_ReportedStartboats_CompetitionId",
                table: "ReportedStartboat",
                newName: "IX_ReportedStartboat_CompetitionId");

            migrationBuilder.RenameIndex(
                name: "IX_ReportedStartboats_ClubId",
                table: "ReportedStartboat",
                newName: "IX_ReportedStartboat_ClubId");

            migrationBuilder.RenameIndex(
                name: "IX_ReportedStartboatMembers_MemberId",
                table: "ReportedStartboatMember",
                newName: "IX_ReportedStartboatMember_MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_ReportedRaces_RegattaId",
                table: "ReportedRace",
                newName: "IX_ReportedRace_RegattaId");

            migrationBuilder.RenameIndex(
                name: "IX_ReportedRaces_OldclassId",
                table: "ReportedRace",
                newName: "IX_ReportedRace_OldclassId");

            migrationBuilder.RenameIndex(
                name: "IX_ReportedRaces_CompetitionId",
                table: "ReportedRace",
                newName: "IX_ReportedRace_CompetitionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReportedStartboatStandby",
                table: "ReportedStartboatStandby",
                columns: new[] { "ReportedStartboatId", "MemberId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReportedStartboat",
                table: "ReportedStartboat",
                column: "ReportedStartboatId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReportedStartboatMember",
                table: "ReportedStartboatMember",
                columns: new[] { "ReportedStartboatId", "MemberId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReportedRace",
                table: "ReportedRace",
                column: "ReportedRaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedRace_Competitions_CompetitionId",
                table: "ReportedRace",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "CompetitionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedRace_Oldclasses_OldclassId",
                table: "ReportedRace",
                column: "OldclassId",
                principalTable: "Oldclasses",
                principalColumn: "OldclassId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedRace_Regattas_RegattaId",
                table: "ReportedRace",
                column: "RegattaId",
                principalTable: "Regattas",
                principalColumn: "RegattaId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedStartboat_Clubs_ClubId",
                table: "ReportedStartboat",
                column: "ClubId",
                principalTable: "Clubs",
                principalColumn: "ClubId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedStartboat_Competitions_CompetitionId",
                table: "ReportedStartboat",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "CompetitionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedStartboat_Regattas_RegattaId",
                table: "ReportedStartboat",
                column: "RegattaId",
                principalTable: "Regattas",
                principalColumn: "RegattaId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedStartboat_ReportedRace_ReportedRaceId",
                table: "ReportedStartboat",
                column: "ReportedRaceId",
                principalTable: "ReportedRace",
                principalColumn: "ReportedRaceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedStartboatMember_Members_MemberId",
                table: "ReportedStartboatMember",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "MemberId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedStartboatMember_ReportedStartboat_ReportedStartboatId",
                table: "ReportedStartboatMember",
                column: "ReportedStartboatId",
                principalTable: "ReportedStartboat",
                principalColumn: "ReportedStartboatId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedStartboatStandby_Members_MemberId",
                table: "ReportedStartboatStandby",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "MemberId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedStartboatStandby_ReportedStartboat_ReportedStartboatId",
                table: "ReportedStartboatStandby",
                column: "ReportedStartboatId",
                principalTable: "ReportedStartboat",
                principalColumn: "ReportedStartboatId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
