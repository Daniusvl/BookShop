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

namespace BookShop.Books
{
    public class BooksDb : DbContext
    {
        private readonly IConfiguration configuration;
        private readonly ILoggedInUser logged_in_user;

        public DbSet<Book> Books { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Photo> Photos { get; set; }

        public BooksDb(DbContextOptions<BooksDb> options, IConfiguration configuration, ILoggedInUser logged_in_user) : base(options)
        {
            this.configuration = configuration;
            this.logged_in_user = logged_in_user;

            if (configuration.IsDevelopment())
            {
                Database.EnsureDeleted();
                Database.EnsureCreated();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (!configuration.IsDevelopment())
                return;

            Author bookAuthor = new Author { Id = 1, Name = "William Shakespeare" };
            modelBuilder.Entity<Author>()
                .HasData(bookAuthor);

            Category category = new Category { Id = 1, Name = "Literature" };
            modelBuilder.Entity<Category>()
                .HasData(category);

            modelBuilder.Entity<Book>()
                .HasData(new Book
                {
                    Id = 1,
                    Name = "Hamlet",
                    Description = "The Tragedy of Hamlet, Prince of Denmark, often shortened to Hamlet, is a tragedy written by William Shakespeare sometime between 1599 and 1601. It is Shakespeare's longest play, with 29,551 words.",
                    Price = 35,
                    FilePath = "TEST",
                    Hidden = false,
                    DateReleased = new DateTime(1603, 1, 1),
                    AuthorId = 1,
                    CategoryId = 1
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
