
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CourseService.API.Models;

namespace CourseService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class testController : ControllerBase
    {
        private readonly TestDynamicCodeCompilerJava _compile;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly CourseContext _context;


        public testController(TestDynamicCodeCompilerJava compile, IWebHostEnvironment env, CourseContext context)
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
                _compile.TestWriteJavaCodeToFile(javaCode.UserCode, javaFilePath);

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

    public class TestDynamicCodeCompilerJava
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public TestDynamicCodeCompilerJava(IWebHostEnvironment env)
        {
            _hostingEnvironment = env;
        }

        public void TestWriteJavaCodeToFile(string javaCode, string javaFilePath)
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
                Arguments = "javac -cp \"libs/junit-platform-console-standalone-1.8.2.jar:.\" Solution.java ",
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
                Arguments = "-jar junit-platform-console-standalone.jar --class-path . --scan-class-path",
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
