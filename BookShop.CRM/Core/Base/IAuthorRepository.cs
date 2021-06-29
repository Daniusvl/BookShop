using BookShop.CRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookShop.CRM.Core.Base
{
    public interface IAuthorRepository
    {
        Task<AuthorModel> Add(AddAuthorCommand command);

        Task<IList<AuthorModel>> GetAll();

        Task<AuthorModel> GetById(int id);

        Task Remove(int id);

        Task<AuthorModel> Update(UpdateAuthorCommand command);
    }
}