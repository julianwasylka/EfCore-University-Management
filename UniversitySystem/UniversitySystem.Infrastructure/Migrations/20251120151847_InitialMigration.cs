using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversitySystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LicznikiIndeksow",
                columns: table => new
                {
                    Prefix = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    AktualnaWartosc = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicznikiIndeksow", x => x.Prefix);
                });

            migrationBuilder.CreateTable(
                name: "Profesorowie",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Imie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nazwisko = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TytulNaukowy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IndeksUczelniany = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AdresZamieszkania_Ulica = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdresZamieszkania_Miasto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdresZamieszkania_KodPocztowy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profesorowie", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wydzialy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wydzialy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gabinety",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumerPokoju = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Budynek = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfesorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gabinety", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gabinety_Profesorowie_ProfesorId",
                        column: x => x.ProfesorId,
                        principalTable: "Profesorowie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Studenci",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Imie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nazwisko = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IndeksUczelniany = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RokStudiow = table.Column<int>(type: "int", nullable: false),
                    AdresZamieszkania_Ulica = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdresZamieszkania_Miasto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdresZamieszkania_KodPocztowy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: false),
                    TematPracyDyplomowej = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PromotorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Studenci", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Studenci_Profesorowie_PromotorId",
                        column: x => x.PromotorId,
                        principalTable: "Profesorowie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Kursy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KodKursu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PunktyECTS = table.Column<int>(type: "int", nullable: false),
                    WydzialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kursy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kursy_Wydzialy_WydzialId",
                        column: x => x.WydzialId,
                        principalTable: "Wydzialy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WymaganiaKursow",
                columns: table => new
                {
                    WymaganePrzezId = table.Column<int>(type: "int", nullable: false),
                    WymaganiaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WymaganiaKursow", x => new { x.WymaganePrzezId, x.WymaganiaId });
                    table.ForeignKey(
                        name: "FK_WymaganiaKursow_Kursy_WymaganePrzezId",
                        column: x => x.WymaganePrzezId,
                        principalTable: "Kursy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WymaganiaKursow_Kursy_WymaganiaId",
                        column: x => x.WymaganiaId,
                        principalTable: "Kursy",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Zapisy",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    KursId = table.Column<int>(type: "int", nullable: false),
                    Ocena = table.Column<double>(type: "float", nullable: true),
                    Semestr = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zapisy", x => new { x.StudentId, x.KursId });
                    table.ForeignKey(
                        name: "FK_Zapisy_Kursy_KursId",
                        column: x => x.KursId,
                        principalTable: "Kursy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Zapisy_Studenci_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Studenci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gabinety_ProfesorId",
                table: "Gabinety",
                column: "ProfesorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kursy_WydzialId",
                table: "Kursy",
                column: "WydzialId");

            migrationBuilder.CreateIndex(
                name: "IX_Profesorowie_IndeksUczelniany",
                table: "Profesorowie",
                column: "IndeksUczelniany",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Studenci_IndeksUczelniany",
                table: "Studenci",
                column: "IndeksUczelniany",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Studenci_PromotorId",
                table: "Studenci",
                column: "PromotorId");

            migrationBuilder.CreateIndex(
                name: "IX_WymaganiaKursow_WymaganiaId",
                table: "WymaganiaKursow",
                column: "WymaganiaId");

            migrationBuilder.CreateIndex(
                name: "IX_Zapisy_KursId",
                table: "Zapisy",
                column: "KursId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gabinety");

            migrationBuilder.DropTable(
                name: "LicznikiIndeksow");

            migrationBuilder.DropTable(
                name: "WymaganiaKursow");

            migrationBuilder.DropTable(
                name: "Zapisy");

            migrationBuilder.DropTable(
                name: "Kursy");

            migrationBuilder.DropTable(
                name: "Studenci");

            migrationBuilder.DropTable(
                name: "Wydzialy");

            migrationBuilder.DropTable(
                name: "Profesorowie");
        }
    }
}
