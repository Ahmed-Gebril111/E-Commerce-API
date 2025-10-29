using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repository;
using Talabat.Core.Services;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
        private readonly IGenericRepository<Order> _orderRepo;

        public OrderService(IBasketRepository basketRepository , IGenericRepository<Product> ProductRepo , IGenericRepository<DeliveryMethod> DeliveryMethodRepo , IGenericRepository<Order> OrderRepo)
        {
            _basketRepository = basketRepository;
            _productRepo = ProductRepo;
            _deliveryMethodRepo = DeliveryMethodRepo;
            _orderRepo = OrderRepo;
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
                    var Product = await _productRepo.GetByIdAsync(item.Id);
                    var ProductItemOrdered = new ProductItemOrdered(Product.Id, Product.Name, Product.PictureUrl);
                    var OrderItem = new OrderItem(ProductItemOrdered, item.Quantity, (int)Product.Price);
                    OrderItems.Add(OrderItem);
                }
            }

            //3.Calculate SubTotal

            var SubTotal = OrderItems.Sum(item=>item.Price * item.Quantity);

            //4.Get Deivery Method from DeliveryMethod Repo

            var DeliveryMethod = await _deliveryMethodRepo.GetByIdAsync(DeliveryMethodId);

            //5.Create Order 

            var Order = new Order(buyerEmail, ShippingAddress, DeliveryMethod, OrderItems, SubTotal);

            //6. Add Order Locally 

            await _orderRepo.Add(Order);

            //7. Save Order To DataBase(ToDto)

            return Order;




        }

        public Task<IReadOnlyList<Order>> GetOrderForSpecificUserAsync(string buyerEmail, int OrderId)
        {
            throw new NotImplementedException();
        }
    }
}
