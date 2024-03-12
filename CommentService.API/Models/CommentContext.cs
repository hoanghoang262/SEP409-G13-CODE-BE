using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CommentService.API.Models
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
        public virtual DbSet<Reply> Replies { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
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
            });

            modelBuilder.Entity<Reply>(entity =>
            {
                entity.Property(e => e.CommentId).HasColumnName("Comment_Id");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date")
                    .HasColumnName("Create_Date");

                entity.Property(e => e.ReplyContent).HasColumnName("Reply_Content");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.Replies)
                    .HasForeignKey(d => d.CommentId)
                    .HasConstraintName("FK_Replies_Comment");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
