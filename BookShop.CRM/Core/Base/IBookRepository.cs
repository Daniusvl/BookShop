using BookShop.CRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookShop.CRM.Core.Base
{
    public interface IBookRepository
    {
        Task<BookModel> Add(AddBookCommand command);
        Task<IList<BookModel>> GetAll();
        Task<BookModel> GetById(int id);
        Task Remove(int id);
        Task<BookModel> Update(UpdateBookCommand command);
    }
}