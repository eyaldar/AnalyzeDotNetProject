using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NuGet.ProjectModel;
using ProjectDependenciesVisualizer.Engine.Interfaces;
using ProjectDependenciesVisualizer.Engine.Models;

namespace ProjectDependenciesVisualizer.Engine
{
    public class SingleProjectAnalyzer : ISingleProjectAnalyzer
    {
        private IRunner runner;
        private IFrameworkTargetAnalyzer frameworkTargetAnalyzer;

        public SingleProjectAnalyzer(IRunner runner,
                                     IFrameworkTargetAnalyzer frameworkTargetAnalyzer)
        {
            this.runner = runner;
            this.frameworkTargetAnalyzer = frameworkTargetAnalyzer;
        }

        public ProjectReferenceModel Analyze(PackageSpec project)
        {
            var frameworkModels = new List<ProjectReferenceModel>();
            LockFile lockFile = GetLockFile(project.FilePath, project.RestoreMetadata.OutputPath);

            foreach(TargetFrameworkInformation targetFramework in project.TargetFrameworks)
            {
                LockFileTarget lockFileTargetFramework = lockFile.Targets.FirstOrDefault(t => t.TargetFramework.Equals(targetFramework.FrameworkName));

                if(lockFileTargetFramework != null)
                {
                    var framework = frameworkTargetAnalyzer.Analyze(targetFramework, lockFileTargetFramework);
                    frameworkModels.Add(framework);
                }
            }

            return new ProjectReferenceModel(project.Name, project.Version.ToString(), frameworkModels);
        }

        private LockFile GetLockFile(string projectPath, string outputPath)
        {
            // Run the restore command
            string[] arguments = new[] {"restore", $"\"{projectPath}\""};
            var runStatus = runner.Run(Path.GetDirectoryName(projectPath), arguments);

            // Load the lock file
            string lockFilePath = Path.Combine(outputPath, "project.assets.json");
            
            return LockFileUtilities.GetLockFile(lockFilePath, NuGet.Common.NullLogger.Instance);
        }
    }
}
