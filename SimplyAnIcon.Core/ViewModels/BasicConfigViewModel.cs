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
    /// BasicConfigViewModel
    /// </summary>
    public class BasicConfigViewModel : ViewModelBase, IConfigViewModel
    {
        /// <summary>
        /// ResolverService
        /// </summary>
        protected readonly IResolverService ResolverService;

        private readonly FastObservableCollection<IConfigurationSectionViewModel> _sections = new FastObservableCollection<IConfigurationSectionViewModel>();
        private string _iconSource;

        private IConfigurationSectionViewModel _selectedSection;

        /// <inheritdoc />
        public IEnumerable<IConfigurationSectionViewModel> Sections => _sections;

        /// <inheritdoc />
        public IConfigurationSectionViewModel SelectedSection
        {
            get => _selectedSection;
            set => Set(ref _selectedSection, value);
        }

        /// <inheritdoc />
        public string IconSource
        {
            get => _iconSource;
            set => Set(ref _iconSource, value);
        }

        /// <summary>
        /// BasicConfigViewModel
        /// </summary>
        public BasicConfigViewModel(IResolverService resolverService)
        {
            ResolverService = resolverService;
        }

        /// <inheritdoc />
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
            var plugins = ResolverService.Resolve<PluginsConfigurationSectionViewModel>();
            plugins.OnInit(catalog);
            return new[] { plugins };
        }
    }
}
