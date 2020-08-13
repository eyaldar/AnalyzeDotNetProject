using System.Collections;
using System.Collections.Generic;

namespace ProjectDependenciesVisualizer.Engine.Models
{
    public class ReferenceModel
    {
        public ReferenceModel(string assemblyName, string version, IEnumerable<ReferenceModel> dependencies)
        {
            AssemblyName = assemblyName;
            Version = version;
            Dependencies = dependencies;
        }

        public string AssemblyName { get; }
        public string Version { get; }
        public IEnumerable<ReferenceModel> Dependencies { get; }
    }
}