using System;
using System.Collections;
using System.Collections.Generic;

namespace ProjectDependenciesVisualizer.Engine.Models
{
    public class FrameworkTargetModel
    {
        public FrameworkTargetModel(string name, IEnumerable<ReferenceModel> references)
        {
            Name = name;
            References = references;
        }

        public string Name { get; }
        public IEnumerable<ReferenceModel> References { get; }
    }
}