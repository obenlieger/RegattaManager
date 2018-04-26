using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RegattaManager.Migrations
{
    public partial class ohnesbmnr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_StartboatMembers_StartboatMemberId",
                table: "StartboatMembers");

            migrationBuilder.DropColumn(
                name: "StartboatMemberId",
                table: "StartboatMembers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StartboatMemberId",
                table: "StartboatMembers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_StartboatMembers_StartboatMemberId",
                table: "StartboatMembers",
                column: "StartboatMemberId");
        }
    }
}
