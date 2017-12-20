using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RegattaManager.Migrations
{
    public partial class RegattaChoosen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegattaChosen");

            migrationBuilder.AddColumn<bool>(
                name: "Choosen",
                table: "Regattas",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Choosen",
                table: "Regattas");

            migrationBuilder.CreateTable(
                name: "RegattaChosen",
                columns: table => new
                {
                    RegattaChosenId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RegattaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegattaChosen", x => x.RegattaChosenId);
                    table.ForeignKey(
                        name: "FK_RegattaChosen_Regattas_RegattaId",
                        column: x => x.RegattaId,
                        principalTable: "Regattas",
                        principalColumn: "RegattaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegattaChosen_RegattaId",
                table: "RegattaChosen",
                column: "RegattaId",
                unique: true);
        }
    }
}
