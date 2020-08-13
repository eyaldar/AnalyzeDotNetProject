using System;
using System.Collections.Generic;
using System.Text;
using NuGet.ProjectModel;
using ProjectDependenciesVisualizer.Engine.Models;

namespace ProjectDependenciesVisualizer.Engine.Interfaces
{
    public interface IFrameworkTargetAnalyzer
    {
        public FrameworkTargetModel Analyze(TargetFrameworkInformation targetFrameworkInformation, LockFileTarget lockFileTarget);
    }
}
