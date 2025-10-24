// SPDX-FileCopyrightText: 2025 Dirius77
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Content.Server.Database.Migrations.Sqlite
{
    /// <inheritdoc />
    public partial class AlternateJobTitles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "alternate_job_titles",
                columns: table => new
                {
                    alternate_job_titles_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    profile_id = table.Column<int>(type: "INTEGER", nullable: false),
                    job_id = table.Column<string>(type: "TEXT", nullable: false),
                    alternate_job_title = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alternate_job_titles", x => x.alternate_job_titles_id);
                    table.ForeignKey(
                        name: "FK_alternate_job_titles_profile_profile_id",
                        column: x => x.profile_id,
                        principalTable: "profile",
                        principalColumn: "profile_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_alternate_job_titles_profile_id",
                table: "alternate_job_titles",
                column: "profile_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alternate_job_titles");
        }
    }
}
