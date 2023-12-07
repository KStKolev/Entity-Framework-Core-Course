using AutoMapper;
using ProductShop.Models;
using ProductShop.ProductShop.DTO.Import;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<ImportUserDTO, User>();
            CreateMap<ImportProductDTO, Product>();
        }
    }
}
