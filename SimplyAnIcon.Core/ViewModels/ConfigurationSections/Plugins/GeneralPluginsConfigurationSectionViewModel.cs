using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Com.Ericmas001.Mvvm;
using Com.Ericmas001.Mvvm.Collections;
using SimplyAnIcon.Core.Models;
using SimplyAnIcon.Core.Settings.Interface;
using SimplyAnIcon.Core.ViewModels.Interfaces;
using SimplyAnIcon.Plugins.Wpf.V1;

namespace SimplyAnIcon.Core.ViewModels.ConfigurationSections.Plugins
{
    /// <summary>
    /// GeneralPluginsConfigurationSectionViewModel
    /// </summary>
    public class GeneralPluginsConfigurationSectionViewModel : ViewModelBase, IConfigurationSectionViewModel
    {
        private readonly IPluginSettings _pluginSettings;

        private ISimplyAWpfPlugin _selectedPlugin;

        /// <inheritdoc />
        public string Name => "General";

        /// <inheritdoc />
        public object IconPath => null;

        /// <summary>
        /// Plugins
        /// </summary>
        public FastObservableCollection<ISimplyAWpfPlugin> Plugins { get; } =
            new FastObservableCollection<ISimplyAWpfPlugin>();

        /// <summary>
        /// SelectedPlugin
        /// </summary>
        public ISimplyAWpfPlugin SelectedPlugin
        {
            get => _selectedPlugin;
            set => Set(ref _selectedPlugin, value);
        }

        private ICommand _upCommand;
        private ICommand _downCommand;

        /// <summary>
        /// GeneralPluginsConfigurationSectionViewModel
        /// </summary>
        public GeneralPluginsConfigurationSectionViewModel(IPluginSettings pluginSettings)
        {
            _pluginSettings = pluginSettings;
        }

        /// <summary>
        /// UpCommand
        /// </summary>
        public ICommand UpCommand =>
            _upCommand = _upCommand ?? new RelayCommand(MoveUp, () => Plugins.IndexOf(SelectedPlugin) > 0);

        /// <summary>
        /// UpCommand
        /// </summary>
        public ICommand DownCommand =>
            _downCommand = _downCommand ?? new RelayCommand(MoveDown, () => Plugins.IndexOf(SelectedPlugin) >= 0 && Plugins.IndexOf(SelectedPlugin) < Plugins.Count - 1);

        /// <inheritdoc />
        public IEnumerable<IConfigurationSectionViewModel> ChildrenSections => new IConfigurationSectionViewModel[0];

        /// <summary>
        /// OnInit
        /// </summary>
        public void OnInit(IEnumerable<PluginInfo> catalog)
        {
            Plugins.AddItems(catalog.Where(x => x.IsActivated && x.IsForeground).Select(x => x.ForegroundPlugin).ToList());
        }

        private void MoveUp()
        {
            var i = Plugins.IndexOf(SelectedPlugin);
            var p = SelectedPlugin;
            Plugins.Remove(p);
            _pluginSettings.MoveOrderUp(p);
            Plugins.Insert(i - 1, p);
            SelectedPlugin = p;
        }
        private void MoveDown()
        {
            var i = Plugins.IndexOf(SelectedPlugin);
            var p = SelectedPlugin;
            Plugins.Remove(p);
            _pluginSettings.MoveOrderDown(p);
            Plugins.Insert(i + 1, p);
            SelectedPlugin = p;
        }
    }
}
