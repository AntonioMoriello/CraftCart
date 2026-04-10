using CraftCart.Services;
using CraftCart.Models;

namespace CraftCart.Pages;

public partial class PaymentPage : ContentPage
{
    private readonly FirebaseDbService _dbService = new FirebaseDbService();

    public PaymentPage()
    {
        InitializeComponent();
        LoadSummary();
    }

    private void LoadSummary()
    {
        var items = CartService.GetCartItems();
        SummaryCollection.ItemsSource = items;
        SubtotalLabel.Text = $"${CartService.GetSubtotal():F2}";
        TaxLabel.Text = $"${CartService.GetTax():F2}";
        ShippingLabel.Text = $"${CartService.GetShipping():F2}";
        TotalLabel.Text = $"${CartService.GetTotal():F2}";
    }

    private void OnCardholderChanged(object? sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(e.NewTextValue))
            CardholderPreviewLabel.Text = e.NewTextValue.ToUpper();
        else
            CardholderPreviewLabel.Text = "YOUR NAME";
    }

    private void OnCardNumberChanged(object? sender, TextChangedEventArgs e)
    {
        string num = e.NewTextValue ?? "";
        num = num.Replace(" ", "");

        if (num.Length >= 4)
        {
            string last4 = num.Substring(num.Length - 4);
            string masked = "\u2022\u2022\u2022\u2022  \u2022\u2022\u2022\u2022  \u2022\u2022\u2022\u2022  " + last4;
            CardNumberPreviewLabel.Text = masked;
        }
        else
        {
            CardNumberPreviewLabel.Text = "\u2022\u2022\u2022\u2022  \u2022\u2022\u2022\u2022  \u2022\u2022\u2022\u2022  \u2022\u2022\u2022\u2022";
        }
    }

    private void OnExpiryChanged(object? sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(e.NewTextValue))
            ExpiryPreviewLabel.Text = e.NewTextValue;
        else
            ExpiryPreviewLabel.Text = "MM/YY";
    }

    private async void OnPayClicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(CardholderEntry.Text))
        {
            ErrorLabel.Text = "Please enter cardholder name";
            return;
        }
        if (string.IsNullOrWhiteSpace(CardNumberEntry.Text) || CardNumberEntry.Text.Replace(" ", "").Length < 16)
        {
            ErrorLabel.Text = "Please enter a valid card number";
            return;
        }
        if (string.IsNullOrWhiteSpace(ExpiryEntry.Text))
        {
            ErrorLabel.Text = "Please enter expiry date";
            return;
        }
        if (string.IsNullOrWhiteSpace(CvvEntry.Text) || CvvEntry.Text.Length < 3)
        {
            ErrorLabel.Text = "Please enter a valid CVV";
            return;
        }

        try
        {
            string buyerId = await SecureStorage.GetAsync("user_id") ?? "";
            string buyerEmail = await SecureStorage.GetAsync("user_email") ?? "";

            Order order = new Order
            {
                BuyerId = buyerId,
                BuyerEmail = buyerEmail,
                Status = "Processing",
                Subtotal = CartService.GetSubtotal(),
                Tax = CartService.GetTax(),
                Shipping = CartService.GetShipping(),
                Total = CartService.GetTotal(),
                OrderDate = DateTime.Now.ToString("yyyy-MM-dd")
            };

            string orderId = await _dbService.AddOrderGetKey(order);

            var cartItems = CartService.GetCartItems();
            foreach (var item in cartItems)
            {
                OrderItem orderItem = new OrderItem
                {
                    OrderId = orderId,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    Price = item.Price
                };
                await _dbService.AddOrderItem(orderItem);
            }

            int totalItems = cartItems.Sum(i => i.Quantity);
            CartService.ClearCart();

            await Navigation.PushAsync(new OrderConfirmedPage(orderId, order.OrderDate, totalItems, order.Total));
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.Message;
        }
    }

    private async void OnBrowseTapped(object? sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new BrowseProductsPage());
    }

    private async void OnCartTapped(object? sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new ShoppingCartPage());
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
