using ProjectDependenciesVisualizer.Model;

namespace ProjectDependenciesVisualizer.Engine.Interfaces
{
    public interface IRunner
    {
        RunStatus Run(string workingDirectory, string[] arguments);
    }
}