using System.Windows.Controls;
using SimplyAnIcon.Core.Settings.Interface;
using SimplyAnIcon.Plugins.Wpf.V1;

namespace SimplyAnIcon.Core.ViewModels.ConfigurationSections.Plugins.Config
{
    /// <summary>
    /// SpecialWpfConfigPluginsConfigurationSectionViewModel
    /// </summary>
    public class SpecialWpfConfigPluginsConfigurationSectionViewModel : AbstractConfigPluginsConfigurationSectionViewModel
    {
        /// <summary>
        /// SpecialControl
        /// </summary>
        public UserControl SpecialControl { get; private set; }
        /// <summary>
        /// SpecialWpfConfigPluginsConfigurationSectionViewModel
        /// </summary>
        public SpecialWpfConfigPluginsConfigurationSectionViewModel(IPluginSettings pluginSettings) : base(pluginSettings)
        {
        }

        /// <summary>
        /// OnInit
        /// </summary>
        public void OnInit(ISimplyAWpfPlugin plugin)
        {
            OnInternalInit(plugin);
            SpecialControl = plugin.CustomConfigControl;
        }
    }
}
