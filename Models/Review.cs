using System;
using System.Collections.Generic;
using System.Text;

namespace CraftCart.Models
{
    public class Review
    {
        public string Id { get; set; } 
        public string ProductId { get; set; }
        public string BuyerId { get; set; }
        public string BuyerEmail { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string Date { get; set; }
    }
}
