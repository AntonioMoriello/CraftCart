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
        public string ItemsText { get; set; }

        [JsonIgnore]
        public string ShortId
        {
            get
            {
                if (string.IsNullOrEmpty(Id))
                    return "CC-0000";

                string clean = Id.Replace("-", "");
                int len = Math.Min(6, clean.Length);
                return "CC-" + clean.Substring(clean.Length - len).ToUpper();
            }
        }

        [JsonIgnore]
        public string StatusColor => Status switch
        {
            "Processing" => "#E65100",
            "Shipped" => "#1565C0",
            "Delivered" => "#2E7D32",
            "Pending" => "#E65100",
            "Declined" => "#C62828",
            _ => "#333333"
        };

        [JsonIgnore]
        public string StatusBackground => Status switch
        {
            "Processing" => "#FFF3E0",
            "Shipped" => "#E3F2FD",
            "Delivered" => "#E8F5E9",
            "Pending" => "#FFF3E0",
            "Declined" => "#FFEBEE",
            _ => "#F5F5F5"
        };
    }
}
