using System.Collections;
using System.Collections.Generic;

namespace ProjectDependenciesVisualizer.Engine.Models
{
    public class ProjectReferenceModel
    {
        public ProjectReferenceModel(string name, string version, IEnumerable<ProjectReferenceModel> dependencies)
        {
            Name = name;
            Version = version;
            Dependencies = dependencies;
        }

        public string Name { get; }
        public string Version { get; }
        public IEnumerable<ProjectReferenceModel> Dependencies { get; }
    }
}