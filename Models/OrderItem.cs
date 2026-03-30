using System;
using System.Collections.Generic;
using System.Text;

namespace CraftCart.Models
{
    public class OrderItem
    {
        public string Id { get; set; } 
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
