using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repository;
using Talabat.Core.Services;

namespace Talabat.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration,IBasketRepository basketRepository,IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId)
        {

            // Hold Secret Key
            StripeConfiguration.ApiKey = _configuration["StripeKeys:SecretKey"];

            //Get Basket
            var Basket = await _basketRepository.GetBasketAsync(basketId);
            if(Basket is null) return null;

            var shippingPrice = 0M;
            if(Basket.DeliveryMethodId.HasValue)
            {
                var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(Basket.DeliveryMethodId.Value);
                shippingPrice = DeliveryMethod.Cost;
            }
            //Total = SubTotal + DM.Cost
            if(Basket.Items.Count>0)
            {
                foreach (var item in Basket.Items)
                {
                    var Product = await _unitOfWork.Repository<Core.Entities.Product>().GetByIdAsync(item.Id);
                    if(item.Price != Product.Price) 
                        item.Price = Product.Price;
                }
            }

            var subTotal = Basket.Items.Sum(item => item.Price * item.Quantity);

            //Create Payment Intent
            var service = new PaymentIntentService();

            PaymentIntent paymentIntent;
            //Create
            if(string.IsNullOrEmpty(Basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)subTotal * 100 + (long)shippingPrice * 100,
                    PaymentMethodTypes = new List<string> { "Card" },
                    Currency = "USD",

                };

                paymentIntent = await service.CreateAsync(options);
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;
            
            }
            //Update
            else
            {
                var Options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)subTotal * 100 + (long)shippingPrice * 100,

                };
                paymentIntent = await service.UpdateAsync(Basket.PaymentIntentId , Options);
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;

                await _basketRepository.UpdateBasketAsync(Basket);

            }
                return Basket;

        }
    }
}
