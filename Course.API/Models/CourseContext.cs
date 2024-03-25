using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CourseService.API.Models
{
    public partial class CourseContext : DbContext
    {
        public CourseContext()
        {
        }

        public CourseContext(DbContextOptions<CourseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AnswerExam> AnswerExams { get; set; } = null!;
        public virtual DbSet<AnswerOption> AnswerOptions { get; set; } = null!;
        public virtual DbSet<Chapter> Chapters { get; set; } = null!;
        public virtual DbSet<CompleteLesson> CompleteLessons { get; set; } = null!;
        public virtual DbSet<CompletedExam> CompletedExams { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Enrollment> Enrollments { get; set; } = null!;
        public virtual DbSet<LastExam> LastExams { get; set; } = null!;
        public virtual DbSet<Lesson> Lessons { get; set; } = null!;
        public virtual DbSet<Note> Notes { get; set; } = null!;
        public virtual DbSet<PracticeQuestion> PracticeQuestions { get; set; } = null!;
        public virtual DbSet<QuestionExam> QuestionExams { get; set; } = null!;
        public virtual DbSet<TestCase> TestCases { get; set; } = null!;
        public virtual DbSet<TheoryQuestion> TheoryQuestions { get; set; } = null!;
        public virtual DbSet<UserAnswerCode> UserAnswerCodes { get; set; } = null!;
        public virtual DbSet<UserCourseProgress> UserCourseProgresses { get; set; } = null!;
        public virtual DbSet<Wishlist> Wishlists { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:sep490g13.database.windows.net,1433;Initial Catalog=Course;Persist Security Info=False;User ID=fptu;Password=24082002aA;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnswerExam>(entity =>
            {
                entity.ToTable("AnswerExam");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CorrectAnswer).HasColumnName("Correct_Answer");

                entity.Property(e => e.ExamId).HasColumnName("exam_id");

                entity.Property(e => e.OptionsText).HasColumnName("options_text");

                entity.HasOne(d => d.Exam)
                    .WithMany(p => p.AnswerExams)
                    .HasForeignKey(d => d.ExamId)
                    .HasConstraintName("FK_AnswerExam_Exam");
            });

            modelBuilder.Entity<AnswerOption>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

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

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CourseId).HasColumnName("Course_Id");

                entity.Property(e => e.IsNew).HasColumnName("Is_New");

                entity.Property(e => e.Part).HasColumnType("money");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Chapters)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_Chapter_Course");
            });

            modelBuilder.Entity<CompleteLesson>(entity =>
            {
                entity.ToTable("Complete_Lesson");

                entity.Property(e => e.LessonId).HasColumnName("Lesson_Id");

                entity.Property(e => e.UserId).HasColumnName("User_id");

                entity.HasOne(d => d.Lesson)
                    .WithMany(p => p.CompleteLessons)
                    .HasForeignKey(d => d.LessonId)
                    .HasConstraintName("FK_Complete_Lesson_Lesson");
            });

            modelBuilder.Entity<CompletedExam>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("CompletedExam");

                entity.Property(e => e.LastExamId).HasColumnName("LastExam_Id");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.HasOne(d => d.LastExam)
                    .WithMany()
                    .HasForeignKey(d => d.LastExamId)
                    .HasConstraintName("FK_CompletedExam_LastExam");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Course");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_At");

                entity.Property(e => e.CreatedBy).HasColumnName("Created_By");

                entity.Property(e => e.Tag).HasMaxLength(50);
            });

            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.ToTable("Enrollment");

                entity.Property(e => e.CourseId).HasColumnName("Course_Id");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_Enrollment_Course");
            });

            modelBuilder.Entity<LastExam>(entity =>
            {
                entity.ToTable("LastExam");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ChapterId).HasColumnName("Chapter_Id");

                entity.Property(e => e.IsPass).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.PercentageCompleted).HasColumnName("Percentage_Completed");

                entity.HasOne(d => d.Chapter)
                    .WithMany(p => p.LastExams)
                    .HasForeignKey(d => d.ChapterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LastExam_Chapter");
            });

            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.ToTable("Lesson");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ChapterId).HasColumnName("Chapter_Id");

                entity.Property(e => e.ContentLesson).HasColumnName("Content_Lesson");

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.Property(e => e.VideoUrl).HasColumnName("Video_URL");

                entity.HasOne(d => d.Chapter)
                    .WithMany(p => p.Lessons)
                    .HasForeignKey(d => d.ChapterId)
                    .HasConstraintName("FK_Videos_Chapter");
            });

            modelBuilder.Entity<Note>(entity =>
            {
                entity.ToTable("Note");

                entity.Property(e => e.ContentNote).HasColumnName("Content_Note");

                entity.Property(e => e.LessonId).HasColumnName("Lesson_Id");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.Property(e => e.VideoLink).HasColumnName("Video_Link");

                entity.HasOne(d => d.Lesson)
                    .WithMany(p => p.Notes)
                    .HasForeignKey(d => d.LessonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Note_Lesson");
            });

            modelBuilder.Entity<PracticeQuestion>(entity =>
            {
                entity.ToTable("Practice_Question");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CodeForm).HasColumnName("Code_Form");

                entity.Property(e => e.TestCaseJava).HasColumnName("TestCase_Java");

                entity.HasOne(d => d.Chapter)
                    .WithMany(p => p.PracticeQuestions)
                    .HasForeignKey(d => d.ChapterId)
                    .HasConstraintName("FK_Code_Question_Chapter");
            });

            modelBuilder.Entity<QuestionExam>(entity =>
            {
                entity.ToTable("Question_Exam");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ContentQuestion).HasColumnName("Content_Question");

                entity.Property(e => e.LastExamId).HasColumnName("LastExam_Id");

                entity.HasOne(d => d.LastExam)
                    .WithMany(p => p.QuestionExams)
                    .HasForeignKey(d => d.LastExamId)
                    .HasConstraintName("FK_Exam_LastExam");
            });

            modelBuilder.Entity<TestCase>(entity =>
            {
                entity.ToTable("TestCase");

                entity.Property(e => e.Id).ValueGeneratedNever();

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

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ContentQuestion).HasColumnName("Content_Question");

                entity.Property(e => e.TimeQuestion).HasColumnName("Time_Question");

                entity.Property(e => e.VideoId).HasColumnName("Video_Id");

                entity.HasOne(d => d.Video)
                    .WithMany(p => p.TheoryQuestions)
                    .HasForeignKey(d => d.VideoId)
                    .HasConstraintName("FK_Questions_Videos");
            });

            modelBuilder.Entity<UserAnswerCode>(entity =>
            {
                entity.ToTable("User_Answer_Code");

                entity.Property(e => e.AnswerCode).HasColumnName("Answer_Code");

                entity.Property(e => e.CodeQuestionId).HasColumnName("Code_Question_Id");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.HasOne(d => d.CodeQuestion)
                    .WithMany(p => p.UserAnswerCodes)
                    .HasForeignKey(d => d.CodeQuestionId)
                    .HasConstraintName("FK_User_Answer_Code_Code_Question");
            });

            modelBuilder.Entity<UserCourseProgress>(entity =>
            {
                entity.ToTable("UserCourseProgress");

                entity.Property(e => e.CourseId).HasColumnName("Course_Id");

                entity.Property(e => e.CurrentChapterId).HasColumnName("Current_Chapter_Id");

                entity.Property(e => e.CurrentLessonId).HasColumnName("Current_Lesson_Id");

                entity.Property(e => e.UserId).HasColumnName("User_Id");
            });

            modelBuilder.Entity<Wishlist>(entity =>
            {
                entity.ToTable("Wishlist");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
