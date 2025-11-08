using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string buyerEmail, String basketId, int DeliveryMethodId, Address ShippingAddress);
        Task<Order> GetOrderByIdForSpecificUserAsync(string buyerEmail, int OrderId);
        Task<IReadOnlyList<Order>> GetOrderForSpecificUserAsync(string buyerEmail);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync();


    }
}
