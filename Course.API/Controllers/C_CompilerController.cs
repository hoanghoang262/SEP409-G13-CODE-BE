using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

namespace CourseService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class C_CompilerController : ControllerBase
    {
        private readonly CCompiler _cCompiler;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public C_CompilerController(CCompiler cCompiler, IWebHostEnvironment env)
        {
            _hostingEnvironment = env;
            _cCompiler = cCompiler;
        }

        [HttpPost]
        public IActionResult CompileCodeC([FromBody] string cCode)
        {

            string rootPath = _hostingEnvironment.ContentRootPath;
            string filePath = Path.Combine(rootPath, "main");
            string compilationResult = _cCompiler.CompileCCode(cCode,filePath);
            return Ok(compilationResult);
        }
    }
}
public class CCompiler
{
    public string CompileCCode(string cCode, string filePath)
    {
        try
        {
            string cFilePath = Path.ChangeExtension(filePath, ".c");

            WriteCCodeToFile(cCode, cFilePath);

            // Compile the C code
            ProcessStartInfo psiCompile = new ProcessStartInfo
            {
                FileName = "gcc",
                Arguments = $"{cFilePath} -o {filePath}.exe",
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process compileProcess = Process.Start(psiCompile))
            {
                compileProcess.WaitForExit();
                if (compileProcess.ExitCode != 0)
                {
                    // Compilation failed
                    string errorOutput = compileProcess.StandardError.ReadToEnd();
                    return $"Compilation failed: {errorOutput}";
                }
            }

           
            ProcessStartInfo psiExecute = new ProcessStartInfo
            {
                FileName = $"{filePath}.exe",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
                
            };

            using (Process executeProcess = Process.Start(psiExecute))
            {
                executeProcess.WaitForExit();
                if (executeProcess.ExitCode == 0)
                {
                   
                    string output = executeProcess.StandardOutput.ReadToEnd();
                    return  output;
                }
                else
                {
                   
                    return $"Execution failed!";
                }
            }
        }
        catch (Exception ex)
        {
            return $"An error occurred: {ex.Message}";
        }
    }
    private void WriteCCodeToFile(string cCode, string cFilePath)
    {
        try
        {
            File.WriteAllText(cFilePath, cCode);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error writing C code to file: {ex.Message}");
        }
    }
}