using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace VetSystemWeb.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public LoginInput Input { get; set; } = new();

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var client = _httpClientFactory.CreateClient();
            var json = JsonSerializer.Serialize(Input);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync("https://localhost:7146/api/Auth/Login", content);
                if (response.IsSuccessStatusCode)
                {
                    var resultJson = await response.Content.ReadAsStringAsync();
                    var tokenObj = JsonSerializer.Deserialize<TokenResponse>(resultJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    // Сохраняем токен в cookie (или session)
                    Response.Cookies.Append("jwtToken", tokenObj!.Token, new Microsoft.AspNetCore.Http.CookieOptions { HttpOnly = true });

                    return RedirectToPage("/Index");
                }
                else
                {
                    ErrorMessage = "Email or password is incorrect.";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Server error: " + ex.Message;
                return Page();
            }
        }

        public class LoginInput
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public class TokenResponse
        {
            public string Token { get; set; } = string.Empty;
        }
    }
}