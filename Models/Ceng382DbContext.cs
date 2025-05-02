using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Week5Lab.Models
{
    public partial class Ceng382DbContext : DbContext
    {
        public Ceng382DbContext()
        {
        }

        // AI Prompt: "Initialize DbContext using options passed from dependency injection"
        public Ceng382DbContext(DbContextOptions<Ceng382DbContext> options)
            : base(options)
        {
        }

        
        // Represents the Students table in the database.
        // AI Prompt: "Define DbSet to represent the Students table in the database"
        public virtual DbSet<Student> Students { get; set; } = null!;

        
        //Represents the ClassInformation table in the database (Week9)
        // AI Prompt: "Define DbSet for ClassInformation table that stores class-related data (used in Week9)"
        public virtual DbSet<ClassInformation> ClassInformation { get; set; } = null!;

         // AI Prompt: "Configure SQL Server connection string if not already configured externally"
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // AI Prompt: "Connect to local SQL Server instance with trusted connection"
                optionsBuilder.UseSqlServer("Server=MERT-MONSTER\\SQLEXPRESS;Database=Ceng382DB;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }
        // AI Prompt: "Configure table schema details using Fluent API for entity mapping
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             // AI Prompt: "Set primary key and column properties for Student table"
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Students");

                entity.Property(e => e.Name)
                      .HasMaxLength(100);

                entity.Property(e => e.Department)
                      .HasMaxLength(100);
            });

            modelBuilder.Entity<ClassInformation>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_ClassInformation");

                entity.Property(e => e.ClassName).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(255);
            });
            
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
