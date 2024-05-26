using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Center.Models;

public partial class Ispr2336AvetisyanSkCenterContext : DbContext
{
    public Ispr2336AvetisyanSkCenterContext()
    {
    }

    public Ispr2336AvetisyanSkCenterContext(DbContextOptions<Ispr2336AvetisyanSkCenterContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<IssuingMagazine> IssuingMagazines { get; set; }

    public virtual DbSet<JobTitle> JobTitles { get; set; }

    public virtual DbSet<Magazin> Magazins { get; set; }

    public virtual DbSet<ReceivingMagazine> ReceivingMagazines { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Worker> Workers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=cfif31.ru;database=ISPr23-36_AvetisyanSK_Center;uid=ISPr23-36_AvetisyanSK;pwd=ISPr23-36_AvetisyanSK", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.36-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Department");

            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<IssuingMagazine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.MagazinId, "FK_IssuingMagazines_MagazinId_idx");

            entity.HasIndex(e => e.WorkerId, "FK_IssuingMagazines_WorkerId_idx");

            entity.HasOne(d => d.Magazin).WithMany(p => p.IssuingMagazines)
                .HasForeignKey(d => d.MagazinId)
                .HasConstraintName("FK_IssuingMagazines_MagazinId");

            entity.HasOne(d => d.Worker).WithMany(p => p.IssuingMagazines)
                .HasForeignKey(d => d.WorkerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IssuingMagazines_WorkerId");
        });

        modelBuilder.Entity<JobTitle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("JobTitle");

            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Magazin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Magazin");

            entity.HasIndex(e => e.CreatorId, "FK_Magazin_WorkerId_idx");

            entity.Property(e => e.Price).HasPrecision(10, 2);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Creator).WithMany(p => p.Magazins)
                .HasForeignKey(d => d.CreatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Magazin_WorkerId");
        });

        modelBuilder.Entity<ReceivingMagazine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.MagazinId, "FK_ReceivingMagazines_MagazinId_idx");

            entity.HasIndex(e => e.WorkerId, "FK_ReceivingMagazines_WorkerId_idx");

            entity.HasOne(d => d.Magazin).WithMany(p => p.ReceivingMagazines)
                .HasForeignKey(d => d.MagazinId)
                .HasConstraintName("FK_ReceivingMagazines_MagazinId");

            entity.HasOne(d => d.Worker).WithMany(p => p.ReceivingMagazines)
                .HasForeignKey(d => d.WorkerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReceivingMagazines_WorkerId");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Role");

            entity.Property(e => e.Name).HasMaxLength(45);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("User");

            entity.HasIndex(e => e.RoleId, "FK_User_RoleId_idx");

            entity.Property(e => e.Login).HasMaxLength(45);
            entity.Property(e => e.Password).HasMaxLength(45);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_RoleId");
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Worker");

            entity.HasIndex(e => e.DepartmentId, "FK_Worker_DepartmentId_idx");

            entity.HasIndex(e => e.JobTitleId, "FK_Worker_JobTitleId_idx");

            entity.HasIndex(e => e.UserId, "FK_Worker_UserId_idx");

            entity.Property(e => e.FullName).HasMaxLength(255);

            entity.HasOne(d => d.Department).WithMany(p => p.Workers)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Worker_DepartmentId");

            entity.HasOne(d => d.JobTitle).WithMany(p => p.Workers)
                .HasForeignKey(d => d.JobTitleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Worker_JobTitleId");

            entity.HasOne(d => d.User).WithMany(p => p.Workers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Worker_UserId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
