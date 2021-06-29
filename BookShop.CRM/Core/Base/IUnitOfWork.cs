﻿namespace BookShop.CRM.Core.Base
{
    public interface IUnitOfWork
    {
        IBookRepository BookRepository { get; }

        ICategoryRepository CategoryRepository { get; }

        IAuthorRepository AuthorRepository { get; }

        IPhotoRepository PhotoRepository { get; }
    }
}
