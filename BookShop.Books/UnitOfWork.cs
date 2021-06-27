using BookShop.Books.Repositories;
using BookShop.Core.Abstract.Repositories;
using BookShop.Domain.Entities;
using System;

namespace BookShop.Books
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BooksDb ctx;

        private IAuthorRepository authorRepository;

        private ICategoryRepository categoryRepository;

        private IBookRepository bookRepository;

        private IPhotoRepository photoRepository;

        public UnitOfWork(BooksDb ctx)
        {
            this.ctx = ctx;
        }

        public IAuthorRepository AuthorRepository => authorRepository ??= new AuthorRepository(ctx, new AsyncRepository<Author>(ctx));

        public ICategoryRepository CategoryRepository => categoryRepository ??= new CategoryRepository(ctx, new AsyncRepository<Category>(ctx));

        public IBookRepository BookRepository => bookRepository ??= new BookRepository(ctx, new AsyncRepository<Book>(ctx));

        public IPhotoRepository PhotoRepository => photoRepository ??= new PhotoRepository(ctx, new AsyncRepository<Photo>(ctx));

        public void Dispose()
        {
            ctx.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
