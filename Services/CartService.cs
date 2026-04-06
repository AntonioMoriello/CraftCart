using CraftCart.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CraftCart.Services
{
    public static class CartService
    {
        static List<CartItem> _cartItems = new List<CartItem>();

        public static void AddToCart(CartItem item)
        {
            var existing = _cartItems.FirstOrDefault(c => c.ProductId == item.ProductId);
            if (existing != null)
            {
                existing.Quantity += item.Quantity;
            }
            else
            {
                _cartItems.Add(item);
            }
        }

        public static void RemoveFromCart(string productId)
        {
            var item = _cartItems.FirstOrDefault(c => c.ProductId == productId);
            if (item != null)
            {
                _cartItems.Remove(item);
            }
        }

        public static void UpdateQuantity(string productId, int quantity)
        {
            var item = _cartItems.FirstOrDefault(c => c.ProductId == productId);
            if (item != null)
            {
                item.Quantity = quantity;
            }
        }

        public static List<CartItem> GetCartItems()
        {
            return _cartItems;
        }

        public static double GetSubtotal()
        {
            return _cartItems.Sum(c => c.Price * c.Quantity);
        }

        public static double GetTax()
        {
            return GetSubtotal() * 0.05;
        }

        public static double GetShipping()
        {
            if (_cartItems.Count == 0)
                return 0;
            return 5.00;
        }

        public static double GetTotal()
        {
            return GetSubtotal() + GetTax() + GetShipping();
        }

        public static void ClearCart()
        {
            _cartItems.Clear();
        }
    }
}
