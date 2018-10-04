﻿// <auto-generated />
using System;
using AdvancedApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AdvancedApp.Migrations
{
    [DbContext(typeof(AdvancedContext))]
    [Migration("20181004124021_DefaultValue")]
    partial class DefaultValue
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AdvancedApp.Models.Employee", b =>
                {
                    b.Property<string>("SSN");

                    b.Property<string>("FirstName");

                    b.Property<string>("FamilyName");

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

                    b.Property<decimal>("Salary")
                        .HasColumnType("decimal(8,2)");

                    b.Property<bool>("SoftDeleted");

                    b.HasKey("SSN", "FirstName", "FamilyName");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("AdvancedApp.Models.SecondaryIdentity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("InActiveUse");

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.Property<string>("PrimaryFamilyName");

                    b.Property<string>("PrimaryFirstName");

                    b.Property<string>("PrimarySSN");

                    b.HasKey("Id");

                    b.HasIndex("PrimarySSN", "PrimaryFirstName", "PrimaryFamilyName")
                        .IsUnique()
                        .HasFilter("[PrimarySSN] IS NOT NULL AND [PrimaryFirstName] IS NOT NULL AND [PrimaryFamilyName] IS NOT NULL");

                    b.ToTable("SecondaryIdentity");
                });

            modelBuilder.Entity("AdvancedApp.Models.SecondaryIdentity", b =>
                {
                    b.HasOne("AdvancedApp.Models.Employee", "PrimaryIdentity")
                        .WithOne("OtherIdentity")
                        .HasForeignKey("AdvancedApp.Models.SecondaryIdentity", "PrimarySSN", "PrimaryFirstName", "PrimaryFamilyName");
                });
#pragma warning restore 612, 618
        }
    }
}
