using BookShop.Core.Abstract.Repositories.Base;
using BookShop.Domain.Entities;

namespace BookShop.Core.Abstract.Repositories
{
    public interface IPhotoRepository : IAsyncRepository<Photo>, IAsyncLinqHelper<Photo>
    {
        bool PhotoExists(string path);
    }
}
