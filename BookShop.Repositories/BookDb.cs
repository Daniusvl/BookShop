using BookShop.Core.Abstract;
using BookShop.Core;
using BookShop.Domain.Entities;
using BookShop.Domain.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookShop.Repositories
{
    public class BookDb : DbContext
    {
        private readonly IConfiguration configuration;
        private readonly ILoggedInUser logged_in_user;

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<BookAuthor> BookAuthors { get; set; }

        public DbSet<BookPhoto> BookPhotos { get; set; }

        public BookDb(DbContextOptions<BookDb> options, IConfiguration configuration, ILoggedInUser logged_in_user) : base(options)
        {
            this.configuration = configuration;
            this.logged_in_user = logged_in_user;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            if (!configuration.IsDevelopment())
                return;

            BookAuthor bookAuthor = new BookAuthor { Id = 1, Name = "William Shakespeare" };
            modelBuilder.Entity<BookAuthor>()
                .HasData(bookAuthor);

            Category category = new Category { Id = 1, Name = "Literature" };
            modelBuilder.Entity<Category>()
                .HasData(category);

            BookPhoto bookPhoto = new BookPhoto { Id = 1, FilePath = "TEST" };
            modelBuilder.Entity<BookPhoto>()
                .HasData(bookPhoto);

            modelBuilder.Entity<Product>()
                .HasData(new Product
                {
                    Id = 1,
                    Name = "Hamlet",
                    Description = "The Tragedy of Hamlet, Prince of Denmark, often shortened to Hamlet, is a tragedy written by William Shakespeare sometime between 1599 and 1601. It is Shakespeare's longest play, with 29,551 words.",
                    Price = 35,
                    FilePath = "TEST",
                    Hidden = false,
                    DateReleased = new DateTime(1603, 1, 1),
                    Photos = new List<BookPhoto> { bookPhoto },
                    Author = bookAuthor,
                    Category = category
                });

        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<EntityEntry> entries = ChangeTracker.Entries();

            foreach (EntityEntry entry  in entries)
            {
                if(entry.State == EntityState.Added)
                {
                    ((BaseEntity)entry.Entity).CreatedBy = logged_in_user.UserId;
                    ((BaseEntity)entry.Entity).DateCreated = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    ((BaseEntity)entry.Entity).LastModifiedBy = logged_in_user.UserId;
                    ((BaseEntity)entry.Entity).DateLastModified = DateTime.Now;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
