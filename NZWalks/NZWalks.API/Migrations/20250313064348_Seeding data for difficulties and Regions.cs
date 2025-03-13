using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NZWalks.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedingdatafordifficultiesandRegions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("60e28c55-c78e-418d-a864-741e9fd834eb"), "Hard" },
                    { new Guid("6e12cb01-ec23-43f3-be03-95d0a2e681ba"), "Medium" },
                    { new Guid("c62caf1c-4b01-4ee9-b01f-22712605dfaa"), "Easy" }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Code", "Name", "RegionImageUrl" },
                values: new object[,]
                {
                    { new Guid("3012669d-1fdf-43f8-a390-6d506826a3d1"), "QT", "Queenstown", "https://unsplash.com/photos/aerial-view-of-city-near-lake-during-daytime-8T8UCBeWuUs" },
                    { new Guid("b2964597-9765-4ef2-89e4-7510d4583f3d"), "AKL", "Ackland", "https://dynamic-media-cdn.tripadvisor.com/media/photo-o/10/dc/e6/a1/enjoy-beautiful-views.jpg?w=600&h=-1&s=1" },
                    { new Guid("c2d724d7-83c1-4903-b68b-4d55c88cc2bb"), "WT", "Wellington", "https://unsplash.com/photos/aerial-view-of-city-buildings-during-sunset-KRtXOMfS4oA" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("60e28c55-c78e-418d-a864-741e9fd834eb"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("6e12cb01-ec23-43f3-be03-95d0a2e681ba"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("c62caf1c-4b01-4ee9-b01f-22712605dfaa"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("3012669d-1fdf-43f8-a390-6d506826a3d1"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("b2964597-9765-4ef2-89e4-7510d4583f3d"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("c2d724d7-83c1-4903-b68b-4d55c88cc2bb"));
        }
    }
}
