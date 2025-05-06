using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GreenFlux.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Groups",
                columns: new[] { "Id", "Capacity", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("1228d5dd-043a-467a-a2de-266edcfe1a96"), 500, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Group A", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("8dc987e1-324e-496b-b5b8-a0a750f4a06e"), 300, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Group B", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "ChargeStations",
                columns: new[] { "Id", "CreatedAt", "GroupId", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("919dbe2a-d310-42a1-a7e2-70dca4217be8"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("8dc987e1-324e-496b-b5b8-a0a750f4a06e"), "Charge Station 3", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("c388f119-315a-4115-8b86-0a2f20c79a4c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("1228d5dd-043a-467a-a2de-266edcfe1a96"), "Charge Station 2", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("eec4f729-8471-4586-8dae-e9b7053d1a05"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("1228d5dd-043a-467a-a2de-266edcfe1a96"), "Charge Station 1", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Connectors",
                columns: new[] { "ChargeStationId", "Id", "CreatedAt", "MaxCurrent", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("919dbe2a-d310-42a1-a7e2-70dca4217be8"), 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("c388f119-315a-4115-8b86-0a2f20c79a4c"), 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 120, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("eec4f729-8471-4586-8dae-e9b7053d1a05"), 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("919dbe2a-d310-42a1-a7e2-70dca4217be8"), 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 90, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("eec4f729-8471-4586-8dae-e9b7053d1a05"), 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 80, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Connectors",
                keyColumns: new[] { "ChargeStationId", "Id" },
                keyValues: new object[] { new Guid("919dbe2a-d310-42a1-a7e2-70dca4217be8"), 1 });

            migrationBuilder.DeleteData(
                table: "Connectors",
                keyColumns: new[] { "ChargeStationId", "Id" },
                keyValues: new object[] { new Guid("c388f119-315a-4115-8b86-0a2f20c79a4c"), 1 });

            migrationBuilder.DeleteData(
                table: "Connectors",
                keyColumns: new[] { "ChargeStationId", "Id" },
                keyValues: new object[] { new Guid("eec4f729-8471-4586-8dae-e9b7053d1a05"), 1 });

            migrationBuilder.DeleteData(
                table: "Connectors",
                keyColumns: new[] { "ChargeStationId", "Id" },
                keyValues: new object[] { new Guid("919dbe2a-d310-42a1-a7e2-70dca4217be8"), 2 });

            migrationBuilder.DeleteData(
                table: "Connectors",
                keyColumns: new[] { "ChargeStationId", "Id" },
                keyValues: new object[] { new Guid("eec4f729-8471-4586-8dae-e9b7053d1a05"), 2 });

            migrationBuilder.DeleteData(
                table: "ChargeStations",
                keyColumn: "Id",
                keyValue: new Guid("919dbe2a-d310-42a1-a7e2-70dca4217be8"));

            migrationBuilder.DeleteData(
                table: "ChargeStations",
                keyColumn: "Id",
                keyValue: new Guid("c388f119-315a-4115-8b86-0a2f20c79a4c"));

            migrationBuilder.DeleteData(
                table: "ChargeStations",
                keyColumn: "Id",
                keyValue: new Guid("eec4f729-8471-4586-8dae-e9b7053d1a05"));

            migrationBuilder.DeleteData(
                table: "Groups",
                keyColumn: "Id",
                keyValue: new Guid("1228d5dd-043a-467a-a2de-266edcfe1a96"));

            migrationBuilder.DeleteData(
                table: "Groups",
                keyColumn: "Id",
                keyValue: new Guid("8dc987e1-324e-496b-b5b8-a0a750f4a06e"));
        }
    }
}