using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SqlAdapter.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Deleted = table.Column<Guid>(nullable: true),
                    PostId = table.Column<Guid>(nullable: true),
                    Age = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserWithPostNumberQuerys",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LatestTitleChange = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    PostNumber = table.Column<int>(nullable: false),
                    RootEntityId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWithPostNumberQuerys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Age = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PinnedPostId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Body = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PinnedPostId",
                table: "Users",
                column: "PinnedPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Posts_PinnedPostId",
                table: "Users",
                column: "PinnedPostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Users_UserId",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "EventHistory");

            migrationBuilder.DropTable(
                name: "UserWithPostNumberQuerys");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Posts");
        }
    }
}
