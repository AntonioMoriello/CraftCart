using Firebase.Database;
using Firebase.Database.Query;
using CraftCart.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CraftCart.Services
{
    public class FirebaseDbService
    {
        FirebaseClient firebase = new FirebaseClient("https://craftcart-ba93f-default-rtdb.firebaseio.com/");

        public async Task AddUser(User user)
        {
            await firebase
                .Child("Users")
                .PostAsync(user);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var users = await firebase
                .Child("Users")
                .OnceAsync<User>();

            var match = users
                .Where(u => u.Object.Email == email)
                .FirstOrDefault();

            if (match == null)
                return null;

            return new User
            {
                Id = match.Key,
                Email = match.Object.Email,
                Role = match.Object.Role,
                DisplayName = match.Object.DisplayName
            };
        }

        public async Task AddProduct(Product product)
        {
            await firebase
                .Child("Products")
                .PostAsync(product);
        }

        public async Task<List<Product>> GetProducts()
        {
            var items = await firebase
                .Child("Products")
                .OnceAsync<Product>();

            return items.Select(item => new Product
            {
                Id = item.Key,
                Name = item.Object.Name,
                Description = item.Object.Description,
                Price = item.Object.Price,
                Category = item.Object.Category,
                ImageUrl = item.Object.ImageUrl,
                SellerId = item.Object.SellerId,
                SellerName = item.Object.SellerName,
                AverageRating = item.Object.AverageRating,
                ReviewCount = item.Object.ReviewCount,
                SalesCount = item.Object.SalesCount
            }).ToList();
        }

        public async Task<List<Product>> GetProductsByCategory(string category)
        {
            var products = await GetProducts();
            return products.Where(p => p.Category == category).ToList();
        }

        public async Task<List<Product>> GetProductsBySeller(string sellerId)
        {
            var products = await GetProducts();
            return products.Where(p => p.SellerId == sellerId).ToList();
        }

        public async Task UpdateProduct(Product p)
        {
            await firebase
                .Child("Products")
                .Child(p.Id)
                .PutAsync(p);
        }

        public async Task DeleteProduct(string id)
        {
            await firebase
                .Child("Products")
                .Child(id)
                .DeleteAsync();
        }

        public async Task<List<Product>> SearchProducts(string query)
        {
            var products = await GetProducts();
            return products.Where(p => p.Name.ToLower().Contains(query.ToLower())).ToList();
        }
    }
}
