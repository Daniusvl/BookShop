using BookShop.CRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookShop.CRM.Core.Base
{
    public interface ICategoryRepository
    {
        Task<CategoryModel> Add(AddCategoryCommand command);

        Task<IList<CategoryModel>> GetAll();

        Task<CategoryModel> GetById(int id);

        Task Remove(int id);

        Task<CategoryModel> Update(UpdateCategoryCommand command);
    }
}