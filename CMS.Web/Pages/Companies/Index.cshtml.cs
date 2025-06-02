using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CMS.Web.Services;
using CMS.Web.Models; // Using CMS.Web.Models.Company
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMS.Web.Pages.Companies
{
    public class IndexModel : PageModel
    {
        private readonly CompanyService _companyService;

        public IList<Company> Companies { get; set; } = new List<Company>(); // Initialize to avoid null reference

        [TempData]
        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        public IndexModel(CompanyService companyService)
        {
            _companyService = companyService;
        }

        public async Task OnGetAsync()
        {
            try
            {
                var companies = await _companyService.GetCompaniesAsync();
                if (companies != null)
                {
                    Companies = companies;
                }
                else
                {
                    ErrorMessage = "No companies found or error loading companies.";
                    // Companies is already initialized to an empty list
                }
            }
            catch (System.Exception ex)
            {
                ErrorMessage = $"Error loading companies: {ex.Message}";
                // Companies is already initialized to an empty list
            }
        }
    }
}
