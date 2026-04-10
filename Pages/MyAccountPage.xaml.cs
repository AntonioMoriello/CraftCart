using CraftCart.Services;
using CraftCart.Models;

namespace CraftCart.Pages;

public partial class MyAccountPage : ContentPage
{
    FirebaseDbService _dbService = new FirebaseDbService();
    FirebaseAuthService _authService = new FirebaseAuthService();
    User _currentUser;

    public MyAccountPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadUserData();
        await LoadStats();
    }

    private async Task LoadUserData()
    {
        try
        {
            string email = await SecureStorage.GetAsync("user_email") ?? "";
            _currentUser = await _dbService.GetUserByEmail(email);

            if (_currentUser != null)
            {
                firstNameEntry.Text = _currentUser.FirstName;
                lastNameEntry.Text = _currentUser.LastName;
                emailEntry.Text = _currentUser.Email;
                phoneEntry.Text = _currentUser.Phone;
                addressEntry.Text = _currentUser.Address;

                string displayName = _currentUser.FirstName + " " + _currentUser.LastName;
                if (string.IsNullOrWhiteSpace(displayName.Trim()))
                    displayName = _currentUser.DisplayName ?? _currentUser.Email;

                profileName.Text = displayName;
                profileEmail.Text = _currentUser.Email;
                profileRole.Text = _currentUser.Role;
            }
        }
        catch (Exception ex)
        {
            statusLabel.TextColor = Colors.Red;
            statusLabel.Text = ex.Message;
        }
    }

    private async Task LoadStats()
    {
        try
        {
            string userId = await SecureStorage.GetAsync("user_id") ?? "";

            int orderCount = await _dbService.GetOrderCountByBuyer(userId);
            double totalSpent = await _dbService.GetTotalSpentByBuyer(userId);
            int reviewCount = await _dbService.GetReviewCountByBuyer(userId);

            totalOrdersLabel.Text = orderCount.ToString();
            totalSpentLabel.Text = $"${totalSpent:F2}";
            reviewsLeftLabel.Text = reviewCount.ToString();
        }
        catch (Exception ex)
        {
            statusLabel.TextColor = Colors.Red;
            statusLabel.Text = ex.Message;
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (_currentUser == null)
        {
            statusLabel.TextColor = Colors.Red;
            statusLabel.Text = "User not loaded";
            return;
        }

        try
        {
            _currentUser.FirstName = firstNameEntry.Text;
            _currentUser.LastName = lastNameEntry.Text;
            _currentUser.Phone = phoneEntry.Text;
            _currentUser.Address = addressEntry.Text;
            _currentUser.DisplayName = firstNameEntry.Text + " " + lastNameEntry.Text;

            await _dbService.UpdateUser(_currentUser);

            profileName.Text = _currentUser.DisplayName;

            statusLabel.TextColor = Colors.Green;
            statusLabel.Text = "Profile updated successfully";
        }
        catch (Exception ex)
        {
            statusLabel.TextColor = Colors.Red;
            statusLabel.Text = ex.Message;
        }
    }

    private async void OnChangePasswordClicked(object sender, EventArgs e)
    {
        string newPassword = await DisplayPromptAsync(
            "Change Password",
            "Enter new password",
            keyboard: Keyboard.Default);

        if (string.IsNullOrWhiteSpace(newPassword))
            return;

        if (newPassword.Length < 6)
        {
            await DisplayAlert("Error", "Password must be at least 6 characters", "OK");
            return;
        }

        string confirmPassword = await DisplayPromptAsync(
            "Confirm Password",
            "Re-enter new password",
            keyboard: Keyboard.Default);

        if (newPassword != confirmPassword)
        {
            await DisplayAlert("Error", "Passwords do not match", "OK");
            return;
        }

        try
        {
            string token = await SecureStorage.GetAsync("firebase_token") ?? "";
            await _authService.ChangePassword(token, newPassword);
            await DisplayAlert("Success", "Password changed successfully", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void OnLogOutClicked(object sender, EventArgs e)
    {
        SecureStorage.Remove("firebase_token");
        SecureStorage.Remove("user_email");
        SecureStorage.Remove("user_role");
        SecureStorage.Remove("user_id");
        Application.Current.MainPage = new NavigationPage(new SignInPage());
    }

    private async void OnBrowseTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new BrowseProductsPage());
    }

    private async void OnCartTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ShoppingCartPage());
    }

    private async void OnOrdersTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MyOrdersPage());
    }
}
