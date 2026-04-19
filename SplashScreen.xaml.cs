using CraftCart.Services;

namespace CraftCart;

public partial class SplashScreen : ContentPage
{
    public SplashScreen()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var loading = LoadingBar.ProgressTo(1.0, 3000, Easing.Linear);
        var seeding = SeedService.SeedProductsIfEmpty();

        await Task.WhenAll(loading, seeding);

        Application.Current!.MainPage = new NavigationPage(new CraftCart.Pages.SignInPage());
    }
}
