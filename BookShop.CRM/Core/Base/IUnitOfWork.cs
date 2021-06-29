using System;

namespace BookShop.CRM.Core.Base
{
    public interface IUnitOfWork : IDisposable
    {
        IBookRepository BookRepository { get; }

        ICategoryRepository CategoryRepository { get; }

        IAuthorRepository AuthorRepository { get; }

        IPhotoRepository PhotoRepository { get; }
    }
}
