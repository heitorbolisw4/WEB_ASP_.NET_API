using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projeto_API_WEB_ASP_.NET.Migrations
{
    /// <inheritdoc />
    public partial class ApplyHasData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Pasworld",
                table: "Administradores",
                newName: "Password");

            migrationBuilder.InsertData(
                table: "Administradores",
                columns: new[] { "Id", "Email", "Password", "Perfil" },
                values: new object[] { 1, "adm@exemplo.com", "12345", "Adm" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Administradores",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Administradores",
                newName: "Pasworld");
        }
    }
}
