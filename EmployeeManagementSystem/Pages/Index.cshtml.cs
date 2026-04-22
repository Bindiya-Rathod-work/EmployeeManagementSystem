using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementSystem.Pages
{
    /// <summary>
    /// Default home page — redirects users based on authentication status.
    /// </summary>
    public class IndexModel : PageModel
    {
        /// <summary>
        /// GET: Redirect to dashboard if logged in, otherwise go to login page.
        /// </summary>
        public IActionResult OnGet()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                    return RedirectToPage("/Admin/Dashboard");

                return RedirectToPage("/EmployeePortal/Profile");
            }

            return RedirectToPage("/Auth/Login");
        }
    }
}