// In CMS.Web/Pages/Posts/Index.cshtml.cs
using Microsoft.AspNetCore.Mvc.RazorPages;
using CMS.Web.Models; // Adjust namespace if your Post model is elsewhere
using System.Net.Http.Json; // For ReadFromJsonAsync
using System.Collections.Generic;
using System.Threading.Tasks;
using System; // For Exception
using Microsoft.Extensions.Logging; // Optional: For logging

namespace CMS.Web.Pages.Posts
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<IndexModel> _logger; // Optional: For logging

        public IndexModel(IHttpClientFactory httpClientFactory, ILogger<IndexModel> logger) // Added logger
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger; // Optional: For logging
        }

        public IList<Post> Posts { get; set; } = new List<Post>();
        public string? ErrorMessage { get; set; }

        public async Task OnGetAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("CmsApiClient");
            try
            {
                // _logger.LogInformation("Attempting to fetch posts from API."); // Optional: Logging
                var response = await httpClient.GetAsync("api/posts");

                if (response.IsSuccessStatusCode)
                {
                    // _logger.LogInformation("API call successful. Reading content."); // Optional: Logging
                    var postsList = await response.Content.ReadFromJsonAsync<List<Post>>();
                    if (postsList != null)
                    {
                        Posts = postsList;
                        // _logger.LogInformation($"Successfully fetched and deserialized {Posts.Count} posts."); // Optional: Logging
                    }
                    else
                    {
                        ErrorMessage = "API returned no data or data could not be deserialized.";
                        // _logger.LogWarning(ErrorMessage); // Optional: Logging
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ErrorMessage = $"Error fetching posts: {response.StatusCode} - {errorContent}";
                    // _logger.LogError(ErrorMessage); // Optional: Logging
                }
            }
            catch (HttpRequestException ex) // More specific exception
            {
                ErrorMessage = $"Exception while fetching posts: {ex.Message}. Check if the API is running and accessible at {httpClient.BaseAddress}api/posts.";
                // _logger.LogError(ex, ErrorMessage); // Optional: Logging
            }
            catch (Exception ex) // Generic fallback
            {
                ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                // _logger.LogError(ex, ErrorMessage); // Optional: Logging
            }
        }
    }
}
