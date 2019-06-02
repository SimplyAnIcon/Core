using SimplyAnIcon.Core.Settings.Interface;
using SimplyAnIcon.Plugins.V1;

namespace SimplyAnIcon.Core.ViewModels.ConfigurationSections.Plugins.Config
{
    /// <summary>
    /// NoConfigPluginsConfigurationSectionViewModel
    /// </summary>
    public class NoConfigPluginsConfigurationSectionViewModel : AbstractConfigPluginsConfigurationSectionViewModel
    {
        /// <summary>
        /// NoConfigPluginsConfigurationSectionViewModel
        /// </summary>
        public NoConfigPluginsConfigurationSectionViewModel(IPluginSettings pluginSettings) : base(pluginSettings)
        {
        }

        /// <summary>
        /// OnInit
        /// </summary>
        public void OnInit(ISimplyAPlugin plugin)
        {
            OnInternalInit(plugin);
        }
    }
}
