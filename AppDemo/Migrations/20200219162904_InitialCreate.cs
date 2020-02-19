using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppDemo.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    id_usuario = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(65)", nullable: true),
                    telefono = table.Column<string>(type: "varchar(10)", nullable: true),
                    correo_electronico = table.Column<string>(type: "varchar(255)", nullable: true),
                    User = table.Column<string>(nullable: true),
                    password_salt = table.Column<string>(type: "text", nullable: true),
                    password_hash = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.id_usuario);
                });

            migrationBuilder.CreateTable(
                name: "usuarios_csv",
                columns: table => new
                {
                    id_usuario = table.Column<int>(nullable: false),
                    horas_trabajadas = table.Column<int>(nullable: true),
                    sueldo_por_hora = table.Column<decimal>(type: "money", nullable: true),
                    fecha_prox_pago = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios_csv", x => x.id_usuario);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "usuarios");

            migrationBuilder.DropTable(
                name: "usuarios_csv");
        }
    }
}
