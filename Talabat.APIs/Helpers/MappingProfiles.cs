using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                     .ForMember(d => d.ProductType, o => o.MapFrom(S => S.ProductType.Name))
                     .ForMember(d => d.ProductBrand, o => o.MapFrom(S => S.ProductBrand.Name))
                     .ForMember(d=>d.PictureUrl , o=>o.MapFrom<ProductPictureUrlResolver>());

            CreateMap<Address, AddressDto>().ReverseMap();

            CreateMap<CustomerBasketDto,CustomerBasket>().ReverseMap();
            CreateMap<BasketItemDto , BasketItem>().ReverseMap();


        }
    }
}
