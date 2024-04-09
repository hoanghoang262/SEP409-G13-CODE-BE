using CompilerService.API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;

namespace CourseService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CPlus_CompilerController : ControllerBase
    {
        private readonly CPlushCompiler _cCompiler;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly CourseContext _context;

        public CPlus_CompilerController(CPlushCompiler cCompiler, IWebHostEnvironment env, CourseContext context)
        {
            _hostingEnvironment = env;
            _cCompiler = cCompiler;
            _context = context;
        }

        [HttpPost]
        public IActionResult CompileCodeCPlusCodeEditor([FromBody] CodeRequestModel request)
        {
            try
            {
                string rootPath = _hostingEnvironment.ContentRootPath;
                string filePath = Path.Combine(rootPath, "main.cpp");
                string compilationResult = _cCompiler.CompileCCode(request.UserCode, filePath);

                if(compilationResult.Contains("Failed"))
                {

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
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpPost]
        public IActionResult CompileCodeCPlusCodeQuestion([FromBody] CodeRequestModel request)
        {
            try
            {
                string rootPath = _hostingEnvironment.ContentRootPath;
                string filePath = Path.Combine(rootPath, "main.cpp");
                string compilationResult = _cCompiler.CompileCCode(request.UserCode, filePath);
                if (compilationResult.Contains("All Test Pass"))
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
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }

    public class CPlushCompiler
    {
        public string CompileCCode(string cCode, string filePath)
        {
            try
            {
                string cppFilePath = Path.ChangeExtension(filePath, ".cpp");

                WriteCCodeToFile(cCode, cppFilePath);

                // Compile the C++ code
                ProcessStartInfo psiCompile = new ProcessStartInfo
                {
                    FileName = "g++",
                    Arguments = $"{cppFilePath} -o {Path.ChangeExtension(filePath, ".exe")}",
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
                    FileName = Path.ChangeExtension(filePath, ".exe"),
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
