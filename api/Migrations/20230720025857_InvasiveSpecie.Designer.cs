﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using api;

#nullable disable

namespace api.Migrations
{
    [DbContext(typeof(DBContext))]
    [Migration("20230720025857_InvasiveSpecie")]
    partial class InvasiveSpecie
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("api.Models.CategoryNaturalArea", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.HasKey("Id");

                    b.ToTable("CategoryNaturalArea", (string)null);
                });

            modelBuilder.Entity("api.Models.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("DepartmentId")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<float?>("Population")
                        .HasColumnType("real");

                    b.Property<string>("PostalCode")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<float?>("Surface")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("City", (string)null);
                });

            modelBuilder.Entity("api.Models.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AircraftPrefix")
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)");

                    b.Property<string[]>("Borders")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("Currency")
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.Property<string>("CurrencyCode")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<string>("CurrencySymbol")
                        .HasMaxLength(2)
                        .HasColumnType("character varying(2)");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string[]>("Flags")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("ISOCode")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<string>("InternetDomain")
                        .HasColumnType("text");

                    b.Property<string[]>("Languages")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("PhonePrefix")
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)");

                    b.Property<float>("Population")
                        .HasColumnType("real");

                    b.Property<string>("RadioPrefix")
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)");

                    b.Property<string>("Region")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("StateCapital")
                        .HasColumnType("text");

                    b.Property<string>("SubRegion")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<float>("Surface")
                        .HasColumnType("real");

                    b.Property<string>("TimeZone")
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.HasKey("Id");

                    b.ToTable("Country", (string)null);
                });

            modelBuilder.Entity("api.Models.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("CityCapitalId")
                        .HasColumnType("integer");

                    b.Property<int>("CityCapitalId1")
                        .HasColumnType("integer");

                    b.Property<int>("CountryId")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int?>("Municipalities")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("PhonePrefix")
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)");

                    b.Property<float?>("Population")
                        .HasColumnType("real");

                    b.Property<int?>("RegionId")
                        .HasColumnType("integer");

                    b.Property<float>("Surface")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("CityCapitalId1");

                    b.HasIndex("CountryId");

                    b.HasIndex("RegionId");

                    b.ToTable("Department", (string)null);
                });

            modelBuilder.Entity("api.Models.InvasiveSpecie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CommonNames")
                        .HasColumnType("text");

                    b.Property<string>("Impact")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Manage")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<int>("RiskLevel")
                        .HasColumnType("integer");

                    b.Property<string>("ScientificName")
                        .HasColumnType("text");

                    b.Property<string>("UrlImage")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("InvasiveSpecie", (string)null);
                });

            modelBuilder.Entity("api.Models.Map", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("DepartmentId")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.Property<string[]>("UrlImages")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("UrlSource")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Map", (string)null);
                });

            modelBuilder.Entity("api.Models.NaturalArea", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("AreaGroupId")
                        .HasColumnType("integer");

                    b.Property<int>("CategoryNaturalAreaId")
                        .HasColumnType("integer");

                    b.Property<int?>("DaneCode")
                        .HasColumnType("integer");

                    b.Property<int?>("DepartmentId")
                        .HasColumnType("integer");

                    b.Property<double?>("LandArea")
                        .HasColumnType("double precision");

                    b.Property<double?>("MaritimeArea")
                        .HasColumnType("double precision");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryNaturalAreaId");

                    b.HasIndex("DepartmentId");

                    b.ToTable("NaturalArea", (string)null);
                });

            modelBuilder.Entity("api.Models.President", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CityId")
                        .HasColumnType("integer");

                    b.Property<int?>("CountryId")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateOnly?>("EndPeriodDate")
                        .HasColumnType("date");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("PoliticalParty")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateOnly>("StartPeriodDate")
                        .HasColumnType("date");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("CountryId");

                    b.ToTable("President", (string)null);
                });

            modelBuilder.Entity("api.Models.Region", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.HasKey("Id");

                    b.ToTable("Region", (string)null);
                });

            modelBuilder.Entity("api.Models.TouristAttraction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CityId")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string[]>("Images")
                        .HasColumnType("text[]");

                    b.Property<string>("Latitude")
                        .HasColumnType("text");

                    b.Property<string>("Longitude")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("TouristAttraction", (string)null);
                });

            modelBuilder.Entity("api.Models.City", b =>
                {
                    b.HasOne("api.Models.Department", "Department")
                        .WithMany("Cities")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("api.Models.Department", b =>
                {
                    b.HasOne("api.Models.City", "CityCapital")
                        .WithMany()
                        .HasForeignKey("CityCapitalId1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api.Models.Country", "Country")
                        .WithMany("Departments")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api.Models.Region", "Region")
                        .WithMany("Departments")
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CityCapital");

                    b.Navigation("Country");

                    b.Navigation("Region");
                });

            modelBuilder.Entity("api.Models.Map", b =>
                {
                    b.HasOne("api.Models.Department", "Department")
                        .WithMany("Maps")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("api.Models.NaturalArea", b =>
                {
                    b.HasOne("api.Models.CategoryNaturalArea", "CategoryNaturalArea")
                        .WithMany("NaturalAreas")
                        .HasForeignKey("CategoryNaturalAreaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api.Models.Department", "Department")
                        .WithMany("NaturalAreas")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CategoryNaturalArea");

                    b.Navigation("Department");
                });

            modelBuilder.Entity("api.Models.President", b =>
                {
                    b.HasOne("api.Models.City", "City")
                        .WithMany("Presidents")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api.Models.Country", null)
                        .WithMany("Presidents")
                        .HasForeignKey("CountryId");

                    b.Navigation("City");
                });

            modelBuilder.Entity("api.Models.TouristAttraction", b =>
                {
                    b.HasOne("api.Models.City", "City")
                        .WithMany("TouristAttractions")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("api.Models.CategoryNaturalArea", b =>
                {
                    b.Navigation("NaturalAreas");
                });

            modelBuilder.Entity("api.Models.City", b =>
                {
                    b.Navigation("Presidents");

                    b.Navigation("TouristAttractions");
                });

            modelBuilder.Entity("api.Models.Country", b =>
                {
                    b.Navigation("Departments");

                    b.Navigation("Presidents");
                });

            modelBuilder.Entity("api.Models.Department", b =>
                {
                    b.Navigation("Cities");

                    b.Navigation("Maps");

                    b.Navigation("NaturalAreas");
                });

            modelBuilder.Entity("api.Models.Region", b =>
                {
                    b.Navigation("Departments");
                });
#pragma warning restore 612, 618
        }
    }
}
