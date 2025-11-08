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
using Talabat.Core.Specifications.Order_Spec;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

       
        public OrderService(IBasketRepository basketRepository ,IUnitOfWork unitOfWork)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int DeliveryMethodId, Address ShippingAddress)
        {
            //1.Get Basket From Basket Repo
            var Basket = await _basketRepository.GetBasketAsync(basketId);

            //2.Get Selected Item At Basket From Product Repo
            var OrderItems = new List<OrderItem>();
            if (Basket?.Items.Count > 0)
            {
                foreach(var item in Basket.Items)
                {
                    var Product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var ProductItemOrdered = new ProductItemOrdered(Product.Id, Product.Name, Product.PictureUrl);
                    var OrderItem = new OrderItem(ProductItemOrdered, item.Quantity, (int)Product.Price);
                    OrderItems.Add(OrderItem);
                }
            }

            //3.Calculate SubTotal

            var SubTotal = OrderItems.Sum(item=>item.Price * item.Quantity);

            //4.Get Deivery Method from DeliveryMethod Repo

            var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);

            //5.Create Order 

            var Order = new Order(buyerEmail, ShippingAddress, DeliveryMethod, OrderItems, SubTotal);

            //6. Add Order Locally 

            await _unitOfWork.Repository<Order>().Add(Order);

            //7. Save Order To DataBase(ToDto)
            var Result = await _unitOfWork.CompleteAsync();
            if (Result <= 0) return null;
            return Order;




        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
        {
            var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return DeliveryMethod;
        }

        public async Task<Order> GetOrderByIdForSpecificUserAsync(string buyerEmail, int OrderId)
        {
            var Spec = new OrderSpecifications(buyerEmail, OrderId);
            var Order = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(Spec);
            return Order;
        }

        public async Task<IReadOnlyList<Order>> GetOrderForSpecificUserAsync(string buyerEmail)
        {
            var Spec = new OrderSpecifications(buyerEmail);
            var Orders  = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(Spec);
            return Orders;
        }
    }
}
