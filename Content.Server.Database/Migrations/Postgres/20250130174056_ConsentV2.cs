using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Content.Server.Database.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class ConsentV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "consent_permissions_entry",
                columns: table => new
                {
                    consent_permissions_entry_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_consent_permissions_entry", x => x.consent_permissions_entry_id);
                });

            migrationBuilder.CreateTable(
                name: "consent_target",
                columns: table => new
                {
                    consent_target_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    target_id = table.Column<Guid>(type: "uuid", nullable: false),
                    target_consent = table.Column<string>(type: "text", nullable: false),
                    target_has_consent = table.Column<bool>(type: "boolean", nullable: false),
                    consent_permissions_entry_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_consent_target", x => x.consent_target_id);
                    table.ForeignKey(
                        name: "FK_consent_target_consent_permissions_entry_consent_permission~",
                        column: x => x.consent_permissions_entry_id,
                        principalTable: "consent_permissions_entry",
                        principalColumn: "consent_permissions_entry_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_consent_permissions_entry_consent_permissions_entry_id_user~",
                table: "consent_permissions_entry",
                columns: new[] { "consent_permissions_entry_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_consent_target_consent_permissions_entry_id",
                table: "consent_target",
                column: "consent_permissions_entry_id");

            migrationBuilder.CreateIndex(
                name: "IX_consent_target_consent_target_id_target_id",
                table: "consent_target",
                columns: new[] { "consent_target_id", "target_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "consent_target");

            migrationBuilder.DropTable(
                name: "consent_permissions_entry");
        }
    }
}
