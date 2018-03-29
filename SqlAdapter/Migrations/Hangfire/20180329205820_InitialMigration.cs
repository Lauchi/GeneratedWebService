using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SqlAdapter.Migrations.Hangfire
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventQueue",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<long>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: false),
                    Deleted = table.Column<Guid>(nullable: true),
                    PostId = table.Column<Guid>(nullable: true),
                    Age = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventQueue", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RowVersions",
                columns: table => new
                {
                    EventType = table.Column<string>(nullable: false),
                    LastRowVersion = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RowVersions", x => x.EventType);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventQueue");

            migrationBuilder.DropTable(
                name: "RowVersions");
        }
    }
}
