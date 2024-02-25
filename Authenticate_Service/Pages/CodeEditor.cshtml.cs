using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthenticateService.API.Pages
{
    public class CodeEditorModel : PageModel
    {
        public string Code { get; set; }

        public void OnGet()
        {
            // Mặc định, bạn có thể load code từ cơ sở dữ liệu hoặc các nguồn khác vào đây
            Code = "function add(a, b) { return a + b; }";
        }
    }
}
