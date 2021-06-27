﻿using BookShop.Core.Abstract.Repositories.Base;
using BookShop.Domain.Entities;
using System.Threading.Tasks;

namespace BookShop.Core.Abstract.Repositories
{
    public interface IAuthorRepository : IHasBaseRepository<Author>
    {
        Task<bool> IsUniqueName(string name);
    }
}
