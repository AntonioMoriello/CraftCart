using CraftCart.Services;
using CraftCart.Models;

namespace CraftCart.Pages;

public partial class OrderManagementPage : ContentPage
{
    private readonly FirebaseDbService _dbService = new FirebaseDbService();

    public OrderManagementPage()
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
            string sellerId = await SecureStorage.GetAsync("user_id") ?? "";

            var orders = await _dbService.GetOrdersBySeller(sellerId);

            foreach (var o in orders)
            {
                var items = await _dbService.GetOrderItemsByOrder(o.Id);
                o.ItemsText = string.Join(", ", items.Select(i => $"{i.ProductName} x {i.Quantity}"));
            }

            OrdersCollection.ItemsSource = orders;
            EmptyLabel.IsVisible = orders.Count == 0;
        }
        catch (Exception ex)
        {
            StatusLabel.Text = ex.Message;
        }
    }

    private async void OnAcceptClicked(object? sender, EventArgs e)
    {
        if (sender is not Button button || button.BindingContext is not Order order)
            return;

        try
        {
            await _dbService.UpdateOrderStatus(order.Id, "Shipped");
            await DisplayAlertAsync("Accepted", "Order marked as Shipped", "OK");
            await LoadOrders();
        }
        catch (Exception ex)
        {
            StatusLabel.Text = ex.Message;
        }
    }

    private async void OnDeclineClicked(object? sender, EventArgs e)
    {
        if (sender is not Button button || button.BindingContext is not Order order)
            return;

        bool confirm = await DisplayAlertAsync(
            "Confirm Decline",
            $"Decline order {order.ShortId}?",
            "Yes",
            "No");

        if (!confirm)
            return;

        try
        {
            await _dbService.UpdateOrderStatus(order.Id, "Declined");
            await LoadOrders();
        }
        catch (Exception ex)
        {
            StatusLabel.Text = ex.Message;
        }
    }

    private async void OnDashboardTapped(object? sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new SellerDashboardPage());
    }

    private void OnOrdersTapped(object? sender, TappedEventArgs e)
    {
    }

    private async void OnAccountTapped(object? sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new MyAccountPage());
    }
}
