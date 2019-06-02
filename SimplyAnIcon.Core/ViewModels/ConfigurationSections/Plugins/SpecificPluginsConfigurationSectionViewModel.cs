using System.Collections.Generic;
using System.Linq;
using Com.Ericmas001.DependencyInjection.Resolvers.Interfaces;
using Com.Ericmas001.Mvvm.Collections;
using SimplyAnIcon.Core.Models;
using SimplyAnIcon.Core.ViewModels.ConfigurationSections.Plugins.Config;
using SimplyAnIcon.Core.ViewModels.Interfaces;
using SimplyAnIcon.Plugins.Wpf.V1;

namespace SimplyAnIcon.Core.ViewModels.ConfigurationSections.Plugins
{
    /// <summary>
    /// SpecificPluginsConfigurationSectionViewModel
    /// </summary>
    public class SpecificPluginsConfigurationSectionViewModel : IConfigurationSectionViewModel
    {
        private readonly IResolverService _resolverService;
        private readonly FastObservableCollection<IConfigurationSectionViewModel> _sections = new FastObservableCollection<IConfigurationSectionViewModel>();

        /// <inheritdoc />
        public string Name => "Plugins Configuration";

        /// <inheritdoc />
        public object IconPath => null;

        /// <inheritdoc />
        public IEnumerable<IConfigurationSectionViewModel> ChildrenSections => _sections;

        /// <summary>
        /// SpecificPluginsConfigurationSectionViewModel
        /// </summary>
        public SpecificPluginsConfigurationSectionViewModel(IResolverService resolverService)
        {
            _resolverService = resolverService;
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnInit(IEnumerable<PluginInfo> catalog)
        {
            foreach (var plugin in catalog.Select(x => x.Plugin).OrderBy(x => x.Name))
            {
                if (plugin is ISimplyAWpfPlugin wpfPlugin && wpfPlugin.CustomConfigControl != null)
                {
                    var section = _resolverService.Resolve<SpecialWpfConfigPluginsConfigurationSectionViewModel>();
                    section.OnInit(wpfPlugin);
                    _sections.Add(section);
                }
                else if (plugin.ConfigurationItems != null && plugin.ConfigurationItems.Any())
                {
                    var section = _resolverService.Resolve<BasicConfigPluginsConfigurationSectionViewModel>();
                    section.OnInit(plugin);
                    _sections.Add(section);
                }
                else
                {
                    var section = _resolverService.Resolve<NoConfigPluginsConfigurationSectionViewModel>();
                    section.OnInit(plugin);
                    _sections.Add(section);
                }
            }
        }
    }
}
