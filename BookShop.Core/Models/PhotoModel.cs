using System.Collections.Generic;

namespace BookShop.Core.Models
{
    public class PhotoModel
    {
        public int Id { get; set; }

        public string FilePath { get; set; }

        public IList<byte> FileBytes { get; set; }

        public override string ToString()
        {
            return FilePath;
        }
    }
}
