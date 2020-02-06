using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace _20MinuteBackend.Infrastructure.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Backends",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OrginalJson = table.Column<string>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Backends", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Backends");
        }
    }
}
