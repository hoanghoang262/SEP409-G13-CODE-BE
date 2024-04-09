using CompilerService.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

namespace CompilerService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class C_CompilerController : ControllerBase
    {
        private readonly CCompiler _cCompiler;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly CourseContext _context;

        public C_CompilerController(CCompiler cCompiler, IWebHostEnvironment env, CourseContext context)
        {
            _hostingEnvironment = env;
            _cCompiler = cCompiler;
            _context = context;
        }

        [HttpPost]
        public  IActionResult CompileCodeCCodeEditor(CodeRequestModel request)
        {

            string rootPath = _hostingEnvironment.ContentRootPath;
            string filePath = Path.Combine(rootPath, "main");
            string compilationResult = _cCompiler.CompileCCode(request.UserCode, filePath);

           
            var userAnswerCode = new UserAnswerCode
            {
                CodeQuestionId = request.PracticeQuestionId,
                AnswerCode = request.UserCode,
                UserId = request.UserId
            };
            _context.UserAnswerCodes.Add(userAnswerCode);
            _context.SaveChangesAsync();

            return Ok(compilationResult);
        }

        [HttpPost]
        public IActionResult CompileCodeCCodeQuestion(CodeRequestModel request)
        {

            string rootPath = _hostingEnvironment.ContentRootPath;
            string filePath = Path.Combine(rootPath, "main");
            string compilationResult = _cCompiler.CompileCCode(request.UserCode, filePath);
            if (compilationResult == "\n")
            {
                var completed = new CompletedPracticeQuestion
                {
                    PracticeQuestionId = request.PracticeQuestionId,
                    UserId = request.UserId
                };
                _context.CompletedPracticeQuestions.Add(completed);
                _context.SaveChanges();
                compilationResult = "All Test Passed";
            }
            var userAnswerCode = new UserAnswerCode
            {
                CodeQuestionId = request.PracticeQuestionId,
                AnswerCode = request.UserCode,
                UserId = request.UserId
            };
            _context.UserAnswerCodes.Add(userAnswerCode);
            _context.SaveChangesAsync();

            return Ok(compilationResult);
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
                        return output;
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
}
