using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CraftCart.Models
{
    public class CartItem
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }

        public bool HasImage => !string.IsNullOrEmpty(ImageUrl) && File.Exists(ImageUrl);

        public ImageSource Image => HasImage ? ImageSource.FromFile(ImageUrl) : null;
    }
}
