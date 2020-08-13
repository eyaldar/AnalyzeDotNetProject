using System.Collections.Generic;
using ProjectDependenciesVisualizer.Engine.Models;

namespace ProjectDependenciesVisualizer.Engine.Interfaces
{
    public interface IProjectsAnalyzer
    {
        IEnumerable<ProjectReferenceModel> Analyze(string projectFilePath);
    }
}