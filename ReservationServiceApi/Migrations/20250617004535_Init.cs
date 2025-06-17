using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ReservationServiceApi.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Reservo");

            migrationBuilder.CreateTable(
                name: "ReservationCart",
                schema: "Reservo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerExternalId = table.Column<int>(type: "integer", nullable: false),
                    PromoCode = table.Column<string>(type: "text", nullable: true),
                    DiscountApplied = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationCart", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reservation",
                schema: "Reservo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerExternalId = table.Column<int>(type: "integer", nullable: false),
                    CustomerName = table.Column<string>(type: "text", nullable: false),
                    ReservationStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReservationEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ReservationCartId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservation_ReservationCart_ReservationCartId",
                        column: x => x.ReservationCartId,
                        principalSchema: "Reservo",
                        principalTable: "ReservationCart",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Room",
                schema: "Reservo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoomNumber = table.Column<int>(type: "integer", nullable: false),
                    RoomStatus = table.Column<int>(type: "integer", nullable: false),
                    PricePerNight = table.Column<decimal>(type: "numeric", nullable: false),
                    ReservationId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Room_Reservation_ReservationId",
                        column: x => x.ReservationId,
                        principalSchema: "Reservo",
                        principalTable: "Reservation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_ReservationCartId",
                schema: "Reservo",
                table: "Reservation",
                column: "ReservationCartId");

            migrationBuilder.CreateIndex(
                name: "IX_Room_ReservationId",
                schema: "Reservo",
                table: "Room",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_Room_RoomNumber",
                schema: "Reservo",
                table: "Room",
                column: "RoomNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Room",
                schema: "Reservo");

            migrationBuilder.DropTable(
                name: "Reservation",
                schema: "Reservo");

            migrationBuilder.DropTable(
                name: "ReservationCart",
                schema: "Reservo");
        }
    }
}
