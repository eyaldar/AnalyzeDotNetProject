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
            var references = new List<ReferenceModel>();

            foreach(var dependency in targetFramework.Dependencies)
            {
                var projectLibrary = lockFileTarget.Libraries.FirstOrDefault(library => library.Name == dependency.Name);

                AddReference(lockFileTarget, references, projectLibrary);
            }

            return new FrameworkTargetModel(targetFramework.FrameworkName.ToString(), references);
        }

        private void AddReference(LockFileTarget lockFileTarget, List<ReferenceModel> references, LockFileTargetLibrary projectLibrary)
        {
            IEnumerable<ReferenceModel> childReferences = AnalyzeReferences(projectLibrary, lockFileTarget);

            references.Add(new ReferenceModel(projectLibrary.Name, projectLibrary.Version.ToString(), childReferences));
        }

        public IEnumerable<ReferenceModel> AnalyzeReferences(LockFileTargetLibrary projectLibrary, LockFileTarget lockFileTarget)
        {
            var references = new List<ReferenceModel>();

            foreach (var childDependency in projectLibrary.Dependencies)
            {
                var childLibrary = lockFileTarget.Libraries.FirstOrDefault(library => library.Name == childDependency.Id);

                IEnumerable<ReferenceModel> childReferences = AnalyzeReferences(childLibrary, lockFileTarget);

                AddReference(lockFileTarget, references, childLibrary);
            }

            return references;
        }
    }
}
