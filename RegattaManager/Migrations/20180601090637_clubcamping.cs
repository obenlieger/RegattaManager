using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RegattaManager.Migrations
{
    public partial class clubcamping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClubCampingFees",
                columns: table => new
                {
                    CampingFeeId = table.Column<int>(type: "int", nullable: false),
                    ClubId = table.Column<int>(type: "int", nullable: false),
                    ClubCampingFeeId = table.Column<int>(type: "int", nullable: false),
                    campingcount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubCampingFees", x => new { x.CampingFeeId, x.ClubId });
                    table.UniqueConstraint("AK_ClubCampingFees_ClubCampingFeeId", x => x.ClubCampingFeeId);
                    table.ForeignKey(
                        name: "FK_ClubCampingFees_CampingFees_CampingFeeId",
                        column: x => x.CampingFeeId,
                        principalTable: "CampingFees",
                        principalColumn: "CampingFeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClubCampingFees_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "ClubId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClubCampingFees_ClubId",
                table: "ClubCampingFees",
                column: "ClubId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClubCampingFees");
        }
    }
}
