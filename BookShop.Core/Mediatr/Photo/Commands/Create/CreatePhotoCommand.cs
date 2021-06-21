using BookShop.Core.Models;
using MediatR;
using System.Collections.Generic;

namespace BookShop.Core.Mediatr.Photo.Commands.Create
{
    public class CreatePhotoCommand : IRequest<PhotoModel>
    {
        public string ProductName { get; set; }

        public IList<byte> FileBytes { get; set; }

    }
}
