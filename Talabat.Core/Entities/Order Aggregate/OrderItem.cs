using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
    public class OrderItem:BaseEntity
    {
        public OrderItem()
        {
            
        }
        public OrderItem(ProductItemOrdered products, decimal price, int quantity)
        {
            this.products = products;
            Price = price;
            Quantity = quantity;
        }

        public ProductItemOrdered products { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

    }
}
