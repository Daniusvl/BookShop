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
            CreateMap<BookAuthor, BookAuthorModel>().ReverseMap();
            CreateMap<BookPhoto, BookPhotoModel>()
                .AfterMap((ent, model) => 
                {
                    model.FileBytes = File.ReadAllBytes(model.FilePath);
                })
                .ReverseMap();
            CreateMap<Category, CategoryModel>().ReverseMap();
            CreateMap<Product, ProductModel>()
                .AfterMap((ent, model) => 
                {
                    model.AuthorName = ent.Author.Name;
                    model.CategoryName = ent.Category.Name;
                })
                .ReverseMap();
        }
    }
}
