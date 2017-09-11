using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RegattaManager.Migrations
{
    public partial class Aenderungen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Member_Club_ClubId",
                table: "Member");

            migrationBuilder.DropForeignKey(
                name: "FK_Race_Boatclass_BoatclassId",
                table: "Race");

            migrationBuilder.DropForeignKey(
                name: "FK_Race_Oldclass_OldclassId",
                table: "Race");

            migrationBuilder.DropForeignKey(
                name: "FK_Race_Raceclass_RaceclassId",
                table: "Race");

            migrationBuilder.DropForeignKey(
                name: "FK_Race_Racestatus_RacestatusId",
                table: "Race");

            migrationBuilder.DropForeignKey(
                name: "FK_Race_Regatta_RegattaId",
                table: "Race");

            migrationBuilder.DropForeignKey(
                name: "FK_Regatta_Club_ClubId",
                table: "Regatta");

            migrationBuilder.DropForeignKey(
                name: "FK_Startboat_Club_ClubId",
                table: "Startboat");

            migrationBuilder.DropForeignKey(
                name: "FK_Startboat_Race_RaceId",
                table: "Startboat");

            migrationBuilder.DropForeignKey(
                name: "FK_Startboat_Startboatstatus_StartboatstatusId",
                table: "Startboat");

            migrationBuilder.DropForeignKey(
                name: "FK_StartboatMember_Member_MemberId",
                table: "StartboatMember");

            migrationBuilder.DropForeignKey(
                name: "FK_StartboatMember_Startboat_StartboatId",
                table: "StartboatMember");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Startboatstatus",
                table: "Startboatstatus");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_StartboatMember_StartboatMemberId",
                table: "StartboatMember");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StartboatMember",
                table: "StartboatMember");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Startboat",
                table: "Startboat");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Regatta",
                table: "Regatta");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Racestatus",
                table: "Racestatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Raceclass",
                table: "Raceclass");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Race",
                table: "Race");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Oldclass",
                table: "Oldclass");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Member",
                table: "Member");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Club",
                table: "Club");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Boatclass",
                table: "Boatclass");

            migrationBuilder.RenameTable(
                name: "Startboatstatus",
                newName: "Startboatstati");

            migrationBuilder.RenameTable(
                name: "StartboatMember",
                newName: "StartboatMembers");

            migrationBuilder.RenameTable(
                name: "Startboat",
                newName: "Startboats");

            migrationBuilder.RenameTable(
                name: "Regatta",
                newName: "Regattas");

            migrationBuilder.RenameTable(
                name: "Racestatus",
                newName: "Racestati");

            migrationBuilder.RenameTable(
                name: "Raceclass",
                newName: "Raceclasses");

            migrationBuilder.RenameTable(
                name: "Race",
                newName: "Races");

            migrationBuilder.RenameTable(
                name: "Oldclass",
                newName: "Oldclasses");

            migrationBuilder.RenameTable(
                name: "Member",
                newName: "Members");

            migrationBuilder.RenameTable(
                name: "Club",
                newName: "Clubs");

            migrationBuilder.RenameTable(
                name: "Boatclass",
                newName: "Boatclasses");

            migrationBuilder.RenameIndex(
                name: "IX_StartboatMember_MemberId",
                table: "StartboatMembers",
                newName: "IX_StartboatMembers_MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_Startboat_StartboatstatusId",
                table: "Startboats",
                newName: "IX_Startboats_StartboatstatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Startboat_RaceId",
                table: "Startboats",
                newName: "IX_Startboats_RaceId");

            migrationBuilder.RenameIndex(
                name: "IX_Startboat_ClubId",
                table: "Startboats",
                newName: "IX_Startboats_ClubId");

            migrationBuilder.RenameIndex(
                name: "IX_Regatta_ClubId",
                table: "Regattas",
                newName: "IX_Regattas_ClubId");

            migrationBuilder.RenameIndex(
                name: "IX_Race_RegattaId",
                table: "Races",
                newName: "IX_Races_RegattaId");

            migrationBuilder.RenameIndex(
                name: "IX_Race_RacestatusId",
                table: "Races",
                newName: "IX_Races_RacestatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Race_RaceclassId",
                table: "Races",
                newName: "IX_Races_RaceclassId");

            migrationBuilder.RenameIndex(
                name: "IX_Race_OldclassId",
                table: "Races",
                newName: "IX_Races_OldclassId");

            migrationBuilder.RenameIndex(
                name: "IX_Race_BoatclassId",
                table: "Races",
                newName: "IX_Races_BoatclassId");

            migrationBuilder.RenameIndex(
                name: "IX_Member_ClubId",
                table: "Members",
                newName: "IX_Members_ClubId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Startboatstati",
                table: "Startboatstati",
                column: "StartboatstatusId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_StartboatMembers_StartboatMemberId",
                table: "StartboatMembers",
                column: "StartboatMemberId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StartboatMembers",
                table: "StartboatMembers",
                columns: new[] { "StartboatId", "MemberId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Startboats",
                table: "Startboats",
                column: "StartboatId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Regattas",
                table: "Regattas",
                column: "RegattaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Racestati",
                table: "Racestati",
                column: "RacestatusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Raceclasses",
                table: "Raceclasses",
                column: "RaceclassId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Races",
                table: "Races",
                column: "RaceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Oldclasses",
                table: "Oldclasses",
                column: "OldclassId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Members",
                table: "Members",
                column: "MemberId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clubs",
                table: "Clubs",
                column: "ClubId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Boatclasses",
                table: "Boatclasses",
                column: "BoatclassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Clubs_ClubId",
                table: "Members",
                column: "ClubId",
                principalTable: "Clubs",
                principalColumn: "ClubId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Races_Boatclasses_BoatclassId",
                table: "Races",
                column: "BoatclassId",
                principalTable: "Boatclasses",
                principalColumn: "BoatclassId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Races_Oldclasses_OldclassId",
                table: "Races",
                column: "OldclassId",
                principalTable: "Oldclasses",
                principalColumn: "OldclassId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Races_Raceclasses_RaceclassId",
                table: "Races",
                column: "RaceclassId",
                principalTable: "Raceclasses",
                principalColumn: "RaceclassId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Races_Racestati_RacestatusId",
                table: "Races",
                column: "RacestatusId",
                principalTable: "Racestati",
                principalColumn: "RacestatusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Races_Regattas_RegattaId",
                table: "Races",
                column: "RegattaId",
                principalTable: "Regattas",
                principalColumn: "RegattaId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Regattas_Clubs_ClubId",
                table: "Regattas",
                column: "ClubId",
                principalTable: "Clubs",
                principalColumn: "ClubId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StartboatMembers_Members_MemberId",
                table: "StartboatMembers",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "MemberId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StartboatMembers_Startboats_StartboatId",
                table: "StartboatMembers",
                column: "StartboatId",
                principalTable: "Startboats",
                principalColumn: "StartboatId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Startboats_Clubs_ClubId",
                table: "Startboats",
                column: "ClubId",
                principalTable: "Clubs",
                principalColumn: "ClubId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Startboats_Races_RaceId",
                table: "Startboats",
                column: "RaceId",
                principalTable: "Races",
                principalColumn: "RaceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Startboats_Startboatstati_StartboatstatusId",
                table: "Startboats",
                column: "StartboatstatusId",
                principalTable: "Startboatstati",
                principalColumn: "StartboatstatusId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_Clubs_ClubId",
                table: "Members");

            migrationBuilder.DropForeignKey(
                name: "FK_Races_Boatclasses_BoatclassId",
                table: "Races");

            migrationBuilder.DropForeignKey(
                name: "FK_Races_Oldclasses_OldclassId",
                table: "Races");

            migrationBuilder.DropForeignKey(
                name: "FK_Races_Raceclasses_RaceclassId",
                table: "Races");

            migrationBuilder.DropForeignKey(
                name: "FK_Races_Racestati_RacestatusId",
                table: "Races");

            migrationBuilder.DropForeignKey(
                name: "FK_Races_Regattas_RegattaId",
                table: "Races");

            migrationBuilder.DropForeignKey(
                name: "FK_Regattas_Clubs_ClubId",
                table: "Regattas");

            migrationBuilder.DropForeignKey(
                name: "FK_StartboatMembers_Members_MemberId",
                table: "StartboatMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_StartboatMembers_Startboats_StartboatId",
                table: "StartboatMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_Startboats_Clubs_ClubId",
                table: "Startboats");

            migrationBuilder.DropForeignKey(
                name: "FK_Startboats_Races_RaceId",
                table: "Startboats");

            migrationBuilder.DropForeignKey(
                name: "FK_Startboats_Startboatstati_StartboatstatusId",
                table: "Startboats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Startboatstati",
                table: "Startboatstati");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Startboats",
                table: "Startboats");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_StartboatMembers_StartboatMemberId",
                table: "StartboatMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StartboatMembers",
                table: "StartboatMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Regattas",
                table: "Regattas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Racestati",
                table: "Racestati");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Races",
                table: "Races");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Raceclasses",
                table: "Raceclasses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Oldclasses",
                table: "Oldclasses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Members",
                table: "Members");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clubs",
                table: "Clubs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Boatclasses",
                table: "Boatclasses");

            migrationBuilder.RenameTable(
                name: "Startboatstati",
                newName: "Startboatstatus");

            migrationBuilder.RenameTable(
                name: "Startboats",
                newName: "Startboat");

            migrationBuilder.RenameTable(
                name: "StartboatMembers",
                newName: "StartboatMember");

            migrationBuilder.RenameTable(
                name: "Regattas",
                newName: "Regatta");

            migrationBuilder.RenameTable(
                name: "Racestati",
                newName: "Racestatus");

            migrationBuilder.RenameTable(
                name: "Races",
                newName: "Race");

            migrationBuilder.RenameTable(
                name: "Raceclasses",
                newName: "Raceclass");

            migrationBuilder.RenameTable(
                name: "Oldclasses",
                newName: "Oldclass");

            migrationBuilder.RenameTable(
                name: "Members",
                newName: "Member");

            migrationBuilder.RenameTable(
                name: "Clubs",
                newName: "Club");

            migrationBuilder.RenameTable(
                name: "Boatclasses",
                newName: "Boatclass");

            migrationBuilder.RenameIndex(
                name: "IX_Startboats_StartboatstatusId",
                table: "Startboat",
                newName: "IX_Startboat_StartboatstatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Startboats_RaceId",
                table: "Startboat",
                newName: "IX_Startboat_RaceId");

            migrationBuilder.RenameIndex(
                name: "IX_Startboats_ClubId",
                table: "Startboat",
                newName: "IX_Startboat_ClubId");

            migrationBuilder.RenameIndex(
                name: "IX_StartboatMembers_MemberId",
                table: "StartboatMember",
                newName: "IX_StartboatMember_MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_Regattas_ClubId",
                table: "Regatta",
                newName: "IX_Regatta_ClubId");

            migrationBuilder.RenameIndex(
                name: "IX_Races_RegattaId",
                table: "Race",
                newName: "IX_Race_RegattaId");

            migrationBuilder.RenameIndex(
                name: "IX_Races_RacestatusId",
                table: "Race",
                newName: "IX_Race_RacestatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Races_RaceclassId",
                table: "Race",
                newName: "IX_Race_RaceclassId");

            migrationBuilder.RenameIndex(
                name: "IX_Races_OldclassId",
                table: "Race",
                newName: "IX_Race_OldclassId");

            migrationBuilder.RenameIndex(
                name: "IX_Races_BoatclassId",
                table: "Race",
                newName: "IX_Race_BoatclassId");

            migrationBuilder.RenameIndex(
                name: "IX_Members_ClubId",
                table: "Member",
                newName: "IX_Member_ClubId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Startboatstatus",
                table: "Startboatstatus",
                column: "StartboatstatusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Startboat",
                table: "Startboat",
                column: "StartboatId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_StartboatMember_StartboatMemberId",
                table: "StartboatMember",
                column: "StartboatMemberId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StartboatMember",
                table: "StartboatMember",
                columns: new[] { "StartboatId", "MemberId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Regatta",
                table: "Regatta",
                column: "RegattaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Racestatus",
                table: "Racestatus",
                column: "RacestatusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Race",
                table: "Race",
                column: "RaceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Raceclass",
                table: "Raceclass",
                column: "RaceclassId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Oldclass",
                table: "Oldclass",
                column: "OldclassId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Member",
                table: "Member",
                column: "MemberId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Club",
                table: "Club",
                column: "ClubId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Boatclass",
                table: "Boatclass",
                column: "BoatclassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Member_Club_ClubId",
                table: "Member",
                column: "ClubId",
                principalTable: "Club",
                principalColumn: "ClubId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Race_Boatclass_BoatclassId",
                table: "Race",
                column: "BoatclassId",
                principalTable: "Boatclass",
                principalColumn: "BoatclassId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Race_Oldclass_OldclassId",
                table: "Race",
                column: "OldclassId",
                principalTable: "Oldclass",
                principalColumn: "OldclassId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Race_Raceclass_RaceclassId",
                table: "Race",
                column: "RaceclassId",
                principalTable: "Raceclass",
                principalColumn: "RaceclassId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Race_Racestatus_RacestatusId",
                table: "Race",
                column: "RacestatusId",
                principalTable: "Racestatus",
                principalColumn: "RacestatusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Race_Regatta_RegattaId",
                table: "Race",
                column: "RegattaId",
                principalTable: "Regatta",
                principalColumn: "RegattaId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Regatta_Club_ClubId",
                table: "Regatta",
                column: "ClubId",
                principalTable: "Club",
                principalColumn: "ClubId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Startboat_Club_ClubId",
                table: "Startboat",
                column: "ClubId",
                principalTable: "Club",
                principalColumn: "ClubId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Startboat_Race_RaceId",
                table: "Startboat",
                column: "RaceId",
                principalTable: "Race",
                principalColumn: "RaceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Startboat_Startboatstatus_StartboatstatusId",
                table: "Startboat",
                column: "StartboatstatusId",
                principalTable: "Startboatstatus",
                principalColumn: "StartboatstatusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StartboatMember_Member_MemberId",
                table: "StartboatMember",
                column: "MemberId",
                principalTable: "Member",
                principalColumn: "MemberId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StartboatMember_Startboat_StartboatId",
                table: "StartboatMember",
                column: "StartboatId",
                principalTable: "Startboat",
                principalColumn: "StartboatId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
