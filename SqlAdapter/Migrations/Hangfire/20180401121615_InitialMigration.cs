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
                name: "DomainEventBase",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: false),
                    Deleted = table.Column<Guid>(nullable: true),
                    PostId = table.Column<Guid>(nullable: true),
                    Age = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainEventBase", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventAndJobQueue",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DomainEventId = table.Column<Guid>(nullable: true),
                    JobName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventAndJobQueue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventAndJobQueue_DomainEventBase_DomainEventId",
                        column: x => x.DomainEventId,
                        principalTable: "DomainEventBase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventAndJobQueue_DomainEventId",
                table: "EventAndJobQueue",
                column: "DomainEventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventAndJobQueue");

            migrationBuilder.DropTable(
                name: "DomainEventBase");
        }
    }
}
