using Com.Ericmas001.DependencyInjection.Registrants;
using SimplyAnIcon.Core.Helpers;
using SimplyAnIcon.Core.Helpers.Interfaces;
using SimplyAnIcon.Core.Services;
using SimplyAnIcon.Core.Services.Interfaces;
using SimplyAnIcon.Core.Settings;
using SimplyAnIcon.Core.Settings.Interface;
using SimplyAnIcon.Core.ViewModels;
using SimplyAnIcon.Core.ViewModels.Interfaces;

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
            RegisterViewModels();
        }

        private void RegisterHelpers()
        {
            Register<IPluginBasicConfigHelper, EmptyPluginBasicConfigHelper>();
            Register<IIconConfigHelper, DefaultIconConfigHelper>();
        }

        private void RegisterServices()
        {
            Register<IIconLogicService, BasicIconLogicService>();
            Register<IPluginService, PluginService>();
        }
        private void RegisterSettings()
        {
            Register<IPluginSettings, PluginSettings>();
        }
        private void RegisterViewModels()
        {
            Register<IConfigViewModel, BasicConfigViewModel>();
            Register<IViewModelFactory>(rsvl => new ViewModelFactory(rsvl.Resolve<IConfigViewModel>));
        }
    }
}
