using System.IO;
using System.Threading.Tasks;

namespace BookShop.Core.Abstract.Features.FileUploader
{
    public abstract class BaseFileUploader
    {
        public abstract Task UploadFile(Stream stream, int id, int ContentLength);

        protected virtual async Task Save(Stream stream, int ContentLength, string path)
        {
            FileStream fileStream = new(path, FileMode.OpenOrCreate);
            int len = 0;
            while (len < ContentLength)
            {
                byte[] bytes = new byte[4096];
                int l = await stream.ReadAsync(bytes, 0, bytes.Length);
                fileStream.Write(bytes, 0, l);
                len += l;
            }
            fileStream.Close();
            await fileStream.DisposeAsync();
        }
    }
}
