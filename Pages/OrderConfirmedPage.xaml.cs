using CraftCart.Services;
using CraftCart.Models;

namespace CraftCart.Pages;

public partial class OrderConfirmedPage : ContentPage
{
    private readonly FirebaseDbService _dbService = new FirebaseDbService();
    private readonly string _orderId;
    private int _rating = 1;

    public OrderConfirmedPage(string orderId, string orderDate, int itemCount, double total)
    {
        InitializeComponent();
        _orderId = orderId;
        OrderIdLabel.Text = orderId;
        OrderDateLabel.Text = orderDate;
        OrderItemsLabel.Text = itemCount.ToString();
        OrderTotalLabel.Text = $"${total:F2}";
    }

    private void SetStars(int count)
    {
        _rating = count;
        Star1Label.Text = count >= 1 ? "\u2B50" : "\u2606";
        Star2Label.Text = count >= 2 ? "\u2B50" : "\u2606";
        Star3Label.Text = count >= 3 ? "\u2B50" : "\u2606";
        Star4Label.Text = count >= 4 ? "\u2B50" : "\u2606";
        Star5Label.Text = count >= 5 ? "\u2B50" : "\u2606";
    }

    private void OnStar1Tapped(object? sender, TappedEventArgs e)
    {
        SetStars(1);
    }

    private void OnStar2Tapped(object? sender, TappedEventArgs e)
    {
        SetStars(2);
    }

    private void OnStar3Tapped(object? sender, TappedEventArgs e)
    {
        SetStars(3);
    }

    private void OnStar4Tapped(object? sender, TappedEventArgs e)
    {
        SetStars(4);
    }

    private void OnStar5Tapped(object? sender, TappedEventArgs e)
    {
        SetStars(5);
    }

    private async void OnSubmitReviewClicked(object? sender, EventArgs e)
    {
        try
        {
            string buyerId = await SecureStorage.GetAsync("user_id") ?? "";
            string buyerEmail = await SecureStorage.GetAsync("user_email") ?? "";

            Review review = new Review
            {
                ProductId = _orderId,
                BuyerId = buyerId,
                BuyerEmail = buyerEmail,
                Rating = _rating,
                Comment = ReviewEntry.Text ?? "",
                Date = DateTime.Now.ToString("yyyy-MM-dd")
            };

            await _dbService.AddReview(review);
            await DisplayAlertAsync("Thank you", "Your review has been submitted", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    private void OnContinueShoppingClicked(object? sender, EventArgs e)
    {
        Application.Current!.MainPage = new NavigationPage(new BrowseProductsPage());
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
