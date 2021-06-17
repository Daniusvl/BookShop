using AutoMapper;
using BookShop.Core.Models;
using BookShop.Domain.Entities;

namespace BookShop.Core.AutoMapperProfiles
{
    class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<BookAuthor, BookAuthorModel>().ReverseMap();
        }
    }
}
