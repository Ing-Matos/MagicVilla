using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class AlimentarTablaVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImagenUrl", "MetrosCudrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[,]
                {
                    { 1, "", "Detalle de la Vila...", new DateTime(2023, 8, 4, 19, 7, 6, 227, DateTimeKind.Local).AddTicks(6763), new DateTime(2023, 8, 4, 19, 7, 6, 227, DateTimeKind.Local).AddTicks(6746), "", 50, "Villa Real", 5, 200.0 },
                    { 2, "", "Detalle de la Vila...", new DateTime(2023, 8, 4, 19, 7, 6, 227, DateTimeKind.Local).AddTicks(6767), new DateTime(2023, 8, 4, 19, 7, 6, 227, DateTimeKind.Local).AddTicks(6766), "", 100, "Primium Villa Real", 10, 400.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
