using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PaymentService.Models
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

        public virtual DbSet<Merchant> Merchants { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<PaymentDestination> PaymentDestinations { get; set; } = null!;
        public virtual DbSet<PaymentNotification> PaymentNotifications { get; set; } = null!;
        public virtual DbSet<PaymentSignature> PaymentSignatures { get; set; } = null!;
        public virtual DbSet<PaymentTransaction> PaymentTransactions { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:fptulearnserver.database.windows.net,1433;Initial Catalog=Payment;Persist Security Info=False;User ID=fptu;Password=24082002aA;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Merchant>(entity =>
            {
                entity.ToTable("Merchant");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.LastUpdateAt).HasColumnType("datetime");

                entity.Property(e => e.LastUpdateBy).HasMaxLength(50);

                entity.Property(e => e.MerchantIpnUrl).HasMaxLength(250);

                entity.Property(e => e.MerchantName).HasMaxLength(50);

                entity.Property(e => e.MerchantReturnUrl).HasMaxLength(250);

                entity.Property(e => e.MerchantWebLink).HasMaxLength(250);

                entity.Property(e => e.SecretKey).HasMaxLength(50);
            });

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

            modelBuilder.Entity<PaymentDestination>(entity =>
            {
                entity.ToTable("PaymentDestination");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.DesLogo).HasMaxLength(250);

                entity.Property(e => e.DesName).HasMaxLength(250);

                entity.Property(e => e.DesShortName).HasMaxLength(50);

                entity.Property(e => e.ParentId).HasMaxLength(50);

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_PaymentDestination_PaymentDestination");
            });

            modelBuilder.Entity<PaymentNotification>(entity =>
            {
                entity.ToTable("PaymentNotification");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.MerchantId).HasMaxLength(50);

                entity.Property(e => e.NotiAmount).HasMaxLength(50);

                entity.Property(e => e.NotiContent).HasMaxLength(50);

                entity.Property(e => e.NotiDate).HasMaxLength(50);

                entity.Property(e => e.NotiMessage).HasMaxLength(50);

                entity.Property(e => e.NotiResDate).HasMaxLength(50);

                entity.Property(e => e.NotiSignature).HasMaxLength(50);

                entity.Property(e => e.NotiStatus).HasMaxLength(50);

                entity.Property(e => e.PaymentId).HasMaxLength(50);

                entity.Property(e => e.PaymentRefId).HasMaxLength(50);

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.PaymentNotifications)
                    .HasForeignKey(d => d.PaymentId)
                    .HasConstraintName("FK_PaymentNotification_Payment");
            });

            modelBuilder.Entity<PaymentSignature>(entity =>
            {
                entity.ToTable("PaymentSignature");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Paymentd).HasMaxLength(50);

                entity.Property(e => e.SignAlgo).HasMaxLength(50);

                entity.Property(e => e.SignDate).HasColumnType("datetime");

                entity.Property(e => e.SignOwn).HasMaxLength(50);

                entity.Property(e => e.SignValue).HasMaxLength(500);

                entity.HasOne(d => d.PaymentdNavigation)
                    .WithMany(p => p.PaymentSignatures)
                    .HasForeignKey(d => d.Paymentd)
                    .HasConstraintName("FK_PaymentSignature_Payment");
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
