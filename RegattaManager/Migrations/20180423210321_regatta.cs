using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RegattaManager.Migrations
{
    public partial class regatta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegattaId",
                table: "ReportedRace",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ReportedRace_RegattaId",
                table: "ReportedRace",
                column: "RegattaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedRace_Regattas_RegattaId",
                table: "ReportedRace",
                column: "RegattaId",
                principalTable: "Regattas",
                principalColumn: "RegattaId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportedRace_Regattas_RegattaId",
                table: "ReportedRace");

            migrationBuilder.DropIndex(
                name: "IX_ReportedRace_RegattaId",
                table: "ReportedRace");

            migrationBuilder.DropColumn(
                name: "RegattaId",
                table: "ReportedRace");
        }
    }
}
