using BookShop.Core.Abstract.Repositories;
using BookShop.Core.Abstract.Repositories.Base;
using BookShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BookShop.Repositories.Repositories
{
    public class BookPhotoRepository : IBookPhotoRepository
    {
        private readonly ACtx ctx;
        private readonly IAsyncRepository<BookPhoto> repo;
        private readonly IAsyncLinqHelper<BookPhoto> helper;

        public BookPhotoRepository(ACtx ctx, IAsyncRepository<BookPhoto> repo, IAsyncLinqHelper<BookPhoto> helper)
        {
            this.ctx = ctx;
            this.repo = repo;
            this.helper = helper;
        }

        public async Task Create(BookPhoto entity)
        {
            await repo.Create(entity);
        }

        public async Task Delete(BookPhoto entity)
        {
            await repo.Delete(entity);
        }

        public async Task<IList<BookPhoto>> GetAll()
        {
            return await repo.GetAll();
        }

        public async Task<BookPhoto> GetById(int id)
        {
            return await repo.GetById(id);
        }

        public bool PhotoExists(string path)
        {
            return File.Exists(path);
        }

        public async Task Update(BookPhoto entity)
        {
            await repo.Update(entity);
        }

        public async Task<IList<BookPhoto>> Where(Func<BookPhoto, bool> predicate)
        {
            return await helper.Where(predicate);
        }
    }
}
