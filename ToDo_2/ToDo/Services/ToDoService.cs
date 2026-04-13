using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using ToDo.Models;

namespace ToDo.Services
{
    public static class ToDoService
    {
        private static readonly HttpClient _client = new HttpClient();
        private const string BaseUrl = "https://todo-list.dcism.org";

        public static async Task<ObservableCollection<ToDoClass>> GetItems(int userId, string status)
        {
            try
            {
                var url = $"{BaseUrl}/getItems_action.php?status={Uri.EscapeDataString(status)}&user_id={userId}";
                var response = await _client.GetAsync(url);
                var responseBody = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(responseBody);
                var root = doc.RootElement;

                int statusCode = root.GetProperty("status").GetInt32();

                if (statusCode == 200 && root.TryGetProperty("data", out var data))
                {
                    var items = new ObservableCollection<ToDoClass>();

                    foreach (var property in data.EnumerateObject())
                    {
                        var itemObj = property.Value;

                        // Skip non-object properties like "count"
                        if (itemObj.ValueKind != JsonValueKind.Object || !itemObj.TryGetProperty("item_id", out _))
                            continue;

                        items.Add(new ToDoClass
                        {
                            item_id = itemObj.GetProperty("item_id").GetInt32(),
                            item_name = itemObj.GetProperty("item_name").GetString(),
                            item_description = itemObj.GetProperty("item_description").GetString(),
                            status = itemObj.GetProperty("status").GetString(),
                            user_id = itemObj.GetProperty("user_id").GetInt32()
                        });
                    }

                    return items;
                }

                return new ObservableCollection<ToDoClass>();
            }
            catch
            {
                return new ObservableCollection<ToDoClass>();
            }
        }

        public static async Task<ObservableCollection<ToDoClass>> GetActiveItems(int userId)
        {
            return await GetItems(userId, "active");
        }

        public static async Task<ObservableCollection<ToDoClass>> GetCompletedItems(int userId)
        {
            return await GetItems(userId, "inactive");
        }

        public static async Task<(bool Success, string Message)> AddItem(string itemName, string itemDescription, int userId)
        {
            try
            {
                var payload = new
                {
                    item_name = itemName,
                    item_description = itemDescription,
                    user_id = userId
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync($"{BaseUrl}/addItem_action.php", content);
                var responseBody = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(responseBody);
                var root = doc.RootElement;

                int statusCode = root.GetProperty("status").GetInt32();
                string message = root.GetProperty("message").GetString();

                return (statusCode == 200, message);
            }
            catch (Exception ex)
            {
                return (false, $"Connection error: {ex.Message}");
            }
        }

        public static async Task<(bool Success, string Message)> UpdateItem(int itemId, string itemName, string itemDescription)
        {
            try
            {
                var payload = new
                {
                    item_id = itemId,
                    item_name = itemName,
                    item_description = itemDescription
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Put, $"{BaseUrl}/editItem_action.php")
                {
                    Content = content
                };

                var response = await _client.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(responseBody);
                var root = doc.RootElement;

                int statusCode = root.GetProperty("status").GetInt32();
                string message = root.GetProperty("message").GetString();

                return (statusCode == 200, message);
            }
            catch (Exception ex)
            {
                return (false, $"Connection error: {ex.Message}");
            }
        }

        public static async Task<(bool Success, string Message)> ChangeStatus(int itemId, string newStatus)
        {
            try
            {
                var payload = new
                {
                    item_id = itemId,
                    status = newStatus
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Put, $"{BaseUrl}/statusItem_action.php")
                {
                    Content = content
                };

                var response = await _client.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(responseBody);
                var root = doc.RootElement;

                int statusCode = root.GetProperty("status").GetInt32();
                string message = root.GetProperty("message").GetString();

                return (statusCode == 200, message);
            }
            catch (Exception ex)
            {
                return (false, $"Connection error: {ex.Message}");
            }
        }

        public static async Task<(bool Success, string Message)> DeleteItem(int itemId)
        {
            try
            {
                var url = $"{BaseUrl}/deleteItem_action.php?item_id={itemId}";

                var request = new HttpRequestMessage(HttpMethod.Delete, url);

                var response = await _client.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(responseBody);
                var root = doc.RootElement;

                int statusCode = root.GetProperty("status").GetInt32();
                string message = root.GetProperty("message").GetString();

                return (statusCode == 200, message);
            }
            catch (Exception ex)
            {
                return (false, $"Connection error: {ex.Message}");
            }
        }
    }
}
