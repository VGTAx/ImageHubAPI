﻿// <auto-generated />
using System;
using ImageHubAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ImageHubAPI.Migrations
{
    [DbContext(typeof(ImageHubContext))]
    partial class ImageHubContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.13");

            modelBuilder.Entity("ImageHubAPI.Models.Friendship", b =>
                {
                    b.Property<string>("FriendshipId")
                        .HasColumnType("TEXT")
                        .HasColumnOrder(1);

                    b.Property<string>("FriendId")
                        .HasColumnType("TEXT")
                        .HasColumnOrder(3);

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT")
                        .HasColumnOrder(2);

                    b.Property<string>("UserId1")
                        .HasColumnType("TEXT");

                    b.HasKey("FriendshipId");

                    b.HasIndex("FriendId");

                    b.HasIndex("UserId");

                    b.HasIndex("UserId1");

                    b.ToTable("Friendships");
                });

            modelBuilder.Entity("ImageHubAPI.Models.Image", b =>
                {
                    b.Property<string>("ImageId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Path")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("ImageId");

                    b.HasIndex("Path")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Images");

                    b.HasData(
                        new
                        {
                            ImageId = "72282ddc-eeb1-4f8d-97ff-578c2e6eb698",
                            Path = "/23ad2a4f-c1f0-4abc-94c0-52854af2039e/github.png",
                            Title = "github.png",
                            UserId = "23ad2a4f-c1f0-4abc-94c0-52854af2039e"
                        },
                        new
                        {
                            ImageId = "33742155-859d-4adf-b52c-524b439ca685",
                            Path = "/23ad2a4f-c1f0-4abc-94c0-52854af2039e/logo.jpg",
                            Title = "github.png",
                            UserId = "23ad2a4f-c1f0-4abc-94c0-52854af2039e"
                        },
                        new
                        {
                            ImageId = "22074d3a-c05a-4016-8e55-f5bc4d695e98",
                            Path = "/55d8220f-2967-4342-8f6c-e6294a3e52c2/PngItem_6631012.png",
                            Title = "github.png",
                            UserId = "55d8220f-2967-4342-8f6c-e6294a3e52c2"
                        },
                        new
                        {
                            ImageId = "85a2c8b8-abe4-4bbf-a683-47d7e557bffd",
                            Path = "/55d8220f-2967-4342-8f6c-e6294a3e52c2/man-search-for-hiring-job-online-from-laptop_1150-52728.jpg",
                            Title = "github.png",
                            UserId = "55d8220f-2967-4342-8f6c-e6294a3e52c2"
                        });
                });

            modelBuilder.Entity("ImageHubAPI.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Id = "55d8220f-2967-4342-8f6c-e6294a3e52c2",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "c7104aba-53a1-4ceb-a2b6-8114a286f834",
                            Email = "username_1@example.com",
                            EmailConfirmed = false,
                            LockoutEnabled = true,
                            Name = "Username_1",
                            NormalizedEmail = "USERNAME_1@EXAMPLE.COM",
                            NormalizedUserName = "USERNAME_1@EXAMPLE.COM",
                            PasswordHash = "AQAAAAIAAYagAAAAEOqmVlPmLR/sG37g5AZB9at/nO0uSfzikTQUDv3VlJk7w3lffHKsI2wO589vFPUNkw==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "b32da478-203d-433c-8023-13c900ff91ca",
                            TwoFactorEnabled = false,
                            UserName = "username_1@example.com"
                        },
                        new
                        {
                            Id = "23ad2a4f-c1f0-4abc-94c0-52854af2039e",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "20b2d932-129d-4f31-b0bd-a0e88244145a",
                            Email = "username_2@example.com",
                            EmailConfirmed = false,
                            LockoutEnabled = true,
                            Name = "Username_2",
                            NormalizedEmail = "USERNAME_2@EXAMPLE.COM",
                            NormalizedUserName = "USERNAME_2@EXAMPLE.COM",
                            PasswordHash = "AQAAAAIAAYagAAAAECGNeswdJvAFFLya98Hp2gMJUtjaLRH2qaoxFZ80f3Tr8HTvjIjH0x4djwMNZUHhjg==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "aab28f0e-b024-4ec9-857b-33f9ee5246bf",
                            TwoFactorEnabled = false,
                            UserName = "username_2@example.com"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("UserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.ToTable("UserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens");
                });

            modelBuilder.Entity("ImageHubAPI.Models.Friendship", b =>
                {
                    b.HasOne("ImageHubAPI.Models.User", "SecondUser")
                        .WithMany()
                        .HasForeignKey("FriendId");

                    b.HasOne("ImageHubAPI.Models.User", "FirstUser")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.HasOne("ImageHubAPI.Models.User", null)
                        .WithMany("Friendships")
                        .HasForeignKey("UserId1");

                    b.Navigation("FirstUser");

                    b.Navigation("SecondUser");
                });

            modelBuilder.Entity("ImageHubAPI.Models.Image", b =>
                {
                    b.HasOne("ImageHubAPI.Models.User", "User")
                        .WithMany("UserImages")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ImageHubAPI.Models.User", b =>
                {
                    b.Navigation("Friendships");

                    b.Navigation("UserImages");
                });
#pragma warning restore 612, 618
        }
    }
}
