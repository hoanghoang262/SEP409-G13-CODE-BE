using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PaymentService.API.Models
{
    public partial class PaymentContext : DbContext
    {
        public PaymentContext()
        {
        }

        public PaymentContext(DbContextOptions<PaymentContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<PaymentTransaction> PaymentTransactions { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:sep490g132.database.windows.net,1433;Initial Catalog=Payment;Persist Security Info=False;User ID=fptu;Password=24082002aA;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.PaymentId).HasMaxLength(50);

                entity.Property(e => e.BuyerId).HasColumnName("Buyer_Id");

                entity.Property(e => e.CourseId).HasColumnName("Course_Id");

                entity.Property(e => e.ExpireDate).HasColumnType("datetime");

                entity.Property(e => e.MerchantId).HasMaxLength(50);

                entity.Property(e => e.PaidAmount).HasColumnType("decimal(19, 2)");

                entity.Property(e => e.PaymentContent).HasMaxLength(250);

                entity.Property(e => e.PaymentCurrency).HasMaxLength(50);

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentDestinationId).HasMaxLength(50);

                entity.Property(e => e.PaymentLanguage).HasMaxLength(50);

                entity.Property(e => e.PaymentLastMessage).HasMaxLength(250);

                entity.Property(e => e.PaymentRefId).HasMaxLength(50);

                entity.Property(e => e.PaymentStatus).HasMaxLength(20);

                entity.Property(e => e.RequriedAmount).HasColumnType("decimal(19, 2)");

                entity.Property(e => e.UserCreateCourseId).HasColumnName("UserCreateCourse_Id");
            });

            modelBuilder.Entity<PaymentTransaction>(entity =>
            {
                entity.ToTable("PaymentTransaction");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.BuyerId).HasColumnName("Buyer_Id");

                entity.Property(e => e.CourseId).HasColumnName("Course_Id");

                entity.Property(e => e.PaymentId).HasMaxLength(50);

                entity.Property(e => e.TranDate).HasColumnType("datetime");

                entity.Property(e => e.TranMessage).HasMaxLength(250);

                entity.Property(e => e.TranPayLoad).HasMaxLength(500);

                entity.Property(e => e.TranStatus).HasMaxLength(50);

                entity.Property(e => e.TransAmount).HasColumnType("decimal(19, 2)");

                entity.Property(e => e.UserCreateCourseId).HasColumnName("UserCreateCourse_Id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
