using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CompileCodeOnline.Models
{
    public partial class CompileCodeContext : DbContext
    {
        public CompileCodeContext()
        {
        }

        public CompileCodeContext(DbContextOptions<CompileCodeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CodeQuestion> CodeQuestions { get; set; } = null!;
        public virtual DbSet<TestCase> TestCases { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=localhost\\sqlserverdb,1435;database=CompileCode;uid=sa;pwd=PassW0rd!;TrustServerCertificate=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CodeQuestion>(entity =>
            {
                entity.ToTable("Code_Question");
            });

            modelBuilder.Entity<TestCase>(entity =>
            {
                entity.ToTable("TestCase");

                entity.Property(e => e.CodeQuestionId).HasColumnName("Code_Question_Id");

                entity.Property(e => e.ExpectedResultString).HasMaxLength(50);

                entity.HasOne(d => d.CodeQuestion)
                    .WithMany(p => p.TestCases)
                    .HasForeignKey(d => d.CodeQuestionId)
                    .HasConstraintName("FK_TestCase_Code_Question");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
