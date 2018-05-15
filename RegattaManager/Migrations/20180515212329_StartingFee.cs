using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RegattaManager.Migrations
{
    public partial class StartingFee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StartingFees_Oldclasses_OldclassId",
                table: "StartingFees");

            migrationBuilder.DropIndex(
                name: "IX_StartingFees_OldclassId",
                table: "StartingFees");

            migrationBuilder.DropColumn(
                name: "OldclassId",
                table: "StartingFees");

            migrationBuilder.AddColumn<int>(
                name: "FromOldclassId",
                table: "StartingFees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ToOldclassId",
                table: "StartingFees",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromOldclassId",
                table: "StartingFees");

            migrationBuilder.DropColumn(
                name: "ToOldclassId",
                table: "StartingFees");

            migrationBuilder.AddColumn<int>(
                name: "OldclassId",
                table: "StartingFees",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StartingFees_OldclassId",
                table: "StartingFees",
                column: "OldclassId");

            migrationBuilder.AddForeignKey(
                name: "FK_StartingFees_Oldclasses_OldclassId",
                table: "StartingFees",
                column: "OldclassId",
                principalTable: "Oldclasses",
                principalColumn: "OldclassId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
