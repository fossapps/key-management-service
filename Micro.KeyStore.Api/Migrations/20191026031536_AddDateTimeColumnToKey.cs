using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Micro.KeyStore.Api.Migrations
{
    public partial class AddDateTimeColumnToKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Keys",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            migrationBuilder.CreateIndex("created_at", "Keys", "CreatedAt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Keys");
        }
    }
}
