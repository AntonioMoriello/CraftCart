using System;
using System.Collections.Generic;
using System.Text;

namespace CraftCart.Models
{
    public class Order
    {
        public string Id { get; set; } 
        public string BuyerId { get; set; }
        public string BuyerEmail { get; set; }
        public string SellerId { get; set; }
        public string Status { get; set; }
        public double Subtotal { get; set; }
        public double Tax { get; set; }
        public double Shipping { get; set; }
        public double Total { get; set; }
        public string OrderDate { get; set; }
    }
}
