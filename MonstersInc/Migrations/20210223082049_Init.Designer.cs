﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MonstersAPI.Models;

namespace MonstersAPI.Migrations
{
    [DbContext(typeof(MonstersIncDbContext))]
    [Migration("20210223082049_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MonstersAPI.Models.DepletedDoor", b =>
                {
                    b.Property<string>("DepletedDoorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("ClosedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("DoorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OpenedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("WorkDayId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("DepletedDoorId");

                    b.HasIndex("WorkDayId");

                    b.ToTable("DepletedDoor");
                });

            modelBuilder.Entity("MonstersAPI.Models.Door", b =>
                {
                    b.Property<string>("DoorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Energy")
                        .HasColumnType("int");

                    b.Property<bool>("IsOpen")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastUsed")
                        .HasColumnType("datetime2");

                    b.Property<string>("WorkDayId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("DoorId");

                    b.HasIndex("WorkDayId");

                    b.ToTable("Doors");
                });

            modelBuilder.Entity("MonstersAPI.Models.WorkDay", b =>
                {
                    b.Property<string>("WorkDayId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Begin")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("End")
                        .HasColumnType("datetime2");

                    b.Property<int>("EnergyCollected")
                        .HasColumnType("int");

                    b.Property<int>("EnergyGoal")
                        .HasColumnType("int");

                    b.Property<string>("IntimidatorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("WorkDayId");

                    b.ToTable("WorkDays");
                });

            modelBuilder.Entity("MonstersAPI.Models.DepletedDoor", b =>
                {
                    b.HasOne("MonstersAPI.Models.WorkDay", null)
                        .WithMany("DepletedDoors")
                        .HasForeignKey("WorkDayId");
                });

            modelBuilder.Entity("MonstersAPI.Models.Door", b =>
                {
                    b.HasOne("MonstersAPI.Models.WorkDay", null)
                        .WithMany("Doors")
                        .HasForeignKey("WorkDayId");
                });
#pragma warning restore 612, 618
        }
    }
}
