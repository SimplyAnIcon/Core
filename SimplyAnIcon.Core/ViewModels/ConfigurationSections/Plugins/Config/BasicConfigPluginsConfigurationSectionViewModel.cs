using System.Linq;
using System.Windows.Input;
using Com.Ericmas001.DependencyInjection.Resolvers.Interfaces;
using Com.Ericmas001.Mvvm;
using Com.Ericmas001.Mvvm.Collections;
using SimplyAnIcon.Core.Settings.Interface;
using SimplyAnIcon.Core.ViewModels.ConfigurationItems;
using SimplyAnIcon.Plugins.V1;
using SimplyAnIcon.Plugins.V1.Settings;

namespace SimplyAnIcon.Core.ViewModels.ConfigurationSections.Plugins.Config
{
    /// <summary>
    /// BasicConfigPluginsConfigurationSectionViewModel
    /// </summary>
    public class BasicConfigPluginsConfigurationSectionViewModel : AbstractConfigPluginsConfigurationSectionViewModel
    {
        private readonly IResolverService _resolverService;
        private ICommand _saveCommand;

        /// <summary>
        /// SaveCommand
        /// </summary>
        public ICommand SaveCommand => _saveCommand = _saveCommand ?? new RelayCommand(OnSave, CanSave);

        private bool CanSave()
        {
            return Items.All(x => x.IsValid());
        }

        private void OnSave()
        {
            Items.ToList().ForEach(x =>
            {
                try
                {
                    Plugin.SetConfigurationValue(x.Setting.Key, x.ResultValue);
                }
                catch { }
            });
        }

        /// <summary>
        /// Items
        /// </summary>
        public FastObservableCollection<AbstractConfigurationItemViewModel> Items { get; } = new FastObservableCollection<AbstractConfigurationItemViewModel>();

        /// <summary>
        /// BasicConfigPluginsConfigurationSectionViewModel
        /// </summary>
        public BasicConfigPluginsConfigurationSectionViewModel(IResolverService resolverService, IPluginSettings pluginSettings) : base(pluginSettings)
        {
            _resolverService = resolverService;
        }

        /// <summary>
        /// OnInit
        /// </summary>
        public void OnInit(ISimplyAPlugin plugin)
        {
            OnInternalInit(plugin);
            foreach (var it in plugin.ConfigurationItems)
            {
                try
                {
                    var defaultValue = plugin.GetConfigurationValue(it.Key);
                    switch (it)
                    {
                        case BoolSettingValue boolIt:
                        {
                            var itVm = _resolverService.Resolve<BoolConfigurationItemViewModel>();
                            itVm.OnInit(boolIt, defaultValue);
                            Items.Add(itVm);
                            break;
                        }
                        case StringListSettingValue listIt:
                        {
                            var itVm = _resolverService.Resolve<StringListConfigurationItemViewModel>();
                            itVm.OnInit(listIt, defaultValue);
                            Items.Add(itVm);
                            break;
                        }
                        case StringSettingValue strIt:
                        {
                            var itVm = _resolverService.Resolve<StringConfigurationItemViewModel>();
                            itVm.OnInit(strIt, defaultValue);
                            Items.Add(itVm);
                            break;
                        }
                        case IntSettingValue intIt:
                        {
                            var itVm = _resolverService.Resolve<IntConfigurationItemViewModel>();
                            itVm.OnInit(intIt, defaultValue);
                            Items.Add(itVm);
                            break;
                        }
                    }
                }
                catch { }
            }
        }
    }
}
