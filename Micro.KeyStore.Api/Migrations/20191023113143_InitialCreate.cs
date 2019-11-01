using Microsoft.EntityFrameworkCore.Migrations;

namespace Micro.KeyStore.Api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Keys",
                table => new
                {
                    Id = table.Column<string>(),
                    Body = table.Column<string>(nullable: true),
                    Sha = table.Column<string>("VARCHAR", maxLength: 250, nullable: true),
                    ShortSha = table.Column<string>("VARCHAR", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keys", x => x.Id);
                    table.UniqueConstraint("UNIQUE_SHORT_SHA", x => x.ShortSha);
                    table.UniqueConstraint("UNIQUE_SHA", x => x.Sha);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Keys");
        }
    }
}
