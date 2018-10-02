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
        }
        public DbSet<Employee> Employees { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<Employee>().Property(e => e.Id).ForSqlServerUseSequenceHiLo();
            // modelBuilder.Entity<Employee>().HasIndex(e => e.SSN).HasName("SSNIndex").IsUnique();
            // modelBuilder.Entity<Employee>().HasAlternateKey(e => e.SSN);

            modelBuilder.Entity<Employee>().Ignore(e => e.Id);
            modelBuilder.Entity<Employee>().HasKey(e => e.SSN);

            modelBuilder.Entity<SecondaryIdentity>()
                .HasOne(s => s.PrimaryIdentity)
                .WithOne(e => e.OtherIdentity)
                .HasPrincipalKey<Employee>(e => e.SSN)
                .HasForeignKey<SecondaryIdentity>(s => s.PrimarySSN);
        }
    }
}
