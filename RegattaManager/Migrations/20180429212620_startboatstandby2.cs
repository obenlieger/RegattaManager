using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RegattaManager.Migrations
{
    public partial class startboatstandby2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StartboatStandby_Members_MemberId",
                table: "StartboatStandby");

            migrationBuilder.DropForeignKey(
                name: "FK_StartboatStandby_Startboats_StartboatId",
                table: "StartboatStandby");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StartboatStandby",
                table: "StartboatStandby");

            migrationBuilder.RenameTable(
                name: "StartboatStandby",
                newName: "StartboatStandbys");

            migrationBuilder.RenameIndex(
                name: "IX_StartboatStandby_MemberId",
                table: "StartboatStandbys",
                newName: "IX_StartboatStandbys_MemberId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StartboatStandbys",
                table: "StartboatStandbys",
                columns: new[] { "StartboatId", "MemberId" });

            migrationBuilder.AddForeignKey(
                name: "FK_StartboatStandbys_Members_MemberId",
                table: "StartboatStandbys",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "MemberId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StartboatStandbys_Startboats_StartboatId",
                table: "StartboatStandbys",
                column: "StartboatId",
                principalTable: "Startboats",
                principalColumn: "StartboatId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StartboatStandbys_Members_MemberId",
                table: "StartboatStandbys");

            migrationBuilder.DropForeignKey(
                name: "FK_StartboatStandbys_Startboats_StartboatId",
                table: "StartboatStandbys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StartboatStandbys",
                table: "StartboatStandbys");

            migrationBuilder.RenameTable(
                name: "StartboatStandbys",
                newName: "StartboatStandby");

            migrationBuilder.RenameIndex(
                name: "IX_StartboatStandbys_MemberId",
                table: "StartboatStandby",
                newName: "IX_StartboatStandby_MemberId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StartboatStandby",
                table: "StartboatStandby",
                columns: new[] { "StartboatId", "MemberId" });

            migrationBuilder.AddForeignKey(
                name: "FK_StartboatStandby_Members_MemberId",
                table: "StartboatStandby",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "MemberId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StartboatStandby_Startboats_StartboatId",
                table: "StartboatStandby",
                column: "StartboatId",
                principalTable: "Startboats",
                principalColumn: "StartboatId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
