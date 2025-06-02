using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CMS.Web.Services;
using CMS.Web.Models; // Using CMS.Web.Models.Company
using System.Threading.Tasks;
using System.Net.Http; // Required for HttpResponseMessage

namespace CMS.Web.Pages.Companies
{
    public class CreateModel : PageModel
    {
        private readonly CompanyService _companyService;

        [BindProperty]
        public Company NewCompany { get; set; } = new Company(); // Initialize to avoid null issues

        public string? ErrorMessage { get; set; }

        public CreateModel(CompanyService companyService)
        {
            _companyService = companyService;
        }

        public IActionResult OnGet()
        {
            // Optionally initialize NewCompany with default values if needed
            // NewCompany = new Company { Name = "Default Name" };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                HttpResponseMessage response = await _companyService.CreateCompanyAsync(NewCompany);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Company created successfully.";
                    return RedirectToPage("./Index");
                }
                else
                {
                    // You could try to read the error content from the API response
                    // string errorContent = await response.Content.ReadAsStringAsync();
                    // ErrorMessage = $"Could not create company. API Error: {response.ReasonPhrase}. Details: {errorContent}";
                    ErrorMessage = $"Could not create company. Status code: {response.StatusCode}. Reason: {response.ReasonPhrase}";
                    return Page();
                }
            }
            catch (System.Exception ex)
            {
                ErrorMessage = $"An error occurred while creating the company: {ex.Message}";
                return Page();
            }
        }
    }
}
