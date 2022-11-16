using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DBinit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BettingDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QueryStringId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BettingDays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BettingTickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BetAmount = table.Column<double>(type: "float", nullable: false),
                    TicketPlacedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsWinningTicket = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BettingTickets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BettingPairs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstOpponent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecondOpponent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatchStartUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SportId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BettingPairs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BettingPairs_Sports_SportId",
                        column: x => x.SportId,
                        principalTable: "Sports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tips",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stake = table.Column<double>(type: "float", nullable: false),
                    BettingPairId = table.Column<int>(type: "int", nullable: true),
                    SportId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tips_BettingPairs_BettingPairId",
                        column: x => x.BettingPairId,
                        principalTable: "BettingPairs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tips_Sports_SportId",
                        column: x => x.SportId,
                        principalTable: "Sports",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TicketPairs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BettingPairId = table.Column<int>(type: "int", nullable: false),
                    TipId = table.Column<int>(type: "int", nullable: false),
                    BettingTicketId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketPairs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketPairs_BettingPairs_BettingPairId",
                        column: x => x.BettingPairId,
                        principalTable: "BettingPairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketPairs_BettingTickets_BettingTicketId",
                        column: x => x.BettingTicketId,
                        principalTable: "BettingTickets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TicketPairs_Tips_TipId",
                        column: x => x.TipId,
                        principalTable: "Tips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BettingPairs_SportId",
                table: "BettingPairs",
                column: "SportId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketPairs_BettingPairId",
                table: "TicketPairs",
                column: "BettingPairId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketPairs_BettingTicketId",
                table: "TicketPairs",
                column: "BettingTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketPairs_TipId",
                table: "TicketPairs",
                column: "TipId");

            migrationBuilder.CreateIndex(
                name: "IX_Tips_BettingPairId",
                table: "Tips",
                column: "BettingPairId");

            migrationBuilder.CreateIndex(
                name: "IX_Tips_SportId",
                table: "Tips",
                column: "SportId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BettingDays");

            migrationBuilder.DropTable(
                name: "TicketPairs");

            migrationBuilder.DropTable(
                name: "BettingTickets");

            migrationBuilder.DropTable(
                name: "Tips");

            migrationBuilder.DropTable(
                name: "BettingPairs");

            migrationBuilder.DropTable(
                name: "Sports");
        }
    }
}
