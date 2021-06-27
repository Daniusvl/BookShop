using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Abstract.Repositories.Base;
using BookShop.Domain.Entities;
using System.IO;

namespace BookShop.Books.Repositories
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly BooksDb ctx;
        public IAsyncRepository<Photo> BaseRepository { get; }

        public PhotoRepository(BooksDb ctx, IAsyncRepository<Photo> _base)
        {
            this.ctx = ctx;
            BaseRepository = _base;
        }

        public bool PhotoExists(string path)
        {
            return File.Exists(path);
        }
    }
}
