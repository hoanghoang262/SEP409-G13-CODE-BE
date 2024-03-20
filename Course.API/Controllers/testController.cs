using CompileCodeOnline;
using CourseService.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System.Reflection;



namespace DynamicCodeCompilerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class testController : ControllerBase
    {
        private readonly DynamicCodeCompiler _codeCompiler;
        private readonly CourseContext _context;
        private readonly CompileCode compileCode;

        public testController(DynamicCodeCompiler codeCompiler, CourseContext context, CompileCode _compileCode)
        {
            _codeCompiler = codeCompiler;
            _context = context;
            compileCode = _compileCode;
        }

        [HttpPost]
        public IActionResult CompileCodeCSharp(CodeRequestModel request)
        {
            try
            {
                var userAnswerCode = new UserAnswerCode
                {
                    CodeQuestionId = request.PracticeQuestionId,
                    AnswerCode = request.UserCode,
                    UserId = request.UserId
                };

                _context.UserAnswerCodes.Add(userAnswerCode);
                _context.SaveChanges();

                string result = compileCode.CompileAndRun(request.UserCode);

                if (result != null)
                {
                    // Log the result of the compilation
                    Console.WriteLine("Compilation result: " + result);
                }
                else
                {
                    // Log that no result was returned
                    Console.WriteLine("No result returned from compilation");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("Exception occurred: " + ex.Message);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }


}
public class CompileCode
{
    public string CompileAndRun(string code)
    {
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
        string assemblyName = Path.GetRandomFileName();
        var refPaths = new[]
        {
                typeof(object).GetTypeInfo().Assembly.Location,
                typeof(Console).GetTypeInfo().Assembly.Location,
                Path.Combine(Path.GetDirectoryName(typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly.Location), "System.Runtime.dll"),
                Path.Combine(Path.GetDirectoryName(typeof(System.Linq.Enumerable).GetTypeInfo().Assembly.Location), "System.Linq.dll"),
                Path.Combine(Path.GetDirectoryName(typeof(object).GetTypeInfo().Assembly.Location), "System.dll"),

            };
        MetadataReference[] references = refPaths.Select(r => MetadataReference.CreateFromFile(r)).ToArray();

        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName,
            syntaxTrees: new[] { syntaxTree },
            references: references,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        // Perform compilation
        using (var ms = new MemoryStream())
        {
            var emitResult = compilation.Emit(ms);

            if (!emitResult.Success)
            {
                // If compilation fails, return error messages
                var errors = emitResult.Diagnostics
                    .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
                    .Select(diagnostic => diagnostic.GetMessage());

                return string.Join("\n", errors);
            }
            else
            {
                // Compilation succeeded, load the assembly and return success message
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                var assembly = System.Reflection.Assembly.Load(ms.ToArray());
                var type = assembly.GetType("DynamicCode");
                var method = type.GetMethod("Execute");

                try
                {
                    object result = method.Invoke(null, null);
                    return "Compilation successful! Result: " + result.ToString();
                }
                catch (Exception ex)
                {
                    return "Error executing compiled code: " + ex.Message;
                }
            }
        }
    }
}





