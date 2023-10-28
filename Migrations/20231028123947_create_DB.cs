using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ImageHubAPI.Migrations
{
    /// <inheritdoc />
    public partial class create_DB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "Friendships",
                columns: table => new
                {
                    FriendshipId = table.Column<string>(type: "TEXT", nullable: false),
                    UserSenderId = table.Column<string>(type: "TEXT", nullable: true),
                    FriendId = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendships", x => x.FriendshipId);
                    table.ForeignKey(
                        name: "FK_Friendships_User_FriendId",
                        column: x => x.FriendId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Friendships_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Friendships_User_UserSenderId",
                        column: x => x.UserSenderId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    ImageId = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Path = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.ImageId);
                    table.ForeignKey(
                        name: "FK_Images_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "23ad2a4f-c1f0-4abc-94c0-52854af2039e", 0, "9f74027d-eb4e-4a97-afd8-b94ff243733c", "username_2@example.com", false, true, null, "Username_2", "USERNAME_2@EXAMPLE.COM", "USERNAME_2@EXAMPLE.COM", "AQAAAAIAAYagAAAAENvwpY06Tnq1EplpjJgC1TYFuCSOJRV9Cq5905IrbPqIjxij+ClCoaF5MnCP8sHBhg==", null, false, "1eb7c101-8f54-41e1-b65a-df7b5b3ded31", false, "username_2@example.com" },
                    { "55d8220f-2967-4342-8f6c-e6294a3e52c2", 0, "c7281e59-96c7-4013-835d-c66cf7251e09", "username_1@example.com", false, true, null, "Username_1", "USERNAME_1@EXAMPLE.COM", "USERNAME_1@EXAMPLE.COM", "AQAAAAIAAYagAAAAEPQHRHIiWIlB6+j/YgzT3pY28k8TDDngVyqwVK8ra0ZojD51zwOMwpwA34yZk7xfng==", null, false, "4e902d61-f69e-48ba-b4c4-0f99c0cfd308", false, "username_1@example.com" }
                });

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "ImageId", "Path", "Title", "UserId" },
                values: new object[,]
                {
                    { "04d7da38-62e4-4f92-a016-3a8cac8ecea8", "/55d8220f-2967-4342-8f6c-e6294a3e52c2/PngItem_6631012.png", "PngItem_6631012.png", "55d8220f-2967-4342-8f6c-e6294a3e52c2" },
                    { "38560b03-d8c8-4134-865e-87d4e1ea8eb1", "/23ad2a4f-c1f0-4abc-94c0-52854af2039e/github.png", "github.png", "23ad2a4f-c1f0-4abc-94c0-52854af2039e" },
                    { "4449e689-70c1-4917-9457-78b0b3e4aa38", "/23ad2a4f-c1f0-4abc-94c0-52854af2039e/logo.jpg", "logo.jpg", "23ad2a4f-c1f0-4abc-94c0-52854af2039e" },
                    { "c753ccde-eed7-4fa7-8338-55708b36a191", "/55d8220f-2967-4342-8f6c-e6294a3e52c2/man-search-for-hiring-job-online-from-laptop_1150-52728.jpg", "man-search-for-hiring-job-online-from-laptop_1150-52728.jpg", "55d8220f-2967-4342-8f6c-e6294a3e52c2" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_FriendId",
                table: "Friendships",
                column: "FriendId");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_UserId",
                table: "Friendships",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_UserSenderId",
                table: "Friendships",
                column: "UserSenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_Path",
                table: "Images",
                column: "Path",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_UserId",
                table: "Images",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friendships");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
