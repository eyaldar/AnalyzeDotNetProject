using NuGet.ProjectModel;

namespace ProjectDependenciesVisualizer.Engine.Interfaces
{
    public interface IDependencyGraphGenerator
    {
        DependencyGraphSpec GenerateDependencyGraph(string projectPath);
    }
}