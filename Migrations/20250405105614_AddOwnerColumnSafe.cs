using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CombustionCarGuideWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerColumnSafe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.CreateTable(
            //     name: "Brands",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Brands", x => x.Id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Users",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Users", x => x.Id);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Cars",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         ModelName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         BrandId = table.Column<int>(type: "int", nullable: false),
            //         CarBrand = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         EngineType = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         MSRP = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //         CostToDrive = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
            //         OwnerRating = table.Column<decimal>(type: "decimal(2,1)", nullable: false),
            //         CargoCapacity = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
            //         ReleaseYear = table.Column<int>(type: "int", nullable: false),
            //         Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         TrimLevels = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         FuelEconomy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         Pros = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         Cons = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         TransmissionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         SeatingCapacity = table.Column<int>(type: "int", nullable: false),
            //         InfotainmentSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         SafetyFeatures = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         OwnerId = table.Column<int>(type: "int", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Cars", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Cars_Brands_BrandId",
            //             column: x => x.BrandId,
            //             principalTable: "Brands",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_Cars_Users_OwnerId",
            //             column: x => x.OwnerId,
            //             principalTable: "Users",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.SetNull);
            //     });

            migrationBuilder.CreateTable(
                name: "CarFavorites",
                columns: table => new
                {
                    FavoriteCarsId = table.Column<int>(type: "int", nullable: false),
                    FavoritedByUsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarFavorites", x => new { x.FavoriteCarsId, x.FavoritedByUsersId });
                    table.ForeignKey(
                        name: "FK_CarFavorites_Cars_FavoriteCarsId",
                        column: x => x.FavoriteCarsId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarFavorites_Users_FavoritedByUsersId",
                        column: x => x.FavoritedByUsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // migrationBuilder.CreateTable(
            //     name: "Comments",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         CarId = table.Column<int>(type: "int", nullable: false),
            //         UserId = table.Column<int>(type: "int", nullable: false),
            //         Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Comments", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Comments_Cars_CarId",
            //             column: x => x.CarId,
            //             principalTable: "Cars",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_Comments_Users_UserId",
            //             column: x => x.UserId,
            //             principalTable: "Users",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "Ratings",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         CarId = table.Column<int>(type: "int", nullable: false),
            //         UserId = table.Column<int>(type: "int", nullable: false),
            //         Star = table.Column<int>(type: "int", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Ratings", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Ratings_Cars_CarId",
            //             column: x => x.CarId,
            //             principalTable: "Cars",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_Ratings_Users_UserId",
            //             column: x => x.UserId,
            //             principalTable: "Users",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateIndex(
            //     name: "IX_CarFavorites_FavoritedByUsersId",
            //     table: "CarFavorites",
            //     column: "FavoritedByUsersId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Cars_BrandId",
            //     table: "Cars",
            //     column: "BrandId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Cars_OwnerId",
            //     table: "Cars",
            //     column: "OwnerId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Comments_CarId",
            //     table: "Comments",
            //     column: "CarId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Comments_UserId",
            //     table: "Comments",
            //     column: "UserId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Ratings_CarId",
            //     table: "Ratings",
            //     column: "CarId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Ratings_UserId",
            //     table: "Ratings",
            //     column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarFavorites");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
