// SPDX-FileCopyrightText: 2025 DoctorJado
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Content.Server.Database.Migrations.Sqlite
{
    /// <inheritdoc />
    public partial class JobLoadouts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "last_job_loadout",
                table: "profile",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "job_loadouts",
                columns: table => new
                {
                    job_loadouts_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    job = table.Column<string>(type: "TEXT", nullable: false),
                    profile_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_loadouts", x => x.job_loadouts_id);
                    table.ForeignKey(
                        name: "FK_job_loadouts_profile_profile_id",
                        column: x => x.profile_id,
                        principalTable: "profile",
                        principalColumn: "profile_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "job_traits",
                columns: table => new
                {
                    job_traits_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    job = table.Column<string>(type: "TEXT", nullable: false),
                    profile_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_traits", x => x.job_traits_id);
                    table.ForeignKey(
                        name: "FK_job_traits_profile_profile_id",
                        column: x => x.profile_id,
                        principalTable: "profile",
                        principalColumn: "profile_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "job_loadout",
                columns: table => new
                {
                    job_loadout_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    job_loadouts_id = table.Column<int>(type: "INTEGER", nullable: false),
                    loadout_name = table.Column<string>(type: "TEXT", nullable: false),
                    custom_name = table.Column<string>(type: "TEXT", nullable: true),
                    custom_description = table.Column<string>(type: "TEXT", nullable: true),
                    custom_color_tint = table.Column<string>(type: "TEXT", nullable: true),
                    custom_heirloom = table.Column<bool>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_loadout", x => x.job_loadout_id);
                    table.ForeignKey(
                        name: "FK_job_loadout_job_loadouts_job_loadouts_id",
                        column: x => x.job_loadouts_id,
                        principalTable: "job_loadouts",
                        principalColumn: "job_loadouts_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "job_trait",
                columns: table => new
                {
                    job_trait_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    job_traits_id = table.Column<int>(type: "INTEGER", nullable: false),
                    trait_name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_trait", x => x.job_trait_id);
                    table.ForeignKey(
                        name: "FK_job_trait_job_traits_job_traits_id",
                        column: x => x.job_traits_id,
                        principalTable: "job_traits",
                        principalColumn: "job_traits_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_job_loadout_job_loadouts_id",
                table: "job_loadout",
                column: "job_loadouts_id");

            migrationBuilder.CreateIndex(
                name: "IX_job_loadouts_profile_id",
                table: "job_loadouts",
                column: "profile_id");

            migrationBuilder.CreateIndex(
                name: "IX_job_trait_job_traits_id",
                table: "job_trait",
                column: "job_traits_id");

            migrationBuilder.CreateIndex(
                name: "IX_job_traits_profile_id",
                table: "job_traits",
                column: "profile_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "job_loadout");

            migrationBuilder.DropTable(
                name: "job_trait");

            migrationBuilder.DropTable(
                name: "job_loadouts");

            migrationBuilder.DropTable(
                name: "job_traits");

            migrationBuilder.DropColumn(
                name: "last_job_loadout",
                table: "profile");
        }
    }
}
