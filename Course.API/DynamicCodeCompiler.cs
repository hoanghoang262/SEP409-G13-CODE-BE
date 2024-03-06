using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis;
using System.Reflection;
using System.Runtime.Loader;
using System.Diagnostics;

namespace CompileCodeOnline
{
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
        public object InvokeMethod(Assembly assembly, string className, string methodName, object inputValue)
        {
            var type = assembly.GetType(className);
            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod(methodName);
            object[] parameters = new object[] { inputValue };


            Stopwatch stopwatch = Stopwatch.StartNew();
            object result = method.Invoke(instance, parameters);
            stopwatch.Stop();

            Console.WriteLine($"Execution time of method {methodName}: {stopwatch.ElapsedMilliseconds} milliseconds");
            return result;
        }
        public object InvokeMethodArrayInt(Assembly assembly, string className, string methodName, int[] inputValue)
        {
            var type = assembly.GetType(className);
            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod(methodName);
            object[] parameters = new object[] { inputValue };

            Stopwatch stopwatch = Stopwatch.StartNew();
            object result = method.Invoke(instance, parameters);
            stopwatch.Stop();

            Console.WriteLine($"Execution time of method {methodName}: {stopwatch.ElapsedMilliseconds} milliseconds");
            return result;
        }
       

    }
}
