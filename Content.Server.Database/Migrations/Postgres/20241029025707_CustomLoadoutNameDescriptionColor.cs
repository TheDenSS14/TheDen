// SPDX-FileCopyrightText: 2024 DEATHB4DEFEAT <77995199+DEATHB4DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 sleepyyapril <123355664+sleepyyapril@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later AND MIT

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Content.Server.Database.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class CustomLoadoutNameDescriptionColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "custom_color_tint",
                table: "loadout",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "custom_description",
                table: "loadout",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "custom_name",
                table: "loadout",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "custom_color_tint",
                table: "loadout");

            migrationBuilder.DropColumn(
                name: "custom_description",
                table: "loadout");

            migrationBuilder.DropColumn(
                name: "custom_name",
                table: "loadout");
        }
    }
}
