﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WeatherAlertAPI.Data;

#nullable disable

namespace WeatherAlertAPI.Migrations
{
    [DbContext(typeof(WeatherDbContext))]
    partial class WeatherDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.5");

            modelBuilder.Entity("WeatherAlertAPI.Models.Cidade", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<decimal?>("Latitude")
                        .HasPrecision(10, 6)
                        .HasColumnType("TEXT")
                        .HasColumnName("latitude");

                    b.Property<decimal?>("Longitude")
                        .HasPrecision(10, 6)
                        .HasColumnType("TEXT")
                        .HasColumnName("longitude");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasColumnName("nome");

                    b.HasKey("Id");

                    b.ToTable("cidade");
                });

            modelBuilder.Entity("WeatherAlertAPI.Models.Dica", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<string>("Mensagem")
                        .HasColumnType("TEXT")
                        .HasColumnName("mensagem");

                    b.Property<string>("NivelRisco")
                        .HasMaxLength(20)
                        .HasColumnType("TEXT")
                        .HasColumnName("nivel_risco");

                    b.HasKey("Id");

                    b.ToTable("dica");
                });
#pragma warning restore 612, 618
        }
    }
}
