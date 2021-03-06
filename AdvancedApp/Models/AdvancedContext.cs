﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvancedApp.Models
{
    public class AdvancedContext : DbContext
    {
        public AdvancedContext(DbContextOptions<AdvancedContext> options) : base(options)
        {
            //ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            Database.AutoTransactionsEnabled = false;
        }
        public DbSet<Employee> Employees { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<Employee>().Property(e => e.Id).ForSqlServerUseSequenceHiLo();
            // modelBuilder.Entity<Employee>().HasIndex(e => e.SSN).HasName("SSNIndex").IsUnique();
            // modelBuilder.Entity<Employee>().HasAlternateKey(e => e.SSN);

               modelBuilder.Entity<Employee>().HasQueryFilter(e => !e.SoftDeleted);

            modelBuilder.Entity<Employee>().Ignore(e => e.Id);
            //modelBuilder.Entity<Employee>().HasKey(e => e.SSN);
            modelBuilder.Entity<Employee>().HasKey(e => new { e.SSN, e.FirstName, e.FamilyName });
            modelBuilder.Entity<Employee>().Property(e => e.Salary)
                .HasColumnType("decimal(8,2)")
                .HasField("databaseSalary")
                .UsePropertyAccessMode(PropertyAccessMode.Field);
                //.IsConcurrencyToken();

            modelBuilder.Entity<Employee>().Property<DateTime>("LastUpdated").HasDefaultValue(new DateTime(2000, 1, 1));
           // modelBuilder.Entity<Employee>().Property(e => e.GeneratedValue).HasDefaultValueSql("GETDATE()");

            modelBuilder.HasSequence<int>("ReferenceSequence").StartsAt(100).IncrementsBy(2);
            //modelBuilder.Entity<Employee>().Property(e => e.GeneratedValue)
            //   // .HasDefaultValueSql(@"'REFERENCE_' + CONVERT(varchar, NEXT VALUE FOR ReferenceSequence)");
            //.HasComputedColumnSql(@"SUBSTRING(FirstName, 1, 1) + FamilyName PERSISTED");
            //modelBuilder.Entity<Employee>().HasIndex(e => e.GeneratedValue);

            modelBuilder.Entity<Employee>().Property(e => e.GeneratedValue).ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<Employee>()
                .Ignore(e => e.RowVersion);
            //.Property(e => e.RowVersion).IsRowVersion();

            modelBuilder.Entity<SecondaryIdentity>()
                .HasOne(s => s.PrimaryIdentity)
                .WithOne(e => e.OtherIdentity)
                //.HasPrincipalKey<Employee>(e => e.SSN)
                //.HasForeignKey<SecondaryIdentity>(s => s.PrimarySSN);
                .HasPrincipalKey<Employee>(e => new { e.SSN, e.FirstName, e.FamilyName })
                .HasForeignKey<SecondaryIdentity>(s => new { s.PrimarySSN, s.PrimaryFirstName, s.PrimaryFamilyName })
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SecondaryIdentity>().Property(e => e.Name).HasMaxLength(100);
        }
    }
}
