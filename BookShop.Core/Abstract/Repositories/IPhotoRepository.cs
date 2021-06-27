using BookShop.Core.Abstract.Repositories.Base;
using BookShop.Domain.Entities;

namespace BookShop.Core.Abstract.Repositories
{
    public interface IPhotoRepository : IHasBaseRepository<Photo>
    {
        bool PhotoExists(string path);
    }
}
