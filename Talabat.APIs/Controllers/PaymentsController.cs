using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentsController : ApiBaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;

        public PaymentsController(IPaymentService paymentService, IMapper mapper)
        {
            _paymentService = paymentService;
            _mapper = mapper;
        }

        [ProducesResponseType(typeof(CustomerBasketDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
        {
        
            var BasketCustomer = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            if (BasketCustomer is null) return BadRequest(new ApiResponse(400, "There Is a Problem With Your Basket"));
            var mappedBasket = _mapper.Map<CustomerBasket , CustomerBasketDto>(BasketCustomer);
            return Ok(mappedBasket);
        
        }



    }
}
