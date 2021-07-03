using BookShop.CRM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookShop.CRM.Core.Base
{
    public interface IPhotoRepository
    {
        Task<PhotoModel> Update(UpdatePhotoCommand command);

        Task<PhotoModel> Add(AddPhotoCommand command);

        Task<IList<PhotoModel>> GetAll();

        Task<PhotoModel> GetById(int id);

        Task Remove(int id);

        Task UploadFile(string path, int id);

        Task<IList<byte>> GetPhotoBytes(int id);
    }
}