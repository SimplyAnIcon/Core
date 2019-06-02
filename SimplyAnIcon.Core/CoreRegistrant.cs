using Com.Ericmas001.DependencyInjection.Registrants;
using SimplyAnIcon.Core.Helpers;
using SimplyAnIcon.Core.Helpers.Interfaces;
using SimplyAnIcon.Core.Services;
using SimplyAnIcon.Core.Services.Interfaces;
using SimplyAnIcon.Core.Settings;
using SimplyAnIcon.Core.Settings.Interface;

namespace SimplyAnIcon.Core
{
    /// <inheritdoc />
    public class CoreRegistrant : AbstractRegistrant
    {
        /// <inheritdoc />
        protected override void RegisterEverything()
        {
            RegisterServices();
            RegisterHelpers();
            RegisterSettings();
        }

        private void RegisterHelpers()
        {
            Register<IPluginBasicConfigHelper, EmptyPluginBasicConfigHelper>();
        }

        private void RegisterServices()
        {
            Register<IPluginService, PluginService>();
        }
        private void RegisterSettings()
        {
            Register<IPluginSettings, PluginSettings>();
        }
    }
}
