using Microsoft.EntityFrameworkCore;
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

            modelBuilder.Entity<SecondaryIdentity>()
                .HasOne(s => s.PrimaryIdentity)
                .WithOne(e => e.OtherIdentity)
                //.HasPrincipalKey<Employee>(e => e.SSN)
                //.HasForeignKey<SecondaryIdentity>(s => s.PrimarySSN);
                .HasPrincipalKey<Employee>(e => new { e.SSN, e.FirstName, e.FamilyName }).HasForeignKey<SecondaryIdentity>(s => new { s.PrimarySSN, s.PrimaryFirstName, s.PrimaryFamilyName });
        }
    }
}
