using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CMS.Web.Services;
using CMS.Web.Models; // Using CMS.Web.Models.Company
using System.Threading.Tasks;
using System.Net.Http; // Required for HttpResponseMessage

namespace CMS.Web.Pages.Companies
{
    public class EditModel : PageModel
    {
        private readonly CompanyService _companyService;

        [BindProperty]
        public Company CompanyToUpdate { get; set; } = new Company();

        public string? ErrorMessage { get; set; }

        public EditModel(CompanyService companyService)
        {
            _companyService = companyService;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound("Company ID not provided.");
            }

            try
            {
                var company = await _companyService.GetCompanyByIdAsync(id.Value);
                if (company == null)
                {
                    return NotFound($"Company with ID {id.Value} not found.");
                }
                CompanyToUpdate = company;
                return Page();
            }
            catch (System.Exception ex)
            {
                ErrorMessage = $"Error fetching company details: {ex.Message}";
                // Consider RedirectToPage with TempData error if preferred
                return Page(); 
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Ensure CompanyToUpdate.Id is correctly bound from the form (via hidden input)
                HttpResponseMessage response = await _companyService.UpdateCompanyAsync(CompanyToUpdate.Id, CompanyToUpdate);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Company updated successfully.";
                    return RedirectToPage("./Index");
                }
                else
                {
                    // string errorContent = await response.Content.ReadAsStringAsync();
                    // ErrorMessage = $"Could not update company. API Error: {response.ReasonPhrase}. Details: {errorContent}";
                    ErrorMessage = $"Could not update company. Status code: {response.StatusCode}. Reason: {response.ReasonPhrase}";
                    return Page();
                }
            }
            catch (System.Exception ex)
            {
                ErrorMessage = $"An error occurred while updating the company: {ex.Message}";
                return Page();
            }
        }
    }
}
