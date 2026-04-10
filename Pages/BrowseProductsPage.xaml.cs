using CraftCart.Services;
using CraftCart.Models;

namespace CraftCart.Pages;

public partial class BrowseProductsPage : ContentPage
{
    private readonly FirebaseDbService _dbService = new FirebaseDbService();
    private List<Product> _allProducts = new List<Product>();
    private string _currentCategory = "All";

    public BrowseProductsPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadProducts();
    }

    private async Task LoadProducts()
    {
        try
        {
            _allProducts = await _dbService.GetProducts();
            FilterAndDisplay();
        }
        catch (Exception ex)
        {
            StatusLabel.Text = ex.Message;
        }
    }

    private void FilterAndDisplay()
    {
        List<Product> filteredProducts = _allProducts;

        if (_currentCategory != "All")
        {
            filteredProducts = filteredProducts
                .Where(p => p.Category == _currentCategory)
                .ToList();
        }

        if (!string.IsNullOrWhiteSpace(SearchEntry.Text))
        {
            filteredProducts = filteredProducts
                .Where(p => p.Name.ToLower().Contains(SearchEntry.Text.ToLower()))
                .ToList();
        }

        ProductCollection.ItemsSource = filteredProducts;
        StatusLabel.Text = filteredProducts.Count == 0 ? "No products found" : string.Empty;
    }

    private void OnSearchTextChanged(object? sender, TextChangedEventArgs e)
    {
        FilterAndDisplay();
    }

    private void SetCategory(string category)
    {
        _currentCategory = category;
        FilterAndDisplay();
    }

    private void OnFilterAll(object? sender, TappedEventArgs e)
    {
        SetCategory("All");
    }

    private void OnFilterJewelry(object? sender, TappedEventArgs e)
    {
        SetCategory("Jewelry");
    }

    private void OnFilterPottery(object? sender, TappedEventArgs e)
    {
        SetCategory("Pottery");
    }

    private void OnFilterArt(object? sender, TappedEventArgs e)
    {
        SetCategory("Art");
    }

    private void OnFilterTextiles(object? sender, TappedEventArgs e)
    {
        SetCategory("Textiles");
    }

    private async void OnProductSelected(object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Product selectedProduct)
        {
            ProductCollection.SelectedItem = null;
            await Navigation.PushAsync(new ProductDetailPage(selectedProduct));
        }
    }

    private void OnBrowseTapped(object? sender, TappedEventArgs e)
    {
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
