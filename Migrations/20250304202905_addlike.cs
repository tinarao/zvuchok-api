using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class addlike : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Samples_Users_UserId",
                table: "Samples");

            migrationBuilder.DropIndex(
                name: "IX_Samples_UserId",
                table: "Samples");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Samples");

            migrationBuilder.RenameColumn(
                name: "Likes",
                table: "Samples",
                newName: "LikesCount");

            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    SampleId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "DATETIME('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Likes_Samples_SampleId",
                        column: x => x.SampleId,
                        principalTable: "Samples",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Likes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Likes_SampleId",
                table: "Likes",
                column: "SampleId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_SampleId_UserId",
                table: "Likes",
                columns: new[] { "SampleId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Likes_UserId",
                table: "Likes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.RenameColumn(
                name: "LikesCount",
                table: "Samples",
                newName: "Likes");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Samples",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Samples_UserId",
                table: "Samples",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Samples_Users_UserId",
                table: "Samples",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
