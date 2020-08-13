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

namespace ProjectDependenciesVisualizer.WPF
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private IProjectsAnalyzer projectsAnalyzer;

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


        private ObservableCollection<ProjectModel> projects;
        public ObservableCollection<ProjectModel> Projects
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

        public MainWindowViewModel(IProjectsAnalyzer projectsAnalyzer)
        {
            this.projectsAnalyzer = projectsAnalyzer;

            Projects = new ObservableCollection<ProjectModel>();
            BrowseCommand = new RelayCommand(AlwaysTrue, OnBrowseExecuted);
            AnalyzeCommand = new RelayCommand(AlwaysTrue, OnAnalyzeExecuted);
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
            IEnumerable<ProjectModel> projects = await Task.Run(() => projectsAnalyzer.Analyze(ProjectPath));

            LoadData(projects);
        }

        private void LoadData(IEnumerable<ProjectModel> projects)
        {
            foreach (var project in projects)
            {
                Projects.Add(project);
            }
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
