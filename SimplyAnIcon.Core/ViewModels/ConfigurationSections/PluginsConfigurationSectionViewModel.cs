using System.Collections.Generic;
using Com.Ericmas001.DependencyInjection.Resolvers.Interfaces;
using Com.Ericmas001.Mvvm.Collections;
using SimplyAnIcon.Core.Models;
using SimplyAnIcon.Core.ViewModels.ConfigurationSections.Plugins;
using SimplyAnIcon.Core.ViewModels.Interfaces;

namespace SimplyAnIcon.Core.ViewModels.ConfigurationSections
{
    /// <summary>
    /// PluginsConfigurationSectionViewModel
    /// </summary>
    public class PluginsConfigurationSectionViewModel : IConfigurationSectionViewModel
    {
        private readonly IResolverService _resolverService;
        private readonly FastObservableCollection<IConfigurationSectionViewModel> _sections = new FastObservableCollection<IConfigurationSectionViewModel>();

        /// <inheritdoc />
        public string Name => "Plugins";

        /// <inheritdoc />
        public object IconPath => null;

        /// <inheritdoc />
        public IEnumerable<IConfigurationSectionViewModel> ChildrenSections => _sections;

        /// <summary>
        /// PluginsConfigurationSectionViewModel
        /// </summary>
        public PluginsConfigurationSectionViewModel(IResolverService resolverService)
        {
            _resolverService = resolverService;
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnInit(IEnumerable<PluginInfo> catalog)
        {
            var genPlugin = _resolverService.Resolve<GeneralPluginsConfigurationSectionViewModel>();
            genPlugin.OnInit(catalog);
            _sections.Add(genPlugin);

            var plugins = _resolverService.Resolve<SpecificPluginsConfigurationSectionViewModel>();
            plugins.OnInit(catalog);
            _sections.Add(plugins);
        }
    }
}
