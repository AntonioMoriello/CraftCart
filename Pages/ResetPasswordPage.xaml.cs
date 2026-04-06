using CraftCart.Services;

namespace CraftCart.Pages;

public partial class ResetPasswordPage : ContentPage
{
    private FirebaseAuthService _authService = new FirebaseAuthService();

    public ResetPasswordPage()
    {
        InitializeComponent();
    }

    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(EmailEntry.Text))
            {
                result.Text = "Please enter your email";
                return;
            }

            await _authService.ResetPassword(EmailEntry.Text);
            await DisplayAlert("Success", "Password reset email sent", "OK");
        }
        catch (Exception ex)
        {
            result.Text = ex.Message;
        }
    }

    private async void OnGoToSignInPage(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SignInPage());
    }
}
