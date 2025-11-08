using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_Aggregate;
using OrderAddress = Talabat.Core.Entities.Order_Aggregate.Address;
using IdentityAddress = Talabat.Core.Entities.Identity.Address;

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

            CreateMap<IdentityAddress, AddressDto>().ReverseMap();
            CreateMap<AddressDto, OrderAddress>();

            CreateMap<CustomerBasketDto,CustomerBasket>().ReverseMap();
            CreateMap<BasketItemDto , BasketItem>().ReverseMap();


        }
    }
}
