using Microsoft.EntityFrameworkCore.Migrations;

namespace Micro.KeyStore.Api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Keys",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Body = table.Column<string>(nullable: true),
                    Sha = table.Column<string>(type: "VARCHAR", maxLength: 250, nullable: true),
                    ShortSha = table.Column<string>(type: "VARCHAR", maxLength: 50, nullable: true)
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
                name: "Keys");
        }
    }
}
