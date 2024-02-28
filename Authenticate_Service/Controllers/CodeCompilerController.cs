using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace DynamicCodeCompilerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CodeCompilerController : ControllerBase
    {
        private readonly DynamicCodeCompiler _codeCompiler;

        public CodeCompilerController(DynamicCodeCompiler codeCompiler)
        {
            _codeCompiler = codeCompiler;
        }

        [HttpPost]
        public IActionResult CompileAndRunCode([FromBody] CodeRequestModel request)
        {
            try
            {
                // Generate code based on user input
                string generatedCode = _codeCompiler.GenerateCode("Solution", request.UserCode);

                // Compile generated code
                Assembly assembly = _codeCompiler.CompileCode(generatedCode);

                // Invoke method from compiled assembly
                object result = _codeCompiler.InvokeMethod(assembly, "Solution", "Answer", request.Input);

                // Compare actual result with expected result
                if (Equals(result, request.ExpectedResult))
                {

                    return Ok("Test passed!");
                }
                else
                {
                    return BadRequest("Test failed: Expected result does not match actual result.");
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Argument error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }

    public class CodeRequestModel
    {
        public int Input { get; set; }
        public int ExpectedResult { get; set; }
        public string UserCode { get; set; }
    }

    public class DynamicCodeCompiler
    {
        public string GenerateCode(string className, string userCode)
        {
            return userCode;
        }

        public Assembly CompileCode(string codeToCompile)
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(codeToCompile);
            string assemblyName = Path.GetRandomFileName();
            var refPaths = new[]
            {
                typeof(object).GetTypeInfo().Assembly.Location,
                typeof(Console).GetTypeInfo().Assembly.Location,
                Path.Combine(Path.GetDirectoryName(typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly.Location), "System.Runtime.dll")
            };
            MetadataReference[] references = refPaths.Select(r => MetadataReference.CreateFromFile(r)).ToArray();

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    string errorMessage = string.Join("\n", failures.Select(failure => $"{failure.Id}: {failure.GetMessage()}"));
                    throw new InvalidOperationException($"Failed to compile code:\n{errorMessage}");
                }

                ms.Seek(0, SeekOrigin.Begin);
                return AssemblyLoadContext.Default.LoadFromStream(ms);
            }
        }

        public object InvokeMethod(Assembly assembly, string className, string methodName, int inputValues)
        {
            var type = assembly.GetType(className);
            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod(methodName, new[] { typeof(int) });

            // Get the parameter types of the method
            var parameterTypes = method.GetParameters().Select(p => p.ParameterType).ToArray();

            // Check if the number of input values matches the number of parameters
            //if (parameterTypes.Length != inputValues.Length)
            //{
            //    throw new ArgumentException("Number of input values does not match the number of parameters.");
            //}

            // Convert each input value to the corresponding parameter type
            //var convertedInputs = new object[inputValues.Length];
            //for (int i = 0; i < inputValues.Length; i++)
            //{
            //    var paramType = parameterTypes[i];
            //    var value = inputValues[i];

            //    // Check if the input value is already of the correct type
            //    if (value != null && paramType.IsAssignableFrom(value.GetType()))
            //    {
            //        // No need for conversion, the value is already of the correct type
            //        convertedInputs[i] = value;
            //    }
            //    else
            //    {
            //        try
            //        {
            //            // Convert the input value to the parameter type
            //            convertedInputs[i] = Convert.ChangeType(value, paramType);
            //        }
            //        catch (InvalidCastException)
            //        {
            //            throw new ArgumentException($"Input value at index {i} is not convertible to type {paramType}.");
            //        }
            //    }
            //}

            // Invoke the method
            // object methodResult = method.Invoke(instance, convertedInputs);

            // Return the result
            return (int)method.Invoke(instance, new object[] { inputValues }); ;
        }



    }
}
