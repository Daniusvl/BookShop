using AutoMapper;
using BookShop.Core.Models;
using BookShop.Domain.Entities;
using System.IO;

namespace BookShop.Core
{
    class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Author, AuthorModel>().ReverseMap();
            CreateMap<Photo, PhotoModel>().ReverseMap();
            CreateMap<Category, CategoryModel>().ReverseMap();
            CreateMap<Book, BookModel>()
                .AfterMap((ent, model) => 
                {
                    model.AuthorName = ent.Author.Name;
                    model.CategoryName = ent.Category.Name;
                })
                .ReverseMap();
        }
    }
}
