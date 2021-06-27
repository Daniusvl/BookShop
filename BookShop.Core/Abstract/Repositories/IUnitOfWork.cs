using System;

namespace BookShop.Core.Abstract.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IAuthorRepository AuthorRepository { get; }

        ICategoryRepository CategoryRepository { get; }

        IBookRepository BookRepository { get; }

        IPhotoRepository PhotoRepository { get; }
    }
}
