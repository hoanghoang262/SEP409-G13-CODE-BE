using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CommentGrpc.Models
{
    public partial class CommentContext : DbContext
    {
        public CommentContext()
        {
        }

        public CommentContext(DbContextOptions<CommentContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Comment> Comments { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=tcp:fptulearnserver.database.windows.net,1433;Initial Catalog=Comment;Persist Security Info=False;User ID=fptu;Password=24082002aA;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.Property(e => e.CommentContent).HasColumnName("Comment_Content");

                entity.Property(e => e.CourseId).HasColumnName("Course_Id");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.ForumPostId).HasColumnName("Forum_Post_Id");

                entity.Property(e => e.LessonId).HasColumnName("Lesson_Id");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
