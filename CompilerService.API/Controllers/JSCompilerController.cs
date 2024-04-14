using CompilerService.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace CompilerService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JSCompilerController : ControllerBase
    {
        private readonly DynamicCodeCompilerJavaTest _compile;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly CourseContext _context;

        public JSCompilerController(DynamicCodeCompilerJavaTest compile, IWebHostEnvironment env, CourseContext context)
        {
            _compile = compile;
            _hostingEnvironment = env;
            _context = context;
        }

        [HttpPost]
        public IActionResult CompileCodeJavaCodeEditor([FromBody] CodeLessonModel request)
        {
            string rootPath = _hostingEnvironment.ContentRootPath;
            string javaFilePath = Path.Combine(rootPath, "Solution.java");

            if (string.IsNullOrWhiteSpace(request.UserCode))
            {
                return BadRequest("Java code is missing.");
            }

            try
            {
                _compile.WriteJavaCodeToFile(request.UserCode, javaFilePath);

                string compilationResult = _compile.CompileAndRun(javaFilePath);

                var userAnswerCode = new CodeUserInLesson
                {
                    LessonId = request.LessonId,
                    UserCode = request.UserCode,
                    UserId = request.UserId
                };
                _context.CodeUserInLessons.Add(userAnswerCode);
                _context.SaveChangesAsync();

                // Return compilation result
                return Ok(compilationResult);
            }
            catch (Exception ex)
            {
                // Return error message
                return StatusCode(500, $"Error compiling Java code: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CompileCodeJavaCodeQuestion([FromBody] CodeRequestModel javaCode)
        {
            string rootPath = _hostingEnvironment.ContentRootPath;
            string javaFilePath = Path.Combine(rootPath, "Solution.java");

            if (string.IsNullOrWhiteSpace(javaCode.UserCode))
            {
                return BadRequest("Java code is missing.");
            }

            try
            {
                _compile.WriteJavaCodeToFile(javaCode.UserCode, javaFilePath);

                string compilationResult = _compile.CompileAndRun(javaFilePath);
                if (compilationResult == "\n")
                {
                    var completed = new CompletedPracticeQuestion
                    {
                        PracticeQuestionId = javaCode.PracticeQuestionId,
                        UserId = javaCode.UserId
                    };
                    _context.CompletedPracticeQuestions.Add(completed);
                    await _context.SaveChangesAsync();
                    compilationResult = "All Test Passed";
                }
                var userAnswerCode = new UserAnswerCode
                {
                    CodeQuestionId = javaCode.PracticeQuestionId,
                    AnswerCode = javaCode.UserCode,
                    UserId = javaCode.UserId
                };
                _context.UserAnswerCodes.Add(userAnswerCode);
                await _context.SaveChangesAsync();

                return Ok(compilationResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error compiling Java code: {ex.Message}");
            }
        }
    }

    public class DynamicCodeCompilerJavaTest
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public DynamicCodeCompilerJavaTest(IWebHostEnvironment env)
        {
            _hostingEnvironment = env;
        }

        public void WriteJavaCodeToFile(string javaCode, string javaFilePath)
        {
            try
            {
                File.WriteAllText(javaFilePath, javaCode);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error writing Java code to file: " + e.Message);
                throw;
            }
        }

        public string CompileAndRun(string javaFilePath)
        {
            string result = "";

            // Compile Java code
            string compileResult = CompileJava(javaFilePath);


            if (compileResult.Contains("Compilation error:"))
            {
                result = compileResult;
            }
            else if (!(string.IsNullOrEmpty(compileResult)))
            {
                result = ExecuteJavaProgram(javaFilePath);
            }

            return result;
        }

        private string CompileJava(string javaFilePath)
        {
            string result = "";

            var startInfo = new ProcessStartInfo
            {
                FileName = "javac",
                Arguments = $"\"{javaFilePath}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            try
            {
                using (var compileProcess = Process.Start(startInfo))
                {
                    string output = compileProcess.StandardOutput.ReadToEnd();
                    string error = compileProcess.StandardError.ReadToEnd();

                    if (!string.IsNullOrEmpty(error))
                    {
                        result = "Compilation error: " + error;
                    }
                    else
                    {
                        result = "Compilation successful: " + output;
                    }
                }
            }
            catch (Exception e)
            {
                result = "Error compiling Java code: " + e.Message;
            }

            return result;
        }

        private string ExecuteJavaProgram(string javaFilePath)
        {
            string result = "";

            // Extract class name from the Java file
            string className = GetJavaClassName(javaFilePath);

            var startInfo = new ProcessStartInfo
            {
                FileName = "java",
                Arguments = className, // Use the extracted class name here
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            try
            {
                using (var process = Process.Start(startInfo))
                {
                    var outputTask = process.StandardOutput.ReadToEndAsync();
                    var errorTask = process.StandardError.ReadToEndAsync();

                    Task.WaitAll(outputTask, errorTask);

                    result = outputTask.Result + "\n" + errorTask.Result;
                }
            }
            catch (Exception e)
            {
                result = "Error executing Java program: " + e.Message;
            }

            return result;
        }

        private string GetJavaClassName(string javaFilePath)
        {
            string[] lines = File.ReadAllLines(javaFilePath);
            foreach (string line in lines)
            {
                // Trim the line to handle spaces before or after the class keyword
                string trimmedLine = line.Trim();
                if (trimmedLine.Contains("class "))
                {
                    // Extract the class name from the line
                    int startIndex = line.IndexOf("class ") + "class ".Length;
                    int endIndex = line.IndexOf("{");
                    return line.Substring(startIndex, endIndex - startIndex).Trim();
                }
            }
            throw new InvalidOperationException("Class name not found in Java file.");
        }
    }
}
