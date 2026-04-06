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

        await loadingBar.ProgressTo(1.0, 3000, Easing.Linear);

        Application.Current.MainPage = new NavigationPage(new CraftCart.Pages.SignInPage());
    }
}
