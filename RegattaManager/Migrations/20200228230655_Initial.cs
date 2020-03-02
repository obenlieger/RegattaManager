using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RegattaManager.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Boatclasses",
                columns: table => new
                {
                    BoatclassId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Seats = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boatclasses", x => x.BoatclassId);
                });

            migrationBuilder.CreateTable(
                name: "CampingFees",
                columns: table => new
                {
                    CampingFeeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Amount = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampingFees", x => x.CampingFeeId);
                });

            migrationBuilder.CreateTable(
                name: "Clubs",
                columns: table => new
                {
                    ClubId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    VNr = table.Column<string>(nullable: true),
                    ShortName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clubs", x => x.ClubId);
                });

            migrationBuilder.CreateTable(
                name: "Mannschaftswertung",
                columns: table => new
                {
                    MWId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClubId = table.Column<int>(nullable: false),
                    ClubName = table.Column<string>(nullable: true),
                    OldclassName = table.Column<string>(nullable: true),
                    Wertung = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mannschaftswertung", x => x.MWId);
                });

            migrationBuilder.CreateTable(
                name: "Oldclasses",
                columns: table => new
                {
                    OldclassId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    FromAge = table.Column<int>(nullable: false),
                    ToAge = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oldclasses", x => x.OldclassId);
                });

            migrationBuilder.CreateTable(
                name: "Raceclasses",
                columns: table => new
                {
                    RaceclassId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Length = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Raceclasses", x => x.RaceclassId);
                });

            migrationBuilder.CreateTable(
                name: "RaceDraws",
                columns: table => new
                {
                    RaceDrawId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    isAbteilungslauf = table.Column<bool>(nullable: false),
                    ReportedSBCountFrom = table.Column<int>(nullable: false),
                    ReportedSBCountTo = table.Column<int>(nullable: false),
                    VorlaufCount = table.Column<int>(nullable: false),
                    HoffnungslaufCount = table.Column<int>(nullable: false),
                    ZwischenlaufCount = table.Column<int>(nullable: false),
                    EndlaufCount = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaceDraws", x => x.RaceDrawId);
                });

            migrationBuilder.CreateTable(
                name: "Racestati",
                columns: table => new
                {
                    RacestatusId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Racestati", x => x.RacestatusId);
                });

            migrationBuilder.CreateTable(
                name: "RaceTyps",
                columns: table => new
                {
                    RaceTypId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    isFinal = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaceTyps", x => x.RaceTypId);
                });

            migrationBuilder.CreateTable(
                name: "Startboatstati",
                columns: table => new
                {
                    StartboatstatusId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Startboatstati", x => x.StartboatstatusId);
                });

            migrationBuilder.CreateTable(
                name: "Waters",
                columns: table => new
                {
                    WaterId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Waters", x => x.WaterId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StartingFees",
                columns: table => new
                {
                    StartingFeeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<float>(nullable: false),
                    BoatclassId = table.Column<int>(nullable: false),
                    FromOldclassId = table.Column<int>(nullable: false),
                    ToOldclassId = table.Column<int>(nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "ClubCampingFees",
                columns: table => new
                {
                    CampingFeeId = table.Column<int>(nullable: false),
                    ClubId = table.Column<int>(nullable: false),
                    ClubCampingFeeId = table.Column<int>(nullable: false),
                    campingcount = table.Column<int>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    MemberId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LastName = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    Birthyear = table.Column<int>(nullable: false),
                    Gender = table.Column<string>(nullable: true),
                    isRented = table.Column<bool>(nullable: false),
                    RentedToClubId = table.Column<int>(nullable: false),
                    RentYear = table.Column<int>(nullable: false),
                    ClubId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.MemberId);
                    table.ForeignKey(
                        name: "FK_Members_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "ClubId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Competitions",
                columns: table => new
                {
                    CompetitionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BoatclassId = table.Column<int>(nullable: false),
                    RaceclassId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competitions", x => x.CompetitionId);
                    table.ForeignKey(
                        name: "FK_Competitions_Boatclasses_BoatclassId",
                        column: x => x.BoatclassId,
                        principalTable: "Boatclasses",
                        principalColumn: "BoatclassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Competitions_Raceclasses_RaceclassId",
                        column: x => x.RaceclassId,
                        principalTable: "Raceclasses",
                        principalColumn: "RaceclassId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RaceDrawRules",
                columns: table => new
                {
                    RaceDrawRuleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RaceDrawId = table.Column<int>(nullable: false),
                    RaceTypId = table.Column<int>(nullable: false),
                    RaceSequence = table.Column<int>(nullable: false),
                    PlacementFrom = table.Column<int>(nullable: false),
                    PlacementTo = table.Column<int>(nullable: false),
                    ToRaceTypId = table.Column<int>(nullable: false),
                    ToRaceSequence = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaceDrawRules", x => x.RaceDrawRuleId);
                    table.ForeignKey(
                        name: "FK_RaceDrawRules_RaceDraws_RaceDrawId",
                        column: x => x.RaceDrawId,
                        principalTable: "RaceDraws",
                        principalColumn: "RaceDrawId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RaceDrawRules_RaceTyps_RaceTypId",
                        column: x => x.RaceTypId,
                        principalTable: "RaceTyps",
                        principalColumn: "RaceTypId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Regattas",
                columns: table => new
                {
                    RegattaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Choosen = table.Column<bool>(nullable: false),
                    FromDate = table.Column<DateTime>(nullable: false),
                    ToDate = table.Column<DateTime>(nullable: false),
                    Waterdepth = table.Column<int>(nullable: false),
                    Startslots = table.Column<int>(nullable: false),
                    ReportText = table.Column<string>(nullable: true),
                    ReportOpening = table.Column<DateTime>(nullable: false),
                    ReportSchedule = table.Column<DateTime>(nullable: false),
                    ReportAddress = table.Column<string>(nullable: true),
                    ReportTel = table.Column<string>(nullable: true),
                    ReportFax = table.Column<string>(nullable: true),
                    ReportMail = table.Column<string>(nullable: true),
                    Judge = table.Column<string>(nullable: true),
                    Awards = table.Column<string>(nullable: true),
                    Security = table.Column<string>(nullable: true),
                    ScheduleText = table.Column<string>(nullable: true),
                    SubscriberFee = table.Column<float>(nullable: false),
                    Accomodation = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    Catering = table.Column<string>(nullable: true),
                    ClubId = table.Column<int>(nullable: false),
                    Organizer = table.Column<string>(nullable: true),
                    StartersLastYear = table.Column<int>(nullable: false),
                    Category = table.Column<string>(nullable: true),
                    WaterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regattas", x => x.RegattaId);
                    table.ForeignKey(
                        name: "FK_Regattas_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "ClubId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Regattas_Waters_WaterId",
                        column: x => x.WaterId,
                        principalTable: "Waters",
                        principalColumn: "WaterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Races",
                columns: table => new
                {
                    RaceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Starttime = table.Column<DateTime>(nullable: false),
                    Realstarttime = table.Column<DateTime>(nullable: false),
                    Gender = table.Column<string>(nullable: true),
                    Spoken = table.Column<bool>(nullable: false),
                    ResultSpoken = table.Column<bool>(nullable: false),
                    Sequence = table.Column<int>(nullable: false),
                    RaceCode = table.Column<string>(nullable: true),
                    FinishType = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    RegattaId = table.Column<int>(nullable: false),
                    RaceclassId = table.Column<int>(nullable: false),
                    OldclassId = table.Column<int>(nullable: false),
                    BoatclassId = table.Column<int>(nullable: false),
                    RacestatusId = table.Column<int>(nullable: false),
                    isAbteilungslauf = table.Column<bool>(nullable: false),
                    RaceTypId = table.Column<int>(nullable: false),
                    ReportedRaceId = table.Column<int>(nullable: false),
                    RaceDrawId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Races", x => x.RaceId);
                    table.ForeignKey(
                        name: "FK_Races_Boatclasses_BoatclassId",
                        column: x => x.BoatclassId,
                        principalTable: "Boatclasses",
                        principalColumn: "BoatclassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Races_Oldclasses_OldclassId",
                        column: x => x.OldclassId,
                        principalTable: "Oldclasses",
                        principalColumn: "OldclassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Races_RaceTyps_RaceTypId",
                        column: x => x.RaceTypId,
                        principalTable: "RaceTyps",
                        principalColumn: "RaceTypId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Races_Raceclasses_RaceclassId",
                        column: x => x.RaceclassId,
                        principalTable: "Raceclasses",
                        principalColumn: "RaceclassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Races_Racestati_RacestatusId",
                        column: x => x.RacestatusId,
                        principalTable: "Racestati",
                        principalColumn: "RacestatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Races_Regattas_RegattaId",
                        column: x => x.RegattaId,
                        principalTable: "Regattas",
                        principalColumn: "RegattaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegattaCampingFees",
                columns: table => new
                {
                    CampingFeeId = table.Column<int>(nullable: false),
                    RegattaId = table.Column<int>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "RegattaClubs",
                columns: table => new
                {
                    ClubId = table.Column<int>(nullable: false),
                    RegattaId = table.Column<int>(nullable: false),
                    Guid = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegattaClubs", x => new { x.ClubId, x.RegattaId });
                    table.ForeignKey(
                        name: "FK_RegattaClubs_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "ClubId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegattaClubs_Regattas_RegattaId",
                        column: x => x.RegattaId,
                        principalTable: "Regattas",
                        principalColumn: "RegattaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegattaCompetitions",
                columns: table => new
                {
                    CompetitionId = table.Column<int>(nullable: false),
                    RegattaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegattaCompetitions", x => new { x.CompetitionId, x.RegattaId });
                    table.ForeignKey(
                        name: "FK_RegattaCompetitions_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "CompetitionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegattaCompetitions_Regattas_RegattaId",
                        column: x => x.RegattaId,
                        principalTable: "Regattas",
                        principalColumn: "RegattaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegattaOldclasses",
                columns: table => new
                {
                    OldclassId = table.Column<int>(nullable: false),
                    RegattaId = table.Column<int>(nullable: false)
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
                name: "RegattaStartingFees",
                columns: table => new
                {
                    StartingFeeId = table.Column<int>(nullable: false),
                    RegattaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegattaStartingFees", x => new { x.StartingFeeId, x.RegattaId });
                    table.ForeignKey(
                        name: "FK_RegattaStartingFees_Regattas_RegattaId",
                        column: x => x.RegattaId,
                        principalTable: "Regattas",
                        principalColumn: "RegattaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegattaStartingFees_StartingFees_StartingFeeId",
                        column: x => x.StartingFeeId,
                        principalTable: "StartingFees",
                        principalColumn: "StartingFeeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReportedRaces",
                columns: table => new
                {
                    ReportedRaceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    modifiedDate = table.Column<DateTime>(nullable: false),
                    isCreated = table.Column<bool>(nullable: false),
                    StartboatCount = table.Column<int>(nullable: false),
                    isAbteilungslauf = table.Column<bool>(nullable: false),
                    OldclassId = table.Column<int>(nullable: false),
                    CompetitionId = table.Column<int>(nullable: false),
                    Gender = table.Column<string>(nullable: true),
                    RaceCode = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    RegattaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportedRaces", x => x.ReportedRaceId);
                    table.ForeignKey(
                        name: "FK_ReportedRaces_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "CompetitionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportedRaces_Oldclasses_OldclassId",
                        column: x => x.OldclassId,
                        principalTable: "Oldclasses",
                        principalColumn: "OldclassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportedRaces_Regattas_RegattaId",
                        column: x => x.RegattaId,
                        principalTable: "Regattas",
                        principalColumn: "RegattaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportedStartboats",
                columns: table => new
                {
                    ReportedStartboatId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Gender = table.Column<string>(nullable: true),
                    modifiedDate = table.Column<DateTime>(nullable: false),
                    isLate = table.Column<bool>(nullable: false),
                    NoStartslot = table.Column<bool>(nullable: false),
                    ClubId = table.Column<int>(nullable: false),
                    RegattaId = table.Column<int>(nullable: false),
                    ReportedRaceId = table.Column<int>(nullable: false),
                    CompetitionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportedStartboats", x => x.ReportedStartboatId);
                    table.ForeignKey(
                        name: "FK_ReportedStartboats_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "ClubId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportedStartboats_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "CompetitionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportedStartboats_Regattas_RegattaId",
                        column: x => x.RegattaId,
                        principalTable: "Regattas",
                        principalColumn: "RegattaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportedStartboats_ReportedRaces_ReportedRaceId",
                        column: x => x.ReportedRaceId,
                        principalTable: "ReportedRaces",
                        principalColumn: "ReportedRaceId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ReportedStartboatMembers",
                columns: table => new
                {
                    ReportedStartboatId = table.Column<int>(nullable: false),
                    MemberId = table.Column<int>(nullable: false),
                    Seatnumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportedStartboatMembers", x => new { x.ReportedStartboatId, x.MemberId });
                    table.ForeignKey(
                        name: "FK_ReportedStartboatMembers_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportedStartboatMembers_ReportedStartboats_ReportedStartboatId",
                        column: x => x.ReportedStartboatId,
                        principalTable: "ReportedStartboats",
                        principalColumn: "ReportedStartboatId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReportedStartboatStandbys",
                columns: table => new
                {
                    ReportedStartboatId = table.Column<int>(nullable: false),
                    MemberId = table.Column<int>(nullable: false),
                    Standbynumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportedStartboatStandbys", x => new { x.ReportedStartboatId, x.MemberId });
                    table.ForeignKey(
                        name: "FK_ReportedStartboatStandbys_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportedStartboatStandbys_ReportedStartboats_ReportedStartboatId",
                        column: x => x.ReportedStartboatId,
                        principalTable: "ReportedStartboats",
                        principalColumn: "ReportedStartboatId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Startboats",
                columns: table => new
                {
                    StartboatId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Startslot = table.Column<int>(nullable: false),
                    Gender = table.Column<string>(nullable: true),
                    Placement = table.Column<int>(nullable: false),
                    FinishTime = table.Column<DateTime>(nullable: false),
                    ClubId = table.Column<int>(nullable: false),
                    RaceId = table.Column<int>(nullable: false),
                    StartboatstatusId = table.Column<int>(nullable: false),
                    ReportedStartboatId = table.Column<int>(nullable: false),
                    RegattaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Startboats", x => x.StartboatId);
                    table.ForeignKey(
                        name: "FK_Startboats_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "ClubId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Startboats_Races_RaceId",
                        column: x => x.RaceId,
                        principalTable: "Races",
                        principalColumn: "RaceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Startboats_Regattas_RegattaId",
                        column: x => x.RegattaId,
                        principalTable: "Regattas",
                        principalColumn: "RegattaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Startboats_ReportedStartboats_ReportedStartboatId",
                        column: x => x.ReportedStartboatId,
                        principalTable: "ReportedStartboats",
                        principalColumn: "ReportedStartboatId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Startboats_Startboatstati_StartboatstatusId",
                        column: x => x.StartboatstatusId,
                        principalTable: "Startboatstati",
                        principalColumn: "StartboatstatusId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "StartboatMembers",
                columns: table => new
                {
                    StartboatId = table.Column<int>(nullable: false),
                    MemberId = table.Column<int>(nullable: false),
                    SeatNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StartboatMembers", x => new { x.StartboatId, x.MemberId });
                    table.ForeignKey(
                        name: "FK_StartboatMembers_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StartboatMembers_Startboats_StartboatId",
                        column: x => x.StartboatId,
                        principalTable: "Startboats",
                        principalColumn: "StartboatId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StartboatStandbys",
                columns: table => new
                {
                    StartboatId = table.Column<int>(nullable: false),
                    MemberId = table.Column<int>(nullable: false),
                    Standbynumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StartboatStandbys", x => new { x.StartboatId, x.MemberId });
                    table.ForeignKey(
                        name: "FK_StartboatStandbys_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StartboatStandbys_Startboats_StartboatId",
                        column: x => x.StartboatId,
                        principalTable: "Startboats",
                        principalColumn: "StartboatId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ClubCampingFees_ClubId",
                table: "ClubCampingFees",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_BoatclassId",
                table: "Competitions",
                column: "BoatclassId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_RaceclassId",
                table: "Competitions",
                column: "RaceclassId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_ClubId",
                table: "Members",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_RaceDrawRules_RaceDrawId",
                table: "RaceDrawRules",
                column: "RaceDrawId");

            migrationBuilder.CreateIndex(
                name: "IX_RaceDrawRules_RaceTypId",
                table: "RaceDrawRules",
                column: "RaceTypId");

            migrationBuilder.CreateIndex(
                name: "IX_Races_BoatclassId",
                table: "Races",
                column: "BoatclassId");

            migrationBuilder.CreateIndex(
                name: "IX_Races_OldclassId",
                table: "Races",
                column: "OldclassId");

            migrationBuilder.CreateIndex(
                name: "IX_Races_RaceTypId",
                table: "Races",
                column: "RaceTypId");

            migrationBuilder.CreateIndex(
                name: "IX_Races_RaceclassId",
                table: "Races",
                column: "RaceclassId");

            migrationBuilder.CreateIndex(
                name: "IX_Races_RacestatusId",
                table: "Races",
                column: "RacestatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Races_RegattaId",
                table: "Races",
                column: "RegattaId");

            migrationBuilder.CreateIndex(
                name: "IX_RegattaCampingFees_RegattaId",
                table: "RegattaCampingFees",
                column: "RegattaId");

            migrationBuilder.CreateIndex(
                name: "IX_RegattaClubs_RegattaId",
                table: "RegattaClubs",
                column: "RegattaId");

            migrationBuilder.CreateIndex(
                name: "IX_RegattaCompetitions_RegattaId",
                table: "RegattaCompetitions",
                column: "RegattaId");

            migrationBuilder.CreateIndex(
                name: "IX_RegattaOldclasses_RegattaId",
                table: "RegattaOldclasses",
                column: "RegattaId");

            migrationBuilder.CreateIndex(
                name: "IX_Regattas_ClubId",
                table: "Regattas",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_Regattas_WaterId",
                table: "Regattas",
                column: "WaterId");

            migrationBuilder.CreateIndex(
                name: "IX_RegattaStartingFees_RegattaId",
                table: "RegattaStartingFees",
                column: "RegattaId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportedRaces_CompetitionId",
                table: "ReportedRaces",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportedRaces_OldclassId",
                table: "ReportedRaces",
                column: "OldclassId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportedRaces_RegattaId",
                table: "ReportedRaces",
                column: "RegattaId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportedStartboatMembers_MemberId",
                table: "ReportedStartboatMembers",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportedStartboats_ClubId",
                table: "ReportedStartboats",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportedStartboats_CompetitionId",
                table: "ReportedStartboats",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportedStartboats_RegattaId",
                table: "ReportedStartboats",
                column: "RegattaId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportedStartboats_ReportedRaceId",
                table: "ReportedStartboats",
                column: "ReportedRaceId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportedStartboatStandbys_MemberId",
                table: "ReportedStartboatStandbys",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_StartboatMembers_MemberId",
                table: "StartboatMembers",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Startboats_ClubId",
                table: "Startboats",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_Startboats_RaceId",
                table: "Startboats",
                column: "RaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Startboats_RegattaId",
                table: "Startboats",
                column: "RegattaId");

            migrationBuilder.CreateIndex(
                name: "IX_Startboats_ReportedStartboatId",
                table: "Startboats",
                column: "ReportedStartboatId");

            migrationBuilder.CreateIndex(
                name: "IX_Startboats_StartboatstatusId",
                table: "Startboats",
                column: "StartboatstatusId");

            migrationBuilder.CreateIndex(
                name: "IX_StartboatStandbys_MemberId",
                table: "StartboatStandbys",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_StartingFees_BoatclassId",
                table: "StartingFees",
                column: "BoatclassId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ClubCampingFees");

            migrationBuilder.DropTable(
                name: "Mannschaftswertung");

            migrationBuilder.DropTable(
                name: "RaceDrawRules");

            migrationBuilder.DropTable(
                name: "RegattaCampingFees");

            migrationBuilder.DropTable(
                name: "RegattaClubs");

            migrationBuilder.DropTable(
                name: "RegattaCompetitions");

            migrationBuilder.DropTable(
                name: "RegattaOldclasses");

            migrationBuilder.DropTable(
                name: "RegattaStartingFees");

            migrationBuilder.DropTable(
                name: "ReportedStartboatMembers");

            migrationBuilder.DropTable(
                name: "ReportedStartboatStandbys");

            migrationBuilder.DropTable(
                name: "StartboatMembers");

            migrationBuilder.DropTable(
                name: "StartboatStandbys");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "RaceDraws");

            migrationBuilder.DropTable(
                name: "CampingFees");

            migrationBuilder.DropTable(
                name: "StartingFees");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Startboats");

            migrationBuilder.DropTable(
                name: "Races");

            migrationBuilder.DropTable(
                name: "ReportedStartboats");

            migrationBuilder.DropTable(
                name: "Startboatstati");

            migrationBuilder.DropTable(
                name: "RaceTyps");

            migrationBuilder.DropTable(
                name: "Racestati");

            migrationBuilder.DropTable(
                name: "ReportedRaces");

            migrationBuilder.DropTable(
                name: "Competitions");

            migrationBuilder.DropTable(
                name: "Oldclasses");

            migrationBuilder.DropTable(
                name: "Regattas");

            migrationBuilder.DropTable(
                name: "Boatclasses");

            migrationBuilder.DropTable(
                name: "Raceclasses");

            migrationBuilder.DropTable(
                name: "Clubs");

            migrationBuilder.DropTable(
                name: "Waters");
        }
    }
}
