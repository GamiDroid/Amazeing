﻿// <auto-generated />
using System;
using Amazeing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Amazeing.Migrations
{
    [DbContext(typeof(AmazeingDbContext))]
    [Migration("20231205175341_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("Amazeing.Models.MazeInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("PotentialReward")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TotalTiles")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Mazes");
                });
#pragma warning restore 612, 618
        }
    }
}