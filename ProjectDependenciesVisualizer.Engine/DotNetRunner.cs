using System.Diagnostics;
using System.IO;
using System.Text;
using ProjectDependenciesVisualizer.Model;
using System.Threading.Tasks;
using ProjectDependenciesVisualizer.Engine.Interfaces;

namespace ProjectDependenciesVisualizer.Engine
{
    /// <remarks>
    /// Credit for the stuff happening in here goes to the https://github.com/jaredcnance/dotnet-status project
    /// </remarks>
    public class DotNetRunner : IRunner
    {
        public RunStatus Run(string workingDirectory, string[] arguments)
        {
            var psi = CreateProcessStartInfo(workingDirectory, arguments);
            return RunProcess(psi);
        }

        private static RunStatus RunProcess(ProcessStartInfo psi)
        {
            var p = new Process();
            try
            {
                p.StartInfo = psi;
                p.Start();

                var output = new StringBuilder();
                var errors = new StringBuilder();
                var outputTask = ConsumeStreamReaderAsync(p.StandardOutput, output);
                var errorTask = ConsumeStreamReaderAsync(p.StandardError, errors);

                var processExited = p.WaitForExit(20000);

                if (processExited == false)
                {
                    p.Kill();

                    return new RunStatus(output.ToString(), errors.ToString(), exitCode: -1);
                }

                Task.WaitAll(outputTask, errorTask);

                return new RunStatus(output.ToString(), errors.ToString(), p.ExitCode);
            }
            finally
            {
                p.Dispose();
            }
        }

        private static ProcessStartInfo CreateProcessStartInfo(string workingDirectory, string[] arguments)
        {
            return new ProcessStartInfo("dotnet", string.Join(" ", arguments))
            {
                WorkingDirectory = workingDirectory,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
        }

        private static async Task ConsumeStreamReaderAsync(StreamReader reader, StringBuilder lines)
        {
            await Task.Yield();

            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                lines.AppendLine(line);
            }
        }
    }
}