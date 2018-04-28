using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RegattaManager.Migrations
{
    public partial class rsbid_in_sb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReportedStartboatId",
                table: "Startboats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Startboats_ReportedStartboatId",
                table: "Startboats",
                column: "ReportedStartboatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Startboats_ReportedStartboats_ReportedStartboatId",
                table: "Startboats",
                column: "ReportedStartboatId",
                principalTable: "ReportedStartboats",
                principalColumn: "ReportedStartboatId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Startboats_ReportedStartboats_ReportedStartboatId",
                table: "Startboats");

            migrationBuilder.DropIndex(
                name: "IX_Startboats_ReportedStartboatId",
                table: "Startboats");

            migrationBuilder.DropColumn(
                name: "ReportedStartboatId",
                table: "Startboats");
        }
    }
}
