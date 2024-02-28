using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ModerationService.API.Models
{
    public partial class Content_ModerationContext : DbContext
    {
        public Content_ModerationContext()
        {
        }

        public Content_ModerationContext(DbContextOptions<Content_ModerationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Chapter> Chapters { get; set; } = null!;
        public virtual DbSet<CodeQuestion> CodeQuestions { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Lesson> Lessons { get; set; } = null!;
        public virtual DbSet<Moderation> Moderations { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<TestCase> TestCases { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=localhost\\sqlserverdb,1435;database=Content_Moderation;uid=sa;pwd=PassW0rd!;TrustServerCertificate=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chapter>(entity =>
            {
                entity.ToTable("Chapter");

                entity.Property(e => e.CourseId).HasColumnName("Course_Id");

                entity.Property(e => e.IsNew).HasColumnName("Is_New");

                entity.Property(e => e.Part).HasColumnType("money");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Chapters)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_Chapter_Course");
            });

            modelBuilder.Entity<CodeQuestion>(entity =>
            {
                entity.ToTable("Code_Question");

                entity.Property(e => e.ChapterId).HasColumnName("Chapter_Id");

                entity.HasOne(d => d.Chapter)
                    .WithMany(p => p.CodeQuestions)
                    .HasForeignKey(d => d.ChapterId)
                    .HasConstraintName("FK_Code_Question_Chapter");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Course");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_At");

                entity.Property(e => e.CreatedBy).HasColumnName("Created_By");

                entity.Property(e => e.Tag).HasMaxLength(50);
            });

            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.ToTable("Lesson");

                entity.Property(e => e.ChapterId).HasColumnName("Chapter_Id");

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.Property(e => e.VideoUrl).HasColumnName("Video_URL");

                entity.HasOne(d => d.Chapter)
                    .WithMany(p => p.Lessons)
                    .HasForeignKey(d => d.ChapterId)
                    .HasConstraintName("FK_Videos_Chapter");
            });

            modelBuilder.Entity<Moderation>(entity =>
            {
                entity.ToTable("Moderation");

                entity.Property(e => e.ApprovedContent).HasColumnName("Approved_Content");

                entity.Property(e => e.ChangeType)
                    .HasMaxLength(50)
                    .HasColumnName("Change_Type");

                entity.Property(e => e.CourseId).HasColumnName("Course_Id");

                entity.Property(e => e.CourseName)
                    .HasMaxLength(200)
                    .HasColumnName("Course_Name");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_At");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("Created_By");

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Moderations)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_Moderation_Course");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.AnswerA).HasColumnName("Answer_A");

                entity.Property(e => e.AnswerB).HasColumnName("Answer_B");

                entity.Property(e => e.AnswerC).HasColumnName("Answer_C");

                entity.Property(e => e.AnswerD).HasColumnName("Answer_D");

                entity.Property(e => e.ContentQuestion).HasColumnName("Content_Question");

                entity.Property(e => e.CorrectAnswer)
                    .HasMaxLength(50)
                    .HasColumnName("Correct_Answer");

                entity.Property(e => e.VideoId).HasColumnName("Video_Id");

                entity.HasOne(d => d.Video)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.VideoId)
                    .HasConstraintName("FK_Questions_Videos");
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
