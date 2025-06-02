using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CMS.Web.Services;
using CMS.Web.Models; // Using CMS.Web.Models.Company
using System.Threading.Tasks;
using System.Net.Http; // Required for HttpResponseMessage

namespace CMS.Web.Pages.Companies
{
    public class DeleteModel : PageModel
    {
        private readonly CompanyService _companyService;

        [BindProperty]
        public Company CompanyToDelete { get; set; } = new Company();

        public string? ErrorMessage { get; set; }
        // SuccessMessage will be handled by TempData on redirect to Index

        public DeleteModel(CompanyService companyService)
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
                    TempData["ErrorMessage"] = $"Company with ID {id.Value} not found. It might have already been deleted.";
                    return RedirectToPage("./Index");
                }
                CompanyToDelete = company;
                return Page();
            }
            catch (System.Exception ex)
            {
                // Using TempData for error on redirect to Index if company cannot be loaded for delete confirmation
                TempData["ErrorMessage"] = $"Error fetching company details for deletion: {ex.Message}";
                return RedirectToPage("./Index");
            }
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound("Company ID not provided for deletion.");
            }
            // CompanyToDelete.Id is bound from the hidden field, can also use it.
            // if (CompanyToDelete.Id != id.Value) { /* Mismatch, handle error */ }


            try
            {
                HttpResponseMessage response = await _companyService.DeleteCompanyAsync(id.Value);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Company deleted successfully.";
                    return RedirectToPage("./Index");
                }
                else
                {
                    // Attempt to reload company details for the page if deletion failed
                    var company = await _companyService.GetCompanyByIdAsync(id.Value);
                    if (company != null)
                    {
                        CompanyToDelete = company;
                    }
                    else
                    {
                        // If company not found after failed delete, means it was likely deleted by another process
                        TempData["ErrorMessage"] = $"Company with ID {id.Value} could not be found after a failed delete attempt. It might have already been deleted.";
                        return RedirectToPage("./Index");
                    }
                    ErrorMessage = $"Could not delete company. Status code: {response.StatusCode}. Reason: {response.ReasonPhrase}";
                    return Page();
                }
            }
            catch (System.Exception ex)
            {
                 // Attempt to reload company details
                var company = await _companyService.GetCompanyByIdAsync(id.Value);
                if (company != null)
                {
                    CompanyToDelete = company;
                }
                else
                {
                     TempData["ErrorMessage"] = $"Company with ID {id.Value} could not be reloaded after an exception during delete. It might have been deleted.";
                     return RedirectToPage("./Index");
                }
                ErrorMessage = $"An error occurred while trying to delete the company: {ex.Message}";
                return Page();
            }
        }
    }
}
