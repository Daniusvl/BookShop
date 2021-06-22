using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Abstract.Repositories.Base;
using BookShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BookShop.Books.Repositories
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly BooksDb ctx;
        private readonly IAsyncRepository<Photo> repo;
        private readonly IAsyncLinqHelper<Photo> helper;

        public PhotoRepository(BooksDb ctx, IAsyncRepository<Photo> repo, IAsyncLinqHelper<Photo> helper)
        {
            this.ctx = ctx;
            this.repo = repo;
            this.helper = helper;
        }

        public async Task Create(Photo entity)
        {
            await repo.Create(entity);
        }

        public async Task Delete(Photo entity)
        {
            await repo.Delete(entity);
        }

        public async Task<IList<Photo>> GetAll()
        {
            return await repo.GetAll();
        }

        public async Task<Photo> GetById(int id)
        {
            return await repo.GetById(id);
        }

        public bool PhotoExists(string path)
        {
            return File.Exists(path);
        }

        public async Task Update(Photo entity)
        {
            await repo.Update(entity);
        }

        public async Task<IList<Photo>> Where(Func<Photo, bool> predicate)
        {
            return await helper.Where(predicate);
        }
    }
}
