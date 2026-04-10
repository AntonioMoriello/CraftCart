using CraftCart.Services;

namespace CraftCart.Pages;

public partial class SignInPage : ContentPage
{
    private readonly FirebaseAuthService _authService = new FirebaseAuthService();
    private readonly FirebaseDbService _dbService = new FirebaseDbService();

    public SignInPage()
    {
        InitializeComponent();
    }

    private async void OnSignInClicked(object? sender, EventArgs e)
    {
        try
        {
            var authResult = await _authService.SignIn(
                EmailEntry.Text,
                PasswordEntry.Text
            );

            await SecureStorage.SetAsync("firebase_token", authResult);

            var user = await _dbService.GetUserByEmail(EmailEntry.Text);

            string role = user?.Role ?? "Buyer";

            await SecureStorage.SetAsync("user_email", EmailEntry.Text);
            await SecureStorage.SetAsync("user_role", role);
            await SecureStorage.SetAsync("user_id", user?.Id ?? "");

            if (role == "Buyer")
            {
                Application.Current!.MainPage = new NavigationPage(new BrowseProductsPage());
            }
            else
            {
                Application.Current!.MainPage = new NavigationPage(new SellerDashboardPage());
            }
        }
        catch (Exception ex)
        {
            ResultLabel.Text = ex.Message;
        }
    }

    private async void OnResetClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new ResetPasswordPage());
    }

    private async void OnGoToSignUpPage(object? sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new SignUpPage());
    }
}
