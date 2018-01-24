using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RegattaManager.Migrations
{
    public partial class ModelErweiterungMeldung : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegattaRaceclass");

            migrationBuilder.AddColumn<string>(
                name: "Accomodation",
                table: "Regattas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Awards",
                table: "Regattas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Catering",
                table: "Regattas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Regattas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Judge",
                table: "Regattas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportAddress",
                table: "Regattas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportFax",
                table: "Regattas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportMail",
                table: "Regattas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReportOpening",
                table: "Regattas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ReportSchedule",
                table: "Regattas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ReportTel",
                table: "Regattas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportText",
                table: "Regattas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScheduleText",
                table: "Regattas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Security",
                table: "Regattas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Startslots",
                table: "Regattas",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "SubscriberFee",
                table: "Regattas",
                type: "real",
                nullable: true,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "WaterId",
                table: "Regattas",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Waterdepth",
                table: "Regattas",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CampingFees",
                columns: table => new
                {
                    CampingFeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<float>(type: "real", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampingFees", x => x.CampingFeeId);
                });

            migrationBuilder.CreateTable(
                name: "Competitions",
                columns: table => new
                {
                    BoatclassId = table.Column<int>(type: "int", nullable: false),
                    RaceclassId = table.Column<int>(type: "int", nullable: false),
                    RegattaId = table.Column<int>(type: "int", nullable: false),
                    CompetitionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competitions", x => new { x.BoatclassId, x.RaceclassId, x.RegattaId });
                    table.UniqueConstraint("AK_Competitions_CompetitionId", x => x.CompetitionId);
                    table.ForeignKey(
                        name: "FK_Competitions_Boatclasses_BoatclassId",
                        column: x => x.BoatclassId,
                        principalTable: "Boatclasses",
                        principalColumn: "BoatclassId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Competitions_Raceclasses_RaceclassId",
                        column: x => x.RaceclassId,
                        principalTable: "Raceclasses",
                        principalColumn: "RaceclassId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Competitions_Regattas_RegattaId",
                        column: x => x.RegattaId,
                        principalTable: "Regattas",
                        principalColumn: "RegattaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegattaOldclasses",
                columns: table => new
                {
                    OldclassId = table.Column<int>(type: "int", nullable: false),
                    RegattaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegattaOldclasses", x => new { x.OldclassId, x.RegattaId });
                    table.ForeignKey(
                        name: "FK_RegattaOldclasses_Oldclasses_OldclassId",
                        column: x => x.OldclassId,
                        principalTable: "Oldclasses",
                        principalColumn: "OldclassId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegattaOldclasses_Regattas_RegattaId",
                        column: x => x.RegattaId,
                        principalTable: "Regattas",
                        principalColumn: "RegattaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StartingFees",
                columns: table => new
                {
                    StartingFeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<float>(type: "real", nullable: false),
                    BoatclassId = table.Column<int>(type: "int", nullable: false),
                    OldclassId = table.Column<int>(type: "int", nullable: false),
                    RegattaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StartingFees", x => x.StartingFeeId);
                    table.ForeignKey(
                        name: "FK_StartingFees_Boatclasses_BoatclassId",
                        column: x => x.BoatclassId,
                        principalTable: "Boatclasses",
                        principalColumn: "BoatclassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StartingFees_Oldclasses_OldclassId",
                        column: x => x.OldclassId,
                        principalTable: "Oldclasses",
                        principalColumn: "OldclassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StartingFees_Regattas_RegattaId",
                        column: x => x.RegattaId,
                        principalTable: "Regattas",
                        principalColumn: "RegattaId",
                        onDelete: ReferentialAction.Restrict);
                });

             migrationBuilder.CreateTable(
                name: "Waters",
                columns: table => new
                {
                    WaterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Waters", x => x.WaterId);
                });
                
            migrationBuilder.CreateTable(
                name: "RegattaCampingFees",
                columns: table => new
                {
                    CampingFeeId = table.Column<int>(type: "int", nullable: false),
                    RegattaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegattaCampingFees", x => new { x.CampingFeeId, x.RegattaId });
                    table.ForeignKey(
                        name: "FK_RegattaCampingFees_CampingFees_CampingFeeId",
                        column: x => x.CampingFeeId,
                        principalTable: "CampingFees",
                        principalColumn: "CampingFeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegattaCampingFees_Regattas_RegattaId",
                        column: x => x.RegattaId,
                        principalTable: "Regattas",
                        principalColumn: "RegattaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Regattas_WaterId",
                table: "Regattas",
                column: "WaterId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_RaceclassId",
                table: "Competitions",
                column: "RaceclassId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_RegattaId",
                table: "Competitions",
                column: "RegattaId");

            migrationBuilder.CreateIndex(
                name: "IX_RegattaCampingFees_RegattaId",
                table: "RegattaCampingFees",
                column: "RegattaId");

            migrationBuilder.CreateIndex(
                name: "IX_RegattaOldclasses_RegattaId",
                table: "RegattaOldclasses",
                column: "RegattaId");

            migrationBuilder.CreateIndex(
                name: "IX_StartingFees_BoatclassId",
                table: "StartingFees",
                column: "BoatclassId");

            migrationBuilder.CreateIndex(
                name: "IX_StartingFees_OldclassId",
                table: "StartingFees",
                column: "OldclassId");

            migrationBuilder.CreateIndex(
                name: "IX_StartingFees_RegattaId",
                table: "StartingFees",
                column: "RegattaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Regattas_Waters_WaterId",
                table: "Regattas",
                column: "WaterId",
                principalTable: "Waters",
                principalColumn: "WaterId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Regattas_Waters_WaterId",
                table: "Regattas");

            migrationBuilder.DropTable(
                name: "Competitions");

            migrationBuilder.DropTable(
                name: "RegattaCampingFees");

            migrationBuilder.DropTable(
                name: "RegattaOldclasses");

            migrationBuilder.DropTable(
                name: "StartingFees");

            migrationBuilder.DropTable(
                name: "Waters");

            migrationBuilder.DropTable(
                name: "CampingFees");

            migrationBuilder.DropIndex(
                name: "IX_Regattas_WaterId",
                table: "Regattas");

            migrationBuilder.DropColumn(
                name: "Accomodation",
                table: "Regattas");

            migrationBuilder.DropColumn(
                name: "Awards",
                table: "Regattas");

            migrationBuilder.DropColumn(
                name: "Catering",
                table: "Regattas");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Regattas");

            migrationBuilder.DropColumn(
                name: "Judge",
                table: "Regattas");

            migrationBuilder.DropColumn(
                name: "ReportAddress",
                table: "Regattas");

            migrationBuilder.DropColumn(
                name: "ReportFax",
                table: "Regattas");

            migrationBuilder.DropColumn(
                name: "ReportMail",
                table: "Regattas");

            migrationBuilder.DropColumn(
                name: "ReportOpening",
                table: "Regattas");

            migrationBuilder.DropColumn(
                name: "ReportSchedule",
                table: "Regattas");

            migrationBuilder.DropColumn(
                name: "ReportTel",
                table: "Regattas");

            migrationBuilder.DropColumn(
                name: "ReportText",
                table: "Regattas");

            migrationBuilder.DropColumn(
                name: "ScheduleText",
                table: "Regattas");

            migrationBuilder.DropColumn(
                name: "Security",
                table: "Regattas");

            migrationBuilder.DropColumn(
                name: "Startslots",
                table: "Regattas");

            migrationBuilder.DropColumn(
                name: "SubscriberFee",
                table: "Regattas");

            migrationBuilder.DropColumn(
                name: "WaterId",
                table: "Regattas");

            migrationBuilder.DropColumn(
                name: "Waterdepth",
                table: "Regattas");

            migrationBuilder.CreateTable(
                name: "RegattaRaceclass",
                columns: table => new
                {
                    RegattaId = table.Column<int>(nullable: false),
                    RaceclassId = table.Column<int>(nullable: false),
                    RegattaRaceclassId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegattaRaceclass", x => new { x.RegattaId, x.RaceclassId });
                    table.UniqueConstraint("AK_RegattaRaceclass_RegattaRaceclassId", x => x.RegattaRaceclassId);
                    table.ForeignKey(
                        name: "FK_RegattaRaceclass_Raceclasses_RaceclassId",
                        column: x => x.RaceclassId,
                        principalTable: "Raceclasses",
                        principalColumn: "RaceclassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegattaRaceclass_Regattas_RegattaId",
                        column: x => x.RegattaId,
                        principalTable: "Regattas",
                        principalColumn: "RegattaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegattaRaceclass_RaceclassId",
                table: "RegattaRaceclass",
                column: "RaceclassId");
        }
    }
}
