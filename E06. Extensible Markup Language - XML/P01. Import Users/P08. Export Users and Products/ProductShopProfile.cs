using AutoMapper;
using ProductShop.Models;
using ProductShop.ProductShop.DTO.Export;
using ProductShop.ProductShop.DTO.Import;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            //Import Mapping
            CreateMap<ImportUserDTO, User>();
            CreateMap<ImportProductDTO, Product>();
            CreateMap<ImportCategoryDTO, Category>();
            CreateMap<ImportCategoryProductDTO, CategoryProduct>();

            //Export Mapping
            CreateMap<User, ExportUserWithProductDTO>();
            CreateMap<Product, ExportBoughtProductDTO>();
            CreateMap<Category, ExportCategoriesByProductsCount>();
            CreateMap<ExportCategoriesByProductsCount, ExportCategoriesByProductsCount>();
        }
    }
}
