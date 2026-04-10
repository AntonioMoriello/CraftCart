using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

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

        [JsonIgnore]
        public string StatusColor => Status switch
        {
            "Processing" => "#E65100",
            "Shipped" => "#1565C0",
            "Delivered" => "#2E7D32",
            _ => "#333333"
        };

        [JsonIgnore]
        public string StatusBackground => Status switch
        {
            "Processing" => "#FFF3E0",
            "Shipped" => "#E3F2FD",
            "Delivered" => "#E8F5E9",
            _ => "#F5F5F5"
        };
    }
}
