using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuGet.ProjectModel;
using ProjectDependenciesVisualizer.Engine.Interfaces;
using ProjectDependenciesVisualizer.Engine.Models;

namespace ProjectDependenciesVisualizer.Engine
{
    public class FrameworkTargetAnalyzer : IFrameworkTargetAnalyzer
    {
        public FrameworkTargetModel Analyze(TargetFrameworkInformation targetFramework, LockFileTarget lockFileTarget)
        {
            foreach(var dependency in targetFramework.Dependencies)
            {
                var projectLibrary = lockFileTarget.Libraries.FirstOrDefault(library => library.Name == dependency.Name);

                IEnumerable<ReferenceModel> references = AnalyzeReferences(projectLibrary, lockFileTarget);
            }

            return null;
        }

        public IEnumerable<ReferenceModel> AnalyzeReferences(LockFileTargetLibrary projectLibrary, LockFileTarget lockFileTarget)
        {
            var references = new List<ReferenceModel>();

            foreach (var childDependency in projectLibrary.Dependencies)
            {
                var childLibrary = lockFileTarget.Libraries.FirstOrDefault(library => library.Name == childDependency.Id);

                IEnumerable<ReferenceModel> childReferences = AnalyzeReferences(childLibrary, lockFileTarget);

                references.Add(new ReferenceModel(childLibrary.Name, childLibrary.Version.ToString(), childReferences));
            }

            return references;
        }
    }
}
