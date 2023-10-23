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
            modelBuilder.HasAnnotation("ProductVersion", "7.0.12");

            modelBuilder.Entity("ImageHubAPI.Models.FriendRequest", b =>
                {
                    b.Property<string>("FriendRequestId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ReceiverId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RequesteId")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("FriendRequestId");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("RequesteId");

                    b.HasIndex("UserId");

                    b.ToTable("FriendRequests");
                });

            modelBuilder.Entity("ImageHubAPI.Models.Friendship", b =>
                {
                    b.Property<string>("FriendshipId")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("SecondUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("FriendshipId");

                    b.HasIndex("FirstUserId");

                    b.HasIndex("SecondUserId");

                    b.HasIndex("UserId");

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

                    b.HasIndex("UserId");

                    b.ToTable("Images");
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

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ImageHubAPI.Models.FriendRequest", b =>
                {
                    b.HasOne("ImageHubAPI.Models.User", "Receiver")
                        .WithMany()
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ImageHubAPI.Models.User", "Requester")
                        .WithMany()
                        .HasForeignKey("RequesteId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ImageHubAPI.Models.User", null)
                        .WithMany("FriendRequests")
                        .HasForeignKey("UserId");

                    b.Navigation("Receiver");

                    b.Navigation("Requester");
                });

            modelBuilder.Entity("ImageHubAPI.Models.Friendship", b =>
                {
                    b.HasOne("ImageHubAPI.Models.User", "FirstUser")
                        .WithMany()
                        .HasForeignKey("FirstUserId");

                    b.HasOne("ImageHubAPI.Models.User", "SecondUser")
                        .WithMany()
                        .HasForeignKey("SecondUserId");

                    b.HasOne("ImageHubAPI.Models.User", null)
                        .WithMany("Friendships")
                        .HasForeignKey("UserId");

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
                    b.Navigation("FriendRequests");

                    b.Navigation("Friendships");

                    b.Navigation("UserImages");
                });
#pragma warning restore 612, 618
        }
    }
}
