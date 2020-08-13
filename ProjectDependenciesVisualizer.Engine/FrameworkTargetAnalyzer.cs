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
        public ProjectReferenceModel Analyze(TargetFrameworkInformation targetFramework, LockFileTarget lockFileTarget)
        {
            var references = new List<ProjectReferenceModel>();

            foreach(var dependency in targetFramework.Dependencies)
            {
                var projectLibrary = lockFileTarget.Libraries.FirstOrDefault(library => library.Name == dependency.Name);

                AddReference(lockFileTarget, references, projectLibrary);
            }

            return new ProjectReferenceModel(targetFramework.FrameworkName.ToString(), string.Empty, references);
        }

        private void AddReference(LockFileTarget lockFileTarget, List<ProjectReferenceModel> references, LockFileTargetLibrary projectLibrary)
        {
            IEnumerable<ProjectReferenceModel> childReferences = AnalyzeReferences(projectLibrary, lockFileTarget);

            references.Add(new ProjectReferenceModel(projectLibrary.Name, projectLibrary.Version.ToString(), childReferences));
        }

        public IEnumerable<ProjectReferenceModel> AnalyzeReferences(LockFileTargetLibrary projectLibrary, LockFileTarget lockFileTarget)
        {
            var references = new List<ProjectReferenceModel>();

            foreach (var childDependency in projectLibrary.Dependencies)
            {
                var childLibrary = lockFileTarget.Libraries.FirstOrDefault(library => library.Name == childDependency.Id);

                IEnumerable<ProjectReferenceModel> childReferences = AnalyzeReferences(childLibrary, lockFileTarget);

                AddReference(lockFileTarget, references, childLibrary);
            }

            return references;
        }
    }
}
