// SPDX-FileCopyrightText: 2025 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 2025 Scylla-Bot <botscylla@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Content.Server.Database.Migrations.Sqlite
{
    /// <inheritdoc />
    public partial class Consents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "profile_consent_preference",
                columns: table => new
                {
                    profile_consent_preference_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    profile_id = table.Column<int>(type: "INTEGER", nullable: false),
                    consent_prototype_id = table.Column<string>(type: "TEXT", nullable: false),
                    level = table.Column<sbyte>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_profile_consent_preference", x => x.profile_consent_preference_id);
                    table.ForeignKey(
                        name: "FK_profile_consent_preference_profile_profile_id",
                        column: x => x.profile_id,
                        principalTable: "profile",
                        principalColumn: "profile_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_profile_consent_preference_profile_id",
                table: "profile_consent_preference",
                column: "profile_id");

            migrationBuilder.CreateIndex(
                name: "IX_profile_consent_preference_profile_id_consent_prototype_id",
                table: "profile_consent_preference",
                columns: new[] { "profile_id", "consent_prototype_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "profile_consent_preference");
        }
    }
}
