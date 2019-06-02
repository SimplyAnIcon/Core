using System.Collections.Generic;
using System.Linq;
using Com.Ericmas001.DependencyInjection.Resolvers.Interfaces;
using Com.Ericmas001.Mvvm;
using Com.Ericmas001.Mvvm.Collections;
using SimplyAnIcon.Core.Models;
using SimplyAnIcon.Core.ViewModels.ConfigurationSections;
using SimplyAnIcon.Core.ViewModels.Interfaces;

namespace SimplyAnIcon.Core.ViewModels
{
    /// <summary>
    /// AbstractConfigViewModel
    /// </summary>
    public abstract class AbstractConfigViewModel : ViewModelBase
    {
        private readonly IResolverService _resolverService;
        private readonly FastObservableCollection<IConfigurationSectionViewModel> _sections = new FastObservableCollection<IConfigurationSectionViewModel>();
        private string _iconSource;

        private IConfigurationSectionViewModel _selectedSection;
        /// <summary>
        /// Sections
        /// </summary>
        public IEnumerable<IConfigurationSectionViewModel> Sections => _sections;

        /// <summary>
        /// SelectedSection
        /// </summary>
        public IConfigurationSectionViewModel SelectedSection
        {
            get => _selectedSection;
            set => Set(ref _selectedSection, value);
        }

        /// <summary>
        /// IconSource
        /// </summary>
        public string IconSource
        {
            get => _iconSource;
            set => Set(ref _iconSource, value);
        }

        /// <summary>
        /// AbstractConfigViewModel
        /// </summary>
        protected AbstractConfigViewModel(IResolverService resolverService)
        {
            _resolverService = resolverService;
        }

        /// <summary>
        /// OnInit
        /// </summary>
        public void OnInit(IEnumerable<PluginInfo> catalog)
        {
            _sections.AddItems(GenerateSections(catalog).ToList());

            SelectedSection = _sections.First();
        }

        /// <summary>
        /// GenerateSections
        /// </summary>
        protected virtual IEnumerable<IConfigurationSectionViewModel> GenerateSections(IEnumerable<PluginInfo> catalog)
        {
            var plugins = _resolverService.Resolve<PluginsConfigurationSectionViewModel>();
            plugins.OnInit(catalog);
            return new[] { plugins };
        }
    }
}
