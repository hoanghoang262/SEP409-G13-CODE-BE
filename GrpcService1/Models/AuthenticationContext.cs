using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace UserGrpc.Models
{
    public partial class AuthenticationContext : DbContext
    {
        public AuthenticationContext()
        {
        }

        public AuthenticationContext(DbContextOptions<AuthenticationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:fptulearnserver.database.windows.net,1433;Initial Catalog=Authentication;Persist Security Info=False;User ID=fptu;Password=24082002aA;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.FullName)
                    .HasMaxLength(50)
                    .HasColumnName("Full_Name");

                entity.Property(e => e.ProfilePict).HasColumnName("Profile_Pict");

                entity.Property(e => e.RoleId).HasColumnName("Role_Id");

                entity.Property(e => e.UserName).HasMaxLength(50);

                entity.Property(e => e.VerificationCode)
                    .HasMaxLength(50)
                    .HasColumnName("Verification_Code");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_User_Role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
