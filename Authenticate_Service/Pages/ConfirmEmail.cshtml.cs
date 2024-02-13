using Authenticate_Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Authenticate_Service.Pages
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly AuthenticationContext _context;
        public bool ShowConfirmation { get; set; }
        public ConfirmEmailModel (AuthenticationContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> OnGetasync(int userId)
        {

            var user = await _context.Users.FindAsync(userId);
            if (user == null )
            {
              
                await _context.SaveChangesAsync();
                ShowConfirmation = true;
            }
            user.EmailConfirmed = true;
            await _context.SaveChangesAsync();

            return Page();
        }
    }
}
