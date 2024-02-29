using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CourseService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JavaCompileController : ControllerBase
    {
        private readonly DynamicCodeCompilerJava _compile;
        public JavaCompileController(DynamicCodeCompilerJava compile)
        {
            _compile = compile;
        }
        [HttpPost]
        public IActionResult CompileCodeJava([FromBody] CodeRequestModel javaCode)
        {
            // Check if the request contains Java code
            if (string.IsNullOrWhiteSpace(javaCode.UserCode))
            {
                return BadRequest("Java code is missing.");
            }

            try
            {
                // Create a temporary file path for the Java code
                //string javaFilePath = Path.Combine(Path.GetTempPath(), "HelloWorld.java");
               string javaFilePath = @"..\Solution.java";

              _compile.WriteJavaCodeToFile(javaCode.UserCode, javaFilePath);
               


                object compilationResult = _compile.RunJavaProgram(javaFilePath);

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
        public  void WriteJavaCodeToFile(string javaCode, string javaFilePath)
        {
            try
            {
                File.WriteAllText(javaFilePath, javaCode);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error writing Java code to file: " + e.Message);
            }
        }

        public string RunJavaProgram(string javaFilePath)
        {
            string result = "";
            string javaHome = Environment.GetEnvironmentVariable("javaHome");

            //if (string.IsNullOrWhiteSpace(javaHome))
            //{
            //    Console.WriteLine("JAVA_HOME environment variable is not set.");
            //    return "JAVA_HOME environment variable is not set.";
            //}

            //string javaExePath = Path.Combine(javaHome, "bin", "java.exe");

            //if (!File.Exists(javaExePath))
            //{
            //    Console.WriteLine("Java executable not found at the specified path: " + javaExePath);
            //    return "Java executable not found.";
            //}

            var startInfo = new ProcessStartInfo
            {
                FileName = "java.exe",
                Arguments = javaFilePath,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            try
            {
                using (var process = Process.Start(startInfo))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    string outputErr = process.StandardError.ReadToEnd();
                    Console.WriteLine("Output of Java code:");
                    Console.WriteLine(output);

                    if (!string.IsNullOrWhiteSpace(outputErr))
                    {
                        Console.WriteLine("Error output of Java code:");
                        Console.WriteLine(outputErr);
                        result = outputErr;
                        throw new Exception("Compilation failed. Check the error output for details.");
                    }
                    else if (process.ExitCode != 0)
                    {
                        string errorMessage = $"Compilation failed with exit code {process.ExitCode}";
                        Console.WriteLine(errorMessage);
                        result = errorMessage;
                        throw new Exception(errorMessage);
                    }
                    else
                    {
                        Console.WriteLine("Compilation successful.");
                        result = output;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error compiling Java code: " + e.Message);
                result = e.Message;
            }
            return result;
        }

    }
}
