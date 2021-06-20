using BookShop.Core.Configuration;
using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace BookShop.Repositories
{
    public class ACtx : DbContext
    {
        private readonly IConfiguration configuration;

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<BookAuthor> BookAuthors { get; set; }

        public DbSet<BookPhoto> BookPhotos { get; set; }

        public ACtx(DbContextOptions<ACtx> options, IConfiguration configuration) : base(options)
        {
            this.configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            if (!configuration.IsDevelopment())
                return;

            BookAuthor bookAuthor = new BookAuthor { Name = "William Shakespeare" };
            modelBuilder.Entity<BookAuthor>()
                .HasData(bookAuthor);

            Category category = new Category { Name = "Literature" };
            modelBuilder.Entity<Category>()
                .HasData(category);

            BookPhoto bookPhoto = new BookPhoto { FilePath = "TEST" };
            modelBuilder.Entity<BookPhoto>()
                .HasData(bookPhoto);

            modelBuilder.Entity<Product>()
                .HasData(new Product
                {
                    Name = "Hamlet",
                    Description = "The Tragedy of Hamlet, Prince of Denmark, often shortened to Hamlet, is a tragedy written by William Shakespeare sometime between 1599 and 1601. It is Shakespeare's longest play, with 29,551 words.",
                    Price = 35,
                    FilePath = "TEST",
                    Hidden = false,
                    DateReleased = new DateTime(1603, 0, 0),
                    Photos = new List<BookPhoto> { bookPhoto },
                    Author = bookAuthor,
                    Category = category
                });
        }
    }
}
