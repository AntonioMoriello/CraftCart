using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace CraftCart.Services
{
    internal class FirebaseAuthService
    {
        private const string ApiKey = "AIzaSyAd1FgI_XzbqpSbMAYawLWyrbddk0kQMA0";
        private readonly HttpClient _httpClient;

        public FirebaseAuthService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> SignUp(string email, string password)
        {
            var url =
                $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={ApiKey}";

            var data = new
            {
                email = email,
                password = password,
                returnSecureToken = true
            };

            var response = await _httpClient.PostAsJsonAsync(url, data);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(content);

            return content;
        }

        public async Task<string> SignIn(string email, string password)
        {
            var url =
                $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={ApiKey}";

            var data = new
            {
                email = email,
                password = password,
                returnSecureToken = true
            };

            var response = await _httpClient.PostAsJsonAsync(url, data);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(content);

            return content;
        }

        public async Task<string> ResetPassword(string email)
        {
            var url =
                $"https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key={ApiKey}";

            var data = new
            {
                requestType = "PASSWORD_RESET",
                email = email
            };

            var response = await _httpClient.PostAsJsonAsync(url, data);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(content);

            return content;
        }

        public async Task<string> ChangePassword(string idToken, string newPassword)
        {
            var url =
                $"https://identitytoolkit.googleapis.com/v1/accounts:update?key={ApiKey}";

            var data = new
            {
                idToken = idToken,
                password = newPassword,
                returnSecureToken = true
            };

            var response = await _httpClient.PostAsJsonAsync(url, data);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(content);

            return content;
        }
    }
}
