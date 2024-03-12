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

        public virtual DbSet<AnswerOption> AnswerOptions { get; set; } = null!;
        public virtual DbSet<Chapter> Chapters { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Lesson> Lessons { get; set; } = null!;
        public virtual DbSet<Moderation> Moderations { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<PracticeQuestion> PracticeQuestions { get; set; } = null!;
        public virtual DbSet<TestCase> TestCases { get; set; } = null!;
        public virtual DbSet<TheoryQuestion> TheoryQuestions { get; set; } = null!;
        public virtual DbSet<UserAnswerCode> UserAnswerCodes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:fptulearnserver.database.windows.net,1433;Initial Catalog=Content_Moderation;Persist Security Info=False;User ID=fptu;Password=24082002aA;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnswerOption>(entity =>
            {
                entity.Property(e => e.CorrectAnswer).HasColumnName("Correct_Answer");

                entity.Property(e => e.OptionsText).HasColumnName("options_text");

                entity.Property(e => e.QuestionId).HasColumnName("question_id");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.AnswerOptions)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK_AnswerOptions_Questions");
            });

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

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Course");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_At");

                entity.Property(e => e.CreatedBy).HasColumnName("Created_By");

                entity.Property(e => e.IsCompleted).HasColumnName("Is_Completed");

                entity.Property(e => e.Tag).HasMaxLength(50);
            });

            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.ToTable("Lesson");

                entity.Property(e => e.ChapterId).HasColumnName("Chapter_Id");

                entity.Property(e => e.ContentLesson).HasColumnName("Content_Lesson");

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

                entity.Property(e => e.CreatedBy).HasColumnName("Created_By");

                entity.Property(e => e.PostId).HasColumnName("Post_Id");

                entity.Property(e => e.PostTitle).HasColumnName("Post_Title");

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Moderations)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_Moderation_Course");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Post");

                entity.Property(e => e.CreatedBy).HasColumnName("Created_By");

                entity.Property(e => e.LastUpdate)
                    .HasColumnType("datetime")
                    .HasColumnName("Last_Update");

                entity.Property(e => e.PostContent).HasColumnName("Post_Content");
            });

            modelBuilder.Entity<PracticeQuestion>(entity =>
            {
                entity.ToTable("Practice_Question");

                entity.Property(e => e.ChapterId).HasColumnName("Chapter_Id");

                entity.Property(e => e.CodeForm).HasColumnName("Code_Form");

                entity.Property(e => e.TestCaseJava).HasColumnName("TestCase_Java");

                entity.HasOne(d => d.Chapter)
                    .WithMany(p => p.PracticeQuestions)
                    .HasForeignKey(d => d.ChapterId)
                    .HasConstraintName("FK_Code_Question_Chapter");
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

            modelBuilder.Entity<TheoryQuestion>(entity =>
            {
                entity.ToTable("Theory_Questions");

                entity.Property(e => e.ContentQuestion).HasColumnName("Content_Question");

                entity.Property(e => e.TimeQuestion)
                    .HasMaxLength(10)
                    .HasColumnName("Time_Question")
                    .IsFixedLength();

                entity.Property(e => e.VideoId).HasColumnName("Video_Id");

                entity.HasOne(d => d.Video)
                    .WithMany(p => p.TheoryQuestions)
                    .HasForeignKey(d => d.VideoId)
                    .HasConstraintName("FK_Questions_Videos");
            });

            modelBuilder.Entity<UserAnswerCode>(entity =>
            {
                entity.ToTable("User Answer Code");

                entity.Property(e => e.AnswerCode).HasColumnName("Answer_Code");

                entity.Property(e => e.CodeQuestionId).HasColumnName("Code_Question_Id");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.HasOne(d => d.CodeQuestion)
                    .WithMany(p => p.UserAnswerCodes)
                    .HasForeignKey(d => d.CodeQuestionId)
                    .HasConstraintName("FK_User Answer Code_Code_Question");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
