using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

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

        [JsonIgnore]
        public double Revenue { get; set; }

        [JsonIgnore]
        public string RevenueText => $"${Revenue:F0}";

        [JsonIgnore]
        public bool HasImage => !string.IsNullOrEmpty(ImageUrl) && File.Exists(ImageUrl);

        [JsonIgnore]
        public ImageSource Image => HasImage ? ImageSource.FromFile(ImageUrl) : null;
    }
}
