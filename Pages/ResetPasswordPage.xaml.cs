using CraftCart.Services;

namespace CraftCart.Pages;

public partial class ResetPasswordPage : ContentPage
{
    private readonly FirebaseAuthService _authService = new FirebaseAuthService();

    public ResetPasswordPage()
    {
        InitializeComponent();
    }

    private async void OnSubmitClicked(object? sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(EmailEntry.Text))
            {
                ResultLabel.Text = "Please enter your email";
                return;
            }

            await _authService.ResetPassword(EmailEntry.Text);
            await DisplayAlertAsync("Success", "Password reset email sent", "OK");
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
