using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.ProjectModel;
using ProjectDependenciesVisualizer.Engine.Interfaces;

namespace ProjectDependenciesVisualizer.Engine
{
    /// <remarks>
    /// Credit for the stuff happening in here goes to the https://github.com/jaredcnance/dotnet-status project
    /// </remarks>
    public class DependencyGraphService : IDependencyGraphGenerator
    {
        private readonly IRunner runner;

        public DependencyGraphService(IRunner runner)
        {
            this.runner = runner;
        }
        
        public DependencyGraphSpec GenerateDependencyGraph(string projectPath)
        {
            string dgOutput = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
                
            string[] arguments = new[] {"msbuild", $"\"{projectPath}\"", "/t:GenerateRestoreGraphFile", $"/p:RestoreGraphOutputPath={dgOutput}"};

            var runStatus = runner.Run(Path.GetDirectoryName(projectPath), arguments);

            if (runStatus.IsSuccess)
            {
                string dependencyGraphText = File.ReadAllText(dgOutput);
                return new DependencyGraphSpec(JsonConvert.DeserializeObject<JObject>(dependencyGraphText));
            }
            else
            {
                throw new Exception($"Unable to process the the project `{projectPath}. Are you sure this is a valid .NET Core or .NET Standard project type?" +
                                                     $"\r\n\r\nHere is the full error message returned from the Microsoft Build Engine:\r\n\r\n" + runStatus.Output);
            }
        }
    }
}