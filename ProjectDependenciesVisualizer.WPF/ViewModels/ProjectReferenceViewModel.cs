using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using ProjectDependenciesVisualizer.Engine.Models;

namespace ProjectDependenciesVisualizer.WPF.ViewModels
{
    public class ProjectReferenceViewModel : INotifyPropertyChanged
    {
        public ProjectReferenceModel Data { get; }

        public IEnumerable<ProjectReferenceViewModel> Dependencies { get; }

        public ProjectReferenceViewModel(ProjectReferenceModel data, IEnumerable<ProjectReferenceViewModel> dependencies)
        {
            Data = data;
            Dependencies = dependencies;
        }

        private bool matchSearch = true;
        public bool MatchSearch
        {
            get
            {
                return matchSearch;
            }
            set
            {
                if (matchSearch != value)
                {
                    matchSearch = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool ChildrenMatchSearch
        { 
            get
            {
                return Dependencies.Any(x => x.MatchSearch);
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
