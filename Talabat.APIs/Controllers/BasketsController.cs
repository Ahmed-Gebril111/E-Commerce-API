using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repository;

namespace Talabat.APIs.Controllers
{
    
    public class BasketsController : ApiBaseController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketsController(IBasketRepository basketRepository , IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        //Get Or Recreate Basket

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(string Basketid)
        {
            var Basket = await _basketRepository.GetBasketAsync(Basketid);
            //if(Basket is null)
            //    return new CustomerBasket(id);
            //else 
            //    return Basket;
            return Basket is null ? new CustomerBasket(Basketid) : Basket;
        }


        //Create Or Update

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            var MappedBasket = _mapper.Map<CustomerBasketDto,CustomerBasket>(basket);
            var CreatedOrUpdatedBasket = await _basketRepository.UpdateBasketAsync(MappedBasket);
            if (CreatedOrUpdatedBasket is null)
                return BadRequest(new ApiResponse(400));
            return Ok(CreatedOrUpdatedBasket);
        }


        //Delete Basket

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string id)
        {
            return await _basketRepository.DeleteBasketAsync(id);   
        }



    }
}
