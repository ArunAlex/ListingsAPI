﻿// <auto-generated />
using System;
using Listings.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Listings.Persistance.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241006114404_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("Listings.Domain.Models.Listing", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Postcode")
                        .HasColumnType("INTEGER");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Suburb")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Listings");
                });

            modelBuilder.Entity("Listings.Domain.Models.SavedListing", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ListingId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("SavedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "ListingId");

                    b.HasIndex("ListingId");

                    b.ToTable("SavedListings");
                });

            modelBuilder.Entity("Listings.Domain.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Listings.Domain.Models.SavedListing", b =>
                {
                    b.HasOne("Listings.Domain.Models.Listing", "Listing")
                        .WithMany("SavedByUsers")
                        .HasForeignKey("ListingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Listings.Domain.Models.User", "User")
                        .WithMany("SavedListings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Listing");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Listings.Domain.Models.Listing", b =>
                {
                    b.Navigation("SavedByUsers");
                });

            modelBuilder.Entity("Listings.Domain.Models.User", b =>
                {
                    b.Navigation("SavedListings");
                });
#pragma warning restore 612, 618
        }
    }
}
