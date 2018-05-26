using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RegattaManager.Migrations
{
    public partial class abteilungslauf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isAbteilungslauf",
                table: "ReportedRaces",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isAbteilungslauf",
                table: "Races",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isAbteilungslauf",
                table: "RaceDraws",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isAbteilungslauf",
                table: "ReportedRaces");

            migrationBuilder.DropColumn(
                name: "isAbteilungslauf",
                table: "Races");

            migrationBuilder.DropColumn(
                name: "isAbteilungslauf",
                table: "RaceDraws");
        }
    }
}
