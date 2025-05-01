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

        public Ceng382DbContext(DbContextOptions<Ceng382DbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Represents the Students table in the database.
        /// </summary>
        public virtual DbSet<Student> Students { get; set; } = null!;

        /// <summary>
        /// Represents the ClassInformation table in the database (Week9)
        /// </summary>
        public virtual DbSet<ClassInformation> ClassInformation { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Connection string is hardcoded here; ideally move to appsettings.json
                optionsBuilder.UseSqlServer("Server=MERT-MONSTER\\SQLEXPRESS;Database=Ceng382DB;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
