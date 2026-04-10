using CraftCart.Services;
using CraftCart.Models;

namespace CraftCart.Pages;

public partial class SignUpPage : ContentPage
{
    private readonly FirebaseAuthService _authService = new FirebaseAuthService();
    private readonly FirebaseDbService _dbService = new FirebaseDbService();

    public SignUpPage()
    {
        InitializeComponent();
    }

    private async void OnSignUpClicked(object? sender, EventArgs e)
    {
        try
        {
            if (PasswordEntry.Text != ConfirmPasswordEntry.Text)
            {
                ResultLabel.Text = "Passwords do not match";
                return;
            }

            string role = "";
            if (BuyerRadio.IsChecked) role = "Buyer";
            else if (SellerRadio.IsChecked) role = "Seller";
            else
            {
                ResultLabel.Text = "Please select a role";
                return;
            }

            var authResult = await _authService.SignUp(
                EmailEntry.Text,
                PasswordEntry.Text
            );

            await SecureStorage.SetAsync("firebase_token", authResult);

            var user = new User
            {
                Email = EmailEntry.Text,
                Role = role,
                DisplayName = EmailEntry.Text
            };

            await _dbService.AddUser(user);

            await SecureStorage.SetAsync("user_email", EmailEntry.Text);
            await SecureStorage.SetAsync("user_role", role);
            await SecureStorage.SetAsync("user_id", user.Id ?? "");

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

    private async void OnGoToSignInPage(object? sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new SignInPage());
    }
}
