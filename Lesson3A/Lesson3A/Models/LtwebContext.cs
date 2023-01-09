using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Lesson3A.Models;

public partial class LtwebContext : DbContext
{
    public LtwebContext()
    {
    }

    public LtwebContext(DbContextOptions<LtwebContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Province> Provinces { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server =LAPTOP-ARHAGM1K;Database = LTWeb;User id=vinh;password =123;Encrypt=false ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CourseName).HasMaxLength(250);
            entity.Property(e => e.Group)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Major).HasMaxLength(150);
            entity.Property(e => e.Note).HasMaxLength(500);
            entity.Property(e => e.SubCode)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Area).HasMaxLength(255);
            entity.Property(e => e.HeadCity).HasMaxLength(255);
            entity.Property(e => e.Population).HasMaxLength(255);
            entity.Property(e => e.Province1)
                .HasMaxLength(255)
                .HasColumnName("Province");
            entity.Property(e => e.Section).HasMaxLength(255);
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.ClOs).HasColumnType("text");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Details).HasColumnType("text");
            entity.Property(e => e.Parrellel).HasMaxLength(150);
            entity.Property(e => e.Prerequisite).HasMaxLength(150);
            entity.Property(e => e.Program).HasMaxLength(50);
            entity.Property(e => e.References).HasColumnType("text");
            entity.Property(e => e.SubCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.SubName).HasMaxLength(150);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.City).HasMaxLength(150);
            entity.Property(e => e.Fullname).HasMaxLength(50);
            entity.Property(e => e.Lastlogin).HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.State).HasMaxLength(150);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Zip)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
