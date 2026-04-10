using CraftCart.Services;
using CraftCart.Models;

namespace CraftCart.Pages;

public partial class ShoppingCartPage : ContentPage
{
    public ShoppingCartPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        RefreshCart();
    }

    private void RefreshCart()
    {
        var items = CartService.GetCartItems();
        CartCollection.ItemsSource = null;
        CartCollection.ItemsSource = items;

        ItemCountLabel.Text = $"{items.Count} items";
        SubtotalLabel.Text = $"${CartService.GetSubtotal():F2}";
        TaxLabel.Text = $"${CartService.GetTax():F2}";
        ShippingLabel.Text = $"${CartService.GetShipping():F2}";
        TotalLabel.Text = $"${CartService.GetTotal():F2}";

        EmptyLabel.IsVisible = items.Count == 0;
    }

    private void OnPlusClicked(object? sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is CartItem item)
        {
            CartService.UpdateQuantity(item.ProductId, item.Quantity + 1);
            RefreshCart();
        }
    }

    private void OnMinusClicked(object? sender, EventArgs e)
    {
        if (sender is not Button button || button.BindingContext is not CartItem item)
        {
            return;
        }

        if (item.Quantity <= 1)
        {
            CartService.RemoveFromCart(item.ProductId);
        }
        else
        {
            CartService.UpdateQuantity(item.ProductId, item.Quantity - 1);
        }
        RefreshCart();
    }

    private async void OnCheckoutClicked(object? sender, EventArgs e)
    {
        var items = CartService.GetCartItems();
        if (items.Count == 0)
        {
            await DisplayAlertAsync("Empty Cart", "Add items to your cart first", "OK");
            return;
        }
        await Navigation.PushAsync(new PaymentPage());
    }

    private async void OnBrowseTapped(object? sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new BrowseProductsPage());
    }

    private void OnCartTapped(object? sender, TappedEventArgs e)
    {
    }

    private async void OnOrdersTapped(object? sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new MyOrdersPage());
    }

    private async void OnAccountTapped(object? sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new MyAccountPage());
    }
}
