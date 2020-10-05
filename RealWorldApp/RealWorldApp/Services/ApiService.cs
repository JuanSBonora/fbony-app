using Newtonsoft.Json;
using RealWorldApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace RealWorldApp.Services
{
    public class ApiService
    {
        public async Task<bool> RegisterUser(string name, string email, string password)
        {
            var register = new Register()
            {
                Name = name,
                Email = email,
                Password = password
            };
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(register);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "api/Accounts/Register", content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }
        public static async Task<bool> Login(string email, string password)
        {
            var login = new Login()
            {
                Email = email,
                Password = password
            };
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(login);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "api/Accounts/Login", content);
            if (!response.IsSuccessStatusCode) return false;
            var jsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Token>(jsonResult);
            Preferences.Set("AccessToken", result.access_token);
            Preferences.Set("userId", result.user_Id);
            Preferences.Set("userName", result.user_name);
            return true;
        }
        public static async Task<List<Category>> GetCategories()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("AccessToken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + "api/Categories");
            return JsonConvert.DeserializeObject<List<Category>>(response);
        }
        public static async Task<Product> GetProductById(int productId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("AccessToken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + "api/Products/" + productId);
            return JsonConvert.DeserializeObject<Product>(response);
        }
        public static async Task<List<ProductByCategory>> GetProductByCategory(int categoryId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("AccessToken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + "Products/ProductsByCategory/" + categoryId);
            return JsonConvert.DeserializeObject<List<ProductByCategory>>(response);
        }
        public static async Task<List<PopularProduct>> GetPopularProducts()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("AccessToken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + "Products/PopularProducts");
            return JsonConvert.DeserializeObject<List<PopularProduct>>(response);
        }
        public async Task<bool> AddItemsInCart(AddToCart addToCart)
        {
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(addToCart);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("AccessToken", string.Empty));
            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "api/ShoppingCartItems", content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }
        public static async Task<CartSubTotal> GetCartSubTotal(int userId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("AccessToken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + "api/ShoppingCartItems/SubTotal/" + userId);
            return JsonConvert.DeserializeObject<CartSubTotal>(response);
        }
        public static async Task<List<ShoppingCartItem>> GetShoppingCartItems(int userId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("AccessToken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + "api/ShoppingCartItems/" + userId);
            return JsonConvert.DeserializeObject<List<ShoppingCartItem>>(response);
        }
        public static async Task<TotalCartItem> GetTotalCartItems(int userId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("AccessToken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + "api/ShoppingCartItems/TotalItems/" + userId);
            return JsonConvert.DeserializeObject<TotalCartItem>(response);
        }
    }
}
