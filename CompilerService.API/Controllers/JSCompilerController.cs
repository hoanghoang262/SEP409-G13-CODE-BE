using JavaScriptEngineSwitcher.Core;
using JavaScriptEngineSwitcher.V8;
using Microsoft.AspNetCore.Mvc;

namespace CompilerService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JSCompilerController : ControllerBase
    {
        [HttpPost]
        public IActionResult CompileAndReturnResult([FromBody] string javascriptCode)
        {
            try
            {
                using (IJsEngine engine = new V8JsEngine())
                {
                    // Biên dịch mã JavaScript thành chuỗi
                    string compiledCode = engine.Evaluate<string>("(" + javascriptCode + ").toString()");

                    // Trả về kết quả biên dịch
                    return Ok(compiledCode);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
