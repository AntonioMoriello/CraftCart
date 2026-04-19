using CraftCart.Services;
using CraftCart.Models;

namespace CraftCart.Pages;

public partial class AddProductPage : ContentPage
{
    private readonly FirebaseDbService _dbService = new FirebaseDbService();
    private string _imagePath = string.Empty;

    public AddProductPage()
    {
        InitializeComponent();
    }

    private async void OnUploadTapped(object? sender, TappedEventArgs e)
    {
        try
        {
            string path = await ImageService.PickAndSaveImage();
            if (string.IsNullOrEmpty(path))
                return;

            _imagePath = path;
            PreviewImage.Source = ImageSource.FromFile(path);
            PreviewBorder.IsVisible = true;
        }
        catch (Exception ex)
        {
            StatusLabel.TextColor = Colors.Red;
            StatusLabel.Text = ex.Message;
        }
    }

    private async void OnSubmitClicked(object? sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(NameEntry.Text))
            {
                StatusLabel.TextColor = Colors.Red;
                StatusLabel.Text = "Please enter a product name";
                return;
            }

            if (string.IsNullOrWhiteSpace(PriceEntry.Text) || !double.TryParse(PriceEntry.Text, out double price))
            {
                StatusLabel.TextColor = Colors.Red;
                StatusLabel.Text = "Please enter a valid price";
                return;
            }

            if (CategoryPicker.SelectedItem == null)
            {
                StatusLabel.TextColor = Colors.Red;
                StatusLabel.Text = "Please select a category";
                return;
            }

            string sellerId = await SecureStorage.GetAsync("user_id") ?? "";
            string sellerEmail = await SecureStorage.GetAsync("user_email") ?? "";

            Product product = new Product
            {
                Name = NameEntry.Text,
                Description = DescriptionEditor.Text ?? "",
                Price = price,
                Category = CategoryPicker.SelectedItem.ToString() ?? "",
                ImageUrl = _imagePath,
                SellerId = sellerId,
                SellerName = sellerEmail,
                AverageRating = 0,
                ReviewCount = 0,
                SalesCount = 0
            };

            await _dbService.AddProduct(product);

            await DisplayAlertAsync("Success", "Product added successfully", "OK");

            await Navigation.PushAsync(new SellerDashboardPage());
        }
        catch (Exception ex)
        {
            StatusLabel.TextColor = Colors.Red;
            StatusLabel.Text = ex.Message;
        }
    }

    private async void OnDashboardTapped(object? sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new SellerDashboardPage());
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
