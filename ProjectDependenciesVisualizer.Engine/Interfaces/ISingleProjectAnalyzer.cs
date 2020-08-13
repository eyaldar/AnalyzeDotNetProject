using System;
using System.Collections.Generic;
using System.Text;
using ProjectDependenciesVisualizer.Engine.Models;

namespace ProjectDependenciesVisualizer.Engine.Interfaces
{
    public interface ISingleProjectAnalyzer
    {
        public ProjectModel Analyze(NuGet.ProjectModel.PackageSpec project);
    }
}
