using System.Text;
using System.Text.Json;
using ToDo.Models;

namespace ToDo.Services
{
    public static class UserService
    {
        private static readonly HttpClient _client = new HttpClient();
        private const string BaseUrl = "https://todo-list.dcism.org";
        private static UserModel _currentUser;

        public static async Task<(bool Success, string Message)> Register(string firstName, string lastName, string email, string password, string confirmPassword)
        {
            try
            {
                var payload = new
                {
                    first_name = firstName,
                    last_name = lastName,
                    email = email,
                    password = password,
                    confirm_password = confirmPassword
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync($"{BaseUrl}/signup_action.php", content);
                var responseBody = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(responseBody);
                var root = doc.RootElement;

                int status = root.GetProperty("status").GetInt32();
                string message = root.GetProperty("message").GetString();

                return (status == 200, message);
            }
            catch (Exception ex)
            {
                return (false, $"Connection error: {ex.Message}");
            }
        }

        public static async Task<(bool Success, string Message)> Login(string email, string password)
        {
            try
            {
                var url = $"{BaseUrl}/signin_action.php?email={Uri.EscapeDataString(email)}&password={Uri.EscapeDataString(password)}";
                var response = await _client.GetAsync(url);
                var responseBody = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(responseBody);
                var root = doc.RootElement;

                int status = root.GetProperty("status").GetInt32();
                string message = root.GetProperty("message").GetString();

                if (status == 200)
                {
                    var data = root.GetProperty("data");
                    _currentUser = new UserModel
                    {
                        id = data.GetProperty("id").GetInt32(),
                        fname = data.GetProperty("fname").GetString(),
                        lname = data.GetProperty("lname").GetString(),
                        email = data.GetProperty("email").GetString(),
                        timemodified = data.GetProperty("timemodified").GetString()
                    };
                    return (true, message);
                }

                return (false, message);
            }
            catch (Exception ex)
            {
                return (false, $"Connection error: {ex.Message}");
            }
        }

        public static void Logout()
        {
            _currentUser = null;
        }

        public static UserModel GetCurrentUser()
        {
            return _currentUser;
        }
    }
}
