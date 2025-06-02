using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json; // Required for GetFromJsonAsync, PostAsJsonAsync, PutAsJsonAsync
using System.Text.Json; // Potentially for JsonSerializerOptions if needed, though GetFromJsonAsync handles common cases
using System.Threading.Tasks;
using CMS.Web.Models; // Using the newly created Company model

namespace CMS.Web.Services
{
    public class CompanyService
    {
        private readonly HttpClient _httpClient;

        public CompanyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Company>?> GetCompaniesAsync()
        {
            // Using GetFromJsonAsync for cleaner code
            return await _httpClient.GetFromJsonAsync<List<Company>>("api/companies");
        }

        public async Task<Company?> GetCompanyByIdAsync(int id)
        {
            // Using GetFromJsonAsync for cleaner code
            return await _httpClient.GetFromJsonAsync<Company>($"api/companies/{id}");
        }

        public async Task<HttpResponseMessage> CreateCompanyAsync(Company company)
        {
            // Using PostAsJsonAsync for cleaner code
            return await _httpClient.PostAsJsonAsync("api/companies", company);
        }

        public async Task<HttpResponseMessage> UpdateCompanyAsync(int id, Company company)
        {
            // Using PutAsJsonAsync for cleaner code
            return await _httpClient.PutAsJsonAsync($"api/companies/{id}", company);
        }

        public async Task<HttpResponseMessage> DeleteCompanyAsync(int id)
        {
            return await _httpClient.DeleteAsync($"api/companies/{id}");
        }
    }
}
