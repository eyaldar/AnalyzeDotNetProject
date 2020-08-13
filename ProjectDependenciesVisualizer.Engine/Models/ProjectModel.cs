using System;
using System.Collections.Generic;

namespace ProjectDependenciesVisualizer.Engine.Models
{
    public class ProjectModel
    {
        public ProjectModel(string name, string version, IEnumerable<FrameworkTargetModel> frameworks)
        {
            Name = name;
            Version = version;
            Frameworks = frameworks;
        }

        public string Name { get; }
        public string Version { get; }
        public IEnumerable<FrameworkTargetModel> Frameworks { get; }
    }
}