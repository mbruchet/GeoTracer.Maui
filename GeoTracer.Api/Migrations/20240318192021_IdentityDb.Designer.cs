﻿// <auto-generated />
using System;
using GeoTracer.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GeoTracer.Api.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240318192021_IdentityDb")]
    partial class IdentityDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GeoTracer.Shared.AccessToken", b =>
                {
                    b.Property<string>("AccessTokenId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AccessTokenValue")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AccessTokenId");

                    b.ToTable("AccessTokens");
                });

            modelBuilder.Entity("GeoTracer.Shared.User", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("JsRoles")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Roles")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SaltPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = "c7d88998-a13b-49da-8449-4e3437327614",
                            Email = "admin@monexample.com",
                            JsRoles = "admin",
                            Name = "admin",
                            Password = "Q1g+vevvsaWFThuHd6pUPhLpmqMbDEECGGst0lhxzwc=",
                            Roles = "[\"admin\"]",
                            SaltPassword = "4hwuiTkyQdrmCTUbStl3RFeBeZEzVQhRQrqHL64qHns=",
                            UserName = "admin@monexample.com"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
