using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using ProjectDependenciesVisualizer.Engine.Interfaces;
using ProjectDependenciesVisualizer.Engine.Models;
using ProjectDependenciesVisualizer.WPF.ViewModels;

namespace ProjectDependenciesVisualizer.WPF
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private IProjectsAnalyzer projectsAnalyzer;

        public ICommand SearchCommand { get; set; }
        public ICommand BrowseCommand { get; set; }
        public ICommand AnalyzeCommand { get; set; }

        private string projectPath;
        public string ProjectPath
        {
            get
            {
                return projectPath;
            }
            set
            {
                if (projectPath != value)
                {
                    projectPath = value;
                    OnPropertyChanged();
                }
            }
        }


        private ObservableCollection<ProjectReferenceViewModel> projects;
        public ObservableCollection<ProjectReferenceViewModel> Projects
        {
            get
            {
                return projects;
            }
            set
            {
                if (projects != value)
                {
                    projects = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isProcessing;
        public bool IsProcessing
        {
            get
            {
                return isProcessing;
            }
            set
            {
                if (isProcessing != value)
                {
                    isProcessing = value;
                    OnPropertyChanged();
                }
            }
        }

        private string searchTerm;
        public string SearchTerm
        {
            get
            {
                return searchTerm;
            }
            set
            {
                if (searchTerm != value)
                {
                    searchTerm = value;
                    OnPropertyChanged();
                }
            }
        }

        public MainWindowViewModel(IProjectsAnalyzer projectsAnalyzer)
        {
            this.projectsAnalyzer = projectsAnalyzer;

            Projects = new ObservableCollection<ProjectReferenceViewModel>();
            SearchCommand = new RelayCommand(AlwaysTrue, OnSearchExecuted);
            BrowseCommand = new RelayCommand(AlwaysTrue, OnBrowseExecuted);
            AnalyzeCommand = new RelayCommand(AlwaysTrue, OnAnalyzeExecuted);
        }

        private void OnSearchExecuted(object obj)
        {
            foreach (var projectReference in Projects)
            {
                SearchReferencesByName(projectReference);
            }
        }

        private void SearchReferencesByName(ProjectReferenceViewModel projectReference)
        {
            foreach (var dependency in projectReference.Dependencies)
            {
                SearchReferencesByName(dependency);
            }

            projectReference.MatchSearch = string.IsNullOrEmpty(SearchTerm)     || 
                                           projectReference.ChildrenMatchSearch || 
                                           projectReference.Data.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase);
        }

        private void OnBrowseExecuted(object obj)
        {
            var openDlg = new OpenFileDialog
            {
                Title = "Choose VS project or solution",
                CheckFileExists = false,
                CheckPathExists = false,
                Filter = "solution or project files (*.sln, *.csproj)|*.sln;*.csproj",
                Multiselect = false
            };

            var dialogResult = openDlg.ShowDialog();

            if(dialogResult.HasValue && dialogResult.Value)
            {
                ProjectPath = openDlg.FileName;
            }
        }

        private bool AlwaysTrue(object obj)
        {
            return true;
        }

        private async void OnAnalyzeExecuted(object obj)
        {
            IsProcessing = true;

            try
            {

                IEnumerable<ProjectReferenceModel> projects = await Task.Run(() => projectsAnalyzer.Analyze(ProjectPath));

                LoadData(projects);
            }
            catch {}
            finally
            {
                IsProcessing = false;
            }
        }

        private void LoadData(IEnumerable<ProjectReferenceModel> projects)
        {
            foreach (var project in projects)
            {
                ProjectReferenceViewModel viewModel = BuildProjectReferenceViewModel(project);
                Projects.Add(viewModel);
            }
        }

        private ProjectReferenceViewModel BuildProjectReferenceViewModel(ProjectReferenceModel project)
        {
            var projectReferences = new List<ProjectReferenceViewModel>();

            foreach (var dependency in project.Dependencies)
            {
                ProjectReferenceViewModel projectReference = BuildProjectReferenceViewModel(dependency);
                projectReferences.Add(projectReference);
            }

            return new ProjectReferenceViewModel(project, projectReferences);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
