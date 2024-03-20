using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace NotificationService.API.Models
{
    public partial class NotificationContext : DbContext
    {
        public NotificationContext()
        {
        }

        public NotificationContext(DbContextOptions<NotificationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<NotificationRecipient> NotificationRecipients { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("server=localhost\\sqlserverdb,1435;database=Notification;uid=sa;pwd=PassW0rd!;TrustServerCertificate=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notification");

                entity.Property(e => e.CourseId).HasColumnName("Course_Id");

                entity.Property(e => e.NotificationContent).HasColumnName("Notification_Content");

                entity.Property(e => e.RecipientId).HasColumnName("Recipient_Id");

                entity.Property(e => e.SendDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Send_Date");
            });

            modelBuilder.Entity<NotificationRecipient>(entity =>
            {
                entity.ToTable("NotificationRecipient");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.NotificationId).HasColumnName("Notification_Id");

                entity.Property(e => e.RecipientId).HasColumnName("Recipient_Id");

                entity.HasOne(d => d.Notification)
                    .WithMany(p => p.NotificationRecipients)
                    .HasForeignKey(d => d.NotificationId)
                    .HasConstraintName("FK_NotificationRecipient_Notification");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
