using System;
using System.Collections.Generic;
using System.Text;

namespace CraftCart.Models
{
    public class Product
    {
        public string Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public string SellerId { get; set; }
        public string SellerName { get; set; }
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public int SalesCount { get; set; }
    }
}
