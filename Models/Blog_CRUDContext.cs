using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MVC4.Models
{
    public partial class Blog_CRUDContext : DbContext
    {
        public Blog_CRUDContext()
        {
        }

        public Blog_CRUDContext(DbContextOptions<Blog_CRUDContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Blog> Blogs { get; set; } = null!;
        public virtual DbSet<CategoryBlog> CategoryBlogs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Database=Blog_CRUD;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>(entity =>
            {
                entity.ToTable("Blog");

                entity.Property(e => e.BlogId).HasColumnName("Blog_id");

                entity.Property(e => e.CategoryId).HasColumnName("Category_id");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Img)
                    .HasMaxLength(500)
                    .HasColumnName("img");

                entity.Property(e => e.Location).HasMaxLength(500);

                entity.Property(e => e.PublicDate)
                    .HasColumnType("date")
                    .HasColumnName("Public_date");

                entity.Property(e => e.ShortDescription).HasMaxLength(500);

                entity.Property(e => e.Title).HasMaxLength(500);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Blogs)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__Blog__Category_i__38996AB5");
            });

            modelBuilder.Entity<CategoryBlog>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("PK__Category__6DB28136056F33A4");

                entity.ToTable("Category_Blog");

                entity.Property(e => e.CategoryId).HasColumnName("Category_id");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(100)
                    .HasColumnName("Category_Name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
