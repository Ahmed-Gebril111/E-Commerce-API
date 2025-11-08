using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ApiBaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService , IMapper mapper )
        {
            _orderService = orderService;
            _mapper = mapper;
        }


        [ProducesResponseType(typeof(Order),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var BuyerEmail =  User.FindFirstValue(ClaimTypes.Email);
            var MappedAddress = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);
            var Order = await _orderService.CreateOrderAsync(BuyerEmail, orderDto.basketId,orderDto.DeliveryMethodId,MappedAddress);
            if (Order is null) return BadRequest(new ApiResponse(400, "There Is A Problem With Your Order "));

            return Ok(Order);
        }

    }
}
