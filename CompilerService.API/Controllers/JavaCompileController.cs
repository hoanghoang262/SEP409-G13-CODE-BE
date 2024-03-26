
using CompilerService.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace CourseService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JavaCompileController : ControllerBase
    {
        private readonly DynamicCodeCompilerJava _compile;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly CourseContext _context;


        public JavaCompileController(DynamicCodeCompilerJava compile, IWebHostEnvironment env, CourseContext context)
        {
            _compile = compile;
            _hostingEnvironment = env;
            _context = context;
        }

        [HttpPost]
        public IActionResult JavaCompileCode([FromBody] CodeRequestModel javaCode)
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

                var userAnswerCode = new UserAnswerCode
                {
                    CodeQuestionId = javaCode.PracticeQuestionId,
                    AnswerCode = javaCode.UserCode,
                    UserId = javaCode.UserId
                };
                _context.UserAnswerCodes.Add(userAnswerCode);
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
    }

    public class DynamicCodeCompilerJava
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public DynamicCodeCompilerJava(IWebHostEnvironment env)
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

            var startInfo = new ProcessStartInfo
            {
                FileName = "java",
                Arguments = "Solution", 
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

    }
}
