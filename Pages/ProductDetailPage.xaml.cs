using CraftCart.Services;
using CraftCart.Models;

namespace CraftCart.Pages;

public partial class ProductDetailPage : ContentPage
{
    private readonly FirebaseDbService _dbService = new FirebaseDbService();
    private readonly Product _product;

    public ProductDetailPage(Product product)
    {
        InitializeComponent();
        _product = product;
        DisplayProduct();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadReviews();
    }

    private void DisplayProduct()
    {
        ProductNameLabel.Text = _product.Name;
        SellerNameLabel.Text = $"by {_product.SellerName}";
        ProductPriceLabel.Text = $"${_product.Price:F2}";
        ProductDescriptionLabel.Text = _product.Description;

        if (_product.HasImage)
        {
            ProductImage.Source = ImageSource.FromFile(_product.ImageUrl);
            ProductImage.IsVisible = true;
            ImagePlaceholder.IsVisible = false;
        }

        int stars = (int)Math.Round(_product.AverageRating);
        string starText = string.Empty;
        for (int i = 0; i < stars; i++)
            starText += "\u2B50";
        RatingStarsLabel.Text = starText;
        ReviewCountLabel.Text = $"({_product.ReviewCount} reviews)";

        if (!string.IsNullOrWhiteSpace(_product.Category))
        {
            var tag = new Border
            {
                BackgroundColor = Color.FromArgb("#F0F0F0"),
                StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 12 },
                Padding = new Thickness(12, 5),
                Stroke = Colors.Transparent,
                Content = new Label { Text = _product.Category, FontSize = 12, TextColor = Color.FromArgb("#555") }
            };
            TagsLayout.Children.Add(tag);
        }
    }

    private async Task LoadReviews()
    {
        try
        {
            var reviews = await _dbService.GetReviewsByProduct(_product.Id);
            ReviewsCollection.ItemsSource = reviews;
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    private async void OnAddToCartClicked(object? sender, EventArgs e)
    {
        CartItem item = new CartItem
        {
            ProductId = _product.Id,
            ProductName = _product.Name,
            Price = _product.Price,
            Quantity = 1,
            ImageUrl = _product.ImageUrl
        };

        CartService.AddToCart(item);
        await DisplayAlertAsync("Added", $"{_product.Name} added to cart", "OK");
    }
}
