// In CMS.Web/Pages/Posts/Create.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CMS.Web.Models; // Adjust if your Post model is elsewhere
using System.Net.Http; // Required for HttpClient
using System.Net.Http.Json; // For PostAsJsonAsync
using System.Threading.Tasks;
using System; // Required for DateTime and Exception
using Microsoft.Extensions.Logging; // Optional for logging

namespace CMS.Web.Pages.Posts
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CreateModel> _logger; // Optional for logging

        public CreateModel(IHttpClientFactory httpClientFactory, ILogger<CreateModel> logger) // Added logger
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger; // Optional for logging
        }

        [BindProperty]
        public Post NewPost { get; set; } = new Post { PublishedDate = DateTime.Now };

        public string? ErrorMessage { get; set; }
        // SuccessMessage is not strictly needed if redirecting, but can be useful if staying on page
        // public string? SuccessMessage { get; set; }

        public void OnGet()
        {
            // Initialize default values if not done in property initializer
            // NewPost.PublishedDate = DateTime.Now; // Example
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var httpClient = _httpClientFactory.CreateClient("CmsApiClient");
            try
            {
                // _logger.LogInformation("Attempting to create a new post via API."); // Optional logging
                // API expects Id to be 0 or not present. Our Post model's Id is int, defaults to 0.
                var response = await httpClient.PostAsJsonAsync("api/posts", NewPost);

                if (response.IsSuccessStatusCode)
                {
                    // _logger.LogInformation("Post created successfully via API."); // Optional logging
                    // Optionally, could pass a success message via TempData if needed after redirect
                    // TempData["SuccessMessage"] = "Post created successfully!";
                    return RedirectToPage("./Index", new { created = "true" }); // Pass a query param to indicate success
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ErrorMessage = $"Error creating post: {response.StatusCode} - {errorContent}";
                    // _logger.LogWarning(ErrorMessage); // Optional logging
                    return Page();
                }
            }
            catch (HttpRequestException ex) // More specific exception
            {
                ErrorMessage = $"Exception while creating post: {ex.Message}. Check if the API is running and accessible at {httpClient.BaseAddress}api/posts.";
                // _logger.LogError(ex, ErrorMessage); // Optional logging
                return Page();
            }
            catch (Exception ex) // Generic fallback
            {
                ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                // _logger.LogError(ex, ErrorMessage); // Optional logging
                return Page();
            }
        }
    }
}
