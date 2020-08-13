using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGet.ProjectModel;
using ProjectDependenciesVisualizer.Engine.Interfaces;
using ProjectDependenciesVisualizer.Engine.Models;

namespace ProjectDependenciesVisualizer.Engine
{
    public class ProjectsAnalyzer : IProjectsAnalyzer
    {
        private IDependencyGraphGenerator dependencyGraphGenerator;
        private ISingleProjectAnalyzer singleProjectAnalyzer;

        public ProjectsAnalyzer(IDependencyGraphGenerator dependencyGraphGenerator,
                                ISingleProjectAnalyzer singleProjectAnalyzer)
        {
            this.dependencyGraphGenerator = dependencyGraphGenerator;
            this.singleProjectAnalyzer = singleProjectAnalyzer;
        }

        public IEnumerable<ProjectModel> Analyze(string projectFilePath)
        {
            List<ProjectModel> projects = new List<ProjectModel>();
            DependencyGraphSpec dependencyGraph = dependencyGraphGenerator.GenerateDependencyGraph(projectFilePath);

            foreach (PackageSpec project in GetPackageReferenceSupportedProjects(dependencyGraph))
            {
                var projectModel = singleProjectAnalyzer.Analyze(project);
                if(projectModel != null)
                    projects.Add(projectModel);
            }

            return projects;
        }

        private static IEnumerable<PackageSpec> GetPackageReferenceSupportedProjects(DependencyGraphSpec dependencyGraph)
        {
            return dependencyGraph.Projects.Where(p => p.RestoreMetadata.ProjectStyle == ProjectStyle.PackageReference);
        }
    }
}