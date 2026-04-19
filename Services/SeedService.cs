using CraftCart.Models;

namespace CraftCart.Services;

public static class SeedService
{
    public static async Task SeedProductsIfEmpty()
    {
        var dbService = new FirebaseDbService();

        try
        {
            var existing = await dbService.GetProducts();
            if (existing.Count > 0)
                return;

            var sampleProducts = new List<Product>
            {
                new Product
                {
                    Name = "Ceramic Vase",
                    Description = "Hand-thrown ceramic vase with a unique glaze finish. Each piece is one of a kind. Perfect for fresh or dried flowers.",
                    Price = 45.00,
                    Category = "Pottery",
                    ImageUrl = "",
                    SellerId = "sample-seller",
                    SellerName = "ArtStudio",
                    AverageRating = 5,
                    ReviewCount = 24,
                    SalesCount = 12
                },
                new Product
                {
                    Name = "Beaded Necklace",
                    Description = "Hand-beaded necklace with natural stones. Adjustable length, makes a unique gift.",
                    Price = 28.00,
                    Category = "Jewelry",
                    ImageUrl = "",
                    SellerId = "sample-seller",
                    SellerName = "JewelCraft",
                    AverageRating = 4,
                    ReviewCount = 11,
                    SalesCount = 7
                },
                new Product
                {
                    Name = "Watercolor Print",
                    Description = "Original watercolor landscape print on premium archival paper. Signed by the artist.",
                    Price = 35.00,
                    Category = "Art",
                    ImageUrl = "",
                    SellerId = "sample-seller",
                    SellerName = "PaintHouse",
                    AverageRating = 5,
                    ReviewCount = 18,
                    SalesCount = 9
                },
                new Product
                {
                    Name = "Knitted Scarf",
                    Description = "Hand-knitted wool scarf, soft and warm. Available in earthy tones perfect for fall and winter.",
                    Price = 22.00,
                    Category = "Textiles",
                    ImageUrl = "",
                    SellerId = "sample-seller",
                    SellerName = "WoolWorks",
                    AverageRating = 4,
                    ReviewCount = 9,
                    SalesCount = 5
                },
                new Product
                {
                    Name = "Clay Teapot",
                    Description = "Traditional clay teapot, glazed and food safe. Holds 4 cups.",
                    Price = 60.00,
                    Category = "Pottery",
                    ImageUrl = "",
                    SellerId = "sample-seller",
                    SellerName = "ArtStudio",
                    AverageRating = 5,
                    ReviewCount = 14,
                    SalesCount = 8
                },
                new Product
                {
                    Name = "Mini Bottle Set",
                    Description = "Set of 3 hand-painted mini bottles. Perfect as decoration or for small flowers.",
                    Price = 30.00,
                    Category = "Pottery",
                    ImageUrl = "",
                    SellerId = "sample-seller",
                    SellerName = "ArtStudio",
                    AverageRating = 4,
                    ReviewCount = 6,
                    SalesCount = 5
                }
            };

            foreach (var p in sampleProducts)
            {
                await dbService.AddProduct(p);
            }
        }
        catch (Exception)
        {
        }
    }
}
