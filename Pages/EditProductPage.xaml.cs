using CraftCart.Services;
using CraftCart.Models;

namespace CraftCart.Pages;

public partial class EditProductPage : ContentPage
{
    private readonly FirebaseDbService _dbService = new FirebaseDbService();
    private readonly Product _product;

    public EditProductPage(Product product)
    {
        InitializeComponent();
        _product = product;
        DisplayProduct();
    }

    private void DisplayProduct()
    {
        NameEntry.Text = _product.Name;
        DescriptionEditor.Text = _product.Description;
        PriceEntry.Text = _product.Price.ToString("F2");

        if (!string.IsNullOrWhiteSpace(_product.Category))
        {
            for (int i = 0; i < CategoryPicker.Items.Count; i++)
            {
                if (CategoryPicker.Items[i] == _product.Category)
                {
                    CategoryPicker.SelectedIndex = i;
                    break;
                }
            }
        }

        if (_product.HasImage)
        {
            CurrentPhoto.Source = ImageSource.FromFile(_product.ImageUrl);
            CurrentPhoto.IsVisible = true;
            PhotoPlaceholder.IsVisible = false;
        }
    }

    private async void OnUploadTapped(object? sender, TappedEventArgs e)
    {
        try
        {
            string path = await ImageService.PickAndSaveImage();
            if (string.IsNullOrEmpty(path))
                return;

            _product.ImageUrl = path;
            CurrentPhoto.Source = ImageSource.FromFile(path);
            CurrentPhoto.IsVisible = true;
            PhotoPlaceholder.IsVisible = false;
        }
        catch (Exception ex)
        {
            StatusLabel.TextColor = Colors.Red;
            StatusLabel.Text = ex.Message;
        }
    }

    private async void OnSaveClicked(object? sender, EventArgs e)
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

            _product.Name = NameEntry.Text;
            _product.Description = DescriptionEditor.Text ?? "";
            _product.Price = price;

            if (CategoryPicker.SelectedItem != null)
                _product.Category = CategoryPicker.SelectedItem.ToString() ?? _product.Category;

            await _dbService.UpdateProduct(_product);

            await DisplayAlertAsync("Success", "Product updated successfully", "OK");

            await Navigation.PushAsync(new SellerDashboardPage());
        }
        catch (Exception ex)
        {
            StatusLabel.TextColor = Colors.Red;
            StatusLabel.Text = ex.Message;
        }
    }

    private async void OnDeleteClicked(object? sender, EventArgs e)
    {
        bool confirm = await DisplayAlertAsync(
            "Confirm Delete",
            $"Are you sure you want to delete {_product.Name}?",
            "Yes",
            "No");

        if (!confirm)
            return;

        try
        {
            await _dbService.DeleteProduct(_product.Id);
            await DisplayAlertAsync("Deleted", "Product deleted", "OK");
            await Navigation.PushAsync(new SellerDashboardPage());
        }
        catch (Exception ex)
        {
            StatusLabel.TextColor = Colors.Red;
            StatusLabel.Text = ex.Message;
        }
    }

    private async void OnCancelClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new SellerDashboardPage());
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
