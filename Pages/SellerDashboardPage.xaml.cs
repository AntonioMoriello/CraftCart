using CraftCart.Services;
using CraftCart.Models;

namespace CraftCart.Pages;

public partial class SellerDashboardPage : ContentPage
{
    private readonly FirebaseDbService _dbService = new FirebaseDbService();

    public SellerDashboardPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadDashboard();
    }

    private async Task LoadDashboard()
    {
        try
        {
            string sellerId = await SecureStorage.GetAsync("user_id") ?? "";
            string sellerEmail = await SecureStorage.GetAsync("user_email") ?? "";

            var user = await _dbService.GetUserByEmail(sellerEmail);
            string displayName = user?.DisplayName;
            if (string.IsNullOrWhiteSpace(displayName))
                displayName = sellerEmail;

            WelcomeLabel.Text = $"Welcome back, {displayName}";

            var products = await _dbService.GetProductsBySeller(sellerId);

            foreach (var p in products)
            {
                p.SalesCount = await _dbService.GetSoldCountByProduct(p.Id);
                p.Revenue = await _dbService.GetRevenueByProduct(p.Id);
            }

            ProductsCollection.ItemsSource = products;
            EmptyLabel.IsVisible = products.Count == 0;

            double totalRevenue = await _dbService.GetRevenueBySeller(sellerId);
            int totalOrders = await _dbService.GetOrderCountBySeller(sellerId);
            double avgRating = await _dbService.GetAverageRatingBySeller(sellerId);

            RevenueLabel.Text = $"${totalRevenue:F0}";
            OrdersLabel.Text = totalOrders.ToString();
            ListingsLabel.Text = products.Count.ToString();
            RatingLabel.Text = avgRating.ToString("F1");
        }
        catch (Exception ex)
        {
            StatusLabel.Text = ex.Message;
        }
    }

    private async void OnAddProductClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddProductPage());
    }

    private async void OnProductSelected(object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Product selected)
        {
            ProductsCollection.SelectedItem = null;
            await Navigation.PushAsync(new EditProductPage(selected));
        }
    }

    private void OnDashboardTapped(object? sender, TappedEventArgs e)
    {
    }

    private async void OnMyProductsTapped(object? sender, TappedEventArgs e)
    {
        await LoadDashboard();
    }

    private async void OnOrdersTapped(object? sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new OrderManagementPage());
    }

    private async void OnAccountTapped(object? sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new MyAccountPage());
    }
}
