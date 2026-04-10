using Firebase.Database;
using Firebase.Database.Query;
using CraftCart.Models;

namespace CraftCart.Services;

public class FirebaseDbService
{
    private readonly FirebaseClient _firebase = new FirebaseClient("https://craftcart-ba93f-default-rtdb.firebaseio.com/");

    public async Task AddUser(User user)
    {
        await _firebase
            .Child("Users")
            .PostAsync(user);
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        var users = await _firebase
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
            DisplayName = match.Object.DisplayName,
            FirstName = match.Object.FirstName,
            LastName = match.Object.LastName,
            Phone = match.Object.Phone,
            Address = match.Object.Address
        };
    }

    public async Task AddProduct(Product product)
    {
        await _firebase
            .Child("Products")
            .PostAsync(product);
    }

    public async Task<List<Product>> GetProducts()
    {
        var items = await _firebase
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
        await _firebase
            .Child("Products")
            .Child(p.Id)
            .PutAsync(p);
    }

    public async Task DeleteProduct(string id)
    {
        await _firebase
            .Child("Products")
            .Child(id)
            .DeleteAsync();
    }

    public async Task<List<Product>> SearchProducts(string query)
    {
        var products = await GetProducts();
        return products.Where(p => p.Name.ToLower().Contains(query.ToLower())).ToList();
    }

    public async Task AddOrder(Order order)
    {
        await _firebase
            .Child("Orders")
            .PostAsync(order);
    }

    public async Task<string> AddOrderGetKey(Order order)
    {
        var result = await _firebase
            .Child("Orders")
            .PostAsync(order);
        return result.Key;
    }

    public async Task AddOrderItem(OrderItem item)
    {
        await _firebase
            .Child("OrderItems")
            .PostAsync(item);
    }

    public async Task<List<Order>> GetOrdersByBuyer(string buyerId)
    {
        var items = await _firebase
            .Child("Orders")
            .OnceAsync<Order>();

        return items.Where(i => i.Object.BuyerId == buyerId)
            .Select(item => new Order
            {
                Id = item.Key,
                BuyerId = item.Object.BuyerId,
                BuyerEmail = item.Object.BuyerEmail,
                SellerId = item.Object.SellerId,
                Status = item.Object.Status,
                Subtotal = item.Object.Subtotal,
                Tax = item.Object.Tax,
                Shipping = item.Object.Shipping,
                Total = item.Object.Total,
                OrderDate = item.Object.OrderDate
            }).ToList();
    }

    public async Task AddReview(Review review)
    {
        await _firebase
            .Child("Reviews")
            .PostAsync(review);
    }

    public async Task<List<Review>> GetReviewsByProduct(string productId)
    {
        var items = await _firebase
            .Child("Reviews")
            .OnceAsync<Review>();

        return items.Where(i => i.Object.ProductId == productId)
            .Select(item => new Review
            {
                Id = item.Key,
                ProductId = item.Object.ProductId,
                BuyerId = item.Object.BuyerId,
                BuyerEmail = item.Object.BuyerEmail,
                Rating = item.Object.Rating,
                Comment = item.Object.Comment,
                Date = item.Object.Date
            }).ToList();
    }

    public async Task UpdateUser(User user)
    {
        await _firebase
            .Child("Users")
            .Child(user.Id)
            .PutAsync(user);
    }

    public async Task<int> GetOrderCountByBuyer(string buyerId)
    {
        var orders = await GetOrdersByBuyer(buyerId);
        return orders.Count;
    }

    public async Task<double> GetTotalSpentByBuyer(string buyerId)
    {
        var orders = await GetOrdersByBuyer(buyerId);
        return orders.Sum(o => o.Total);
    }

    public async Task<int> GetReviewCountByBuyer(string buyerId)
    {
        var items = await _firebase
            .Child("Reviews")
            .OnceAsync<Review>();

        return items.Where(i => i.Object.BuyerId == buyerId).Count();
    }
}
