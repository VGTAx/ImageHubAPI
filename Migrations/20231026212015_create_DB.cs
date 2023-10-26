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
                    UserId = table.Column<string>(type: "TEXT", nullable: true),
                    FriendId = table.Column<string>(type: "TEXT", nullable: true),
                    UserId1 = table.Column<string>(type: "TEXT", nullable: true)
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
                        name: "FK_Friendships_User_UserId1",
                        column: x => x.UserId1,
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
                    { "23ad2a4f-c1f0-4abc-94c0-52854af2039e", 0, "20b2d932-129d-4f31-b0bd-a0e88244145a", "username_2@example.com", false, true, null, "Username_2", "USERNAME_2@EXAMPLE.COM", "USERNAME_2@EXAMPLE.COM", "AQAAAAIAAYagAAAAECGNeswdJvAFFLya98Hp2gMJUtjaLRH2qaoxFZ80f3Tr8HTvjIjH0x4djwMNZUHhjg==", null, false, "aab28f0e-b024-4ec9-857b-33f9ee5246bf", false, "username_2@example.com" },
                    { "55d8220f-2967-4342-8f6c-e6294a3e52c2", 0, "c7104aba-53a1-4ceb-a2b6-8114a286f834", "username_1@example.com", false, true, null, "Username_1", "USERNAME_1@EXAMPLE.COM", "USERNAME_1@EXAMPLE.COM", "AQAAAAIAAYagAAAAEOqmVlPmLR/sG37g5AZB9at/nO0uSfzikTQUDv3VlJk7w3lffHKsI2wO589vFPUNkw==", null, false, "b32da478-203d-433c-8023-13c900ff91ca", false, "username_1@example.com" }
                });

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "ImageId", "Path", "Title", "UserId" },
                values: new object[,]
                {
                    { "22074d3a-c05a-4016-8e55-f5bc4d695e98", "/55d8220f-2967-4342-8f6c-e6294a3e52c2/PngItem_6631012.png", "github.png", "55d8220f-2967-4342-8f6c-e6294a3e52c2" },
                    { "33742155-859d-4adf-b52c-524b439ca685", "/23ad2a4f-c1f0-4abc-94c0-52854af2039e/logo.jpg", "github.png", "23ad2a4f-c1f0-4abc-94c0-52854af2039e" },
                    { "72282ddc-eeb1-4f8d-97ff-578c2e6eb698", "/23ad2a4f-c1f0-4abc-94c0-52854af2039e/github.png", "github.png", "23ad2a4f-c1f0-4abc-94c0-52854af2039e" },
                    { "85a2c8b8-abe4-4bbf-a683-47d7e557bffd", "/55d8220f-2967-4342-8f6c-e6294a3e52c2/man-search-for-hiring-job-online-from-laptop_1150-52728.jpg", "github.png", "55d8220f-2967-4342-8f6c-e6294a3e52c2" }
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
                name: "IX_Friendships_UserId1",
                table: "Friendships",
                column: "UserId1");

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
