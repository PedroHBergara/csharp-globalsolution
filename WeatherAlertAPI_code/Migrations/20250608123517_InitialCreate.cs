using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherAlertAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cidade",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    latitude = table.Column<decimal>(type: "TEXT", precision: 10, scale: 6, nullable: true),
                    longitude = table.Column<decimal>(type: "TEXT", precision: 10, scale: 6, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cidade", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dica",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nivel_risco = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    mensagem = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dica", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cidade");

            migrationBuilder.DropTable(
                name: "dica");
        }
    }
}
