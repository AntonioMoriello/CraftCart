using CraftCart.Services;
using CraftCart.Models;

namespace CraftCart.Pages;

public partial class MyOrdersPage : ContentPage
{
    private readonly FirebaseDbService _dbService = new FirebaseDbService();
    private List<Order> _allOrders = new List<Order>();
    private string _currentFilter = "All";

    public MyOrdersPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadOrders();
    }

    private async Task LoadOrders()
    {
        try
        {
            string buyerId = await SecureStorage.GetAsync("user_id") ?? "";
            if (string.IsNullOrEmpty(buyerId))
            {
                EmptyLabel.IsVisible = true;
                EmptyLabel.Text = "Please sign in to view your orders";
                return;
            }

            _allOrders = await _dbService.GetOrdersByBuyer(buyerId);
            FilterAndDisplay();
        }
        catch (Exception ex)
        {
            StatusLabel.Text = ex.Message;
        }
    }

    private void FilterAndDisplay()
    {
        var filtered = _allOrders;

        if (_currentFilter != "All")
        {
            filtered = filtered.Where(o => o.Status == _currentFilter).ToList();
        }

        OrdersCollection.ItemsSource = filtered;
        EmptyLabel.IsVisible = filtered.Count == 0;
    }

    private void SetFilter(string filter)
    {
        _currentFilter = filter;
        FilterAndDisplay();
    }

    private void OnFilterAll(object? sender, TappedEventArgs e) => SetFilter("All");
    private void OnFilterProcessing(object? sender, TappedEventArgs e) => SetFilter("Processing");
    private void OnFilterShipped(object? sender, TappedEventArgs e) => SetFilter("Shipped");
    private void OnFilterDelivered(object? sender, TappedEventArgs e) => SetFilter("Delivered");

    private async void OnBrowseTapped(object? sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new BrowseProductsPage());
    }

    private async void OnCartTapped(object? sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new ShoppingCartPage());
    }

    private void OnOrdersTapped(object? sender, TappedEventArgs e)
    {
    }

    private async void OnAccountTapped(object? sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new MyAccountPage());
    }
}
