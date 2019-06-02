using System.Collections.Generic;
using System.IO;
using System.Linq;
using SimplyAnIcon.Core.Helpers.Interfaces;
using SimplyAnIcon.Core.Models;
using SimplyAnIcon.Core.Settings.Interface;
using SimplyAnIcon.Plugins.V1;
using SimplyAnIcon.Plugins.Wpf.V1;

namespace SimplyAnIcon.Core.Settings
{
    /// <summary>
    /// PluginSettings
    /// </summary>
    public class PluginSettings : IPluginSettings
    {
        private readonly IWindowsHelper _windowsHelper;
        private readonly IJsonHelper _jsonHelper;
        private readonly IPluginBasicConfigHelper _pluginBasicConfigHelper;

        /// <summary>
        /// PluginSettings
        /// </summary>
        public PluginSettings(IWindowsHelper windowsHelper, IJsonHelper jsonHelper, IPluginBasicConfigHelper pluginBasicConfigHelper)
        {
            _windowsHelper = windowsHelper;
            _jsonHelper = jsonHelper;
            _pluginBasicConfigHelper = pluginBasicConfigHelper;
        }

        /// <inheritdoc />
        public PluginSettingEntry GetPluginSetting(ISimplyAPlugin plugin) => LoadPluginSettings().SingleOrDefault(p => p.Name == GetPluginName(plugin));

        /// <inheritdoc />
        public bool SetActivationStatus(ISimplyAPlugin plugin, bool value)
        {
            var plugins = LoadPluginSettings().ToArray();

            var entry = plugins.SingleOrDefault(p => p.Name == GetPluginName(plugin));

            if (entry == null)
                return value;

            var newValue = value || _pluginBasicConfigHelper.GetForcedPlugins().Contains(entry.Name);

            if (newValue != entry.IsActive)
            {
                if (newValue)
                {
                    try
                    {
                        plugin.OnActivation();
                    }
                    catch
                    {
                        newValue = false;
                    }
                }
                else
                {
                    try
                    {
                        plugin.OnDeactivation();
                    }
                    catch
                    {
                        //Just too bad !
                    }
                }

                entry.IsActive = newValue;
            }

            SavePluginSettings(plugins);
            return newValue;
        }

        /// <inheritdoc />
        public void MoveOrderUp(ISimplyAPlugin plugin)
        {
            var plugins = LoadPluginSettings().OrderBy(x => x.Order).ToList();

            var entry = plugins.SingleOrDefault(p => p.Name == GetPluginName(plugin));

            if (entry == null)
                return;

            var index = plugins.IndexOf(entry);

            if (index <= 0)
                return;

            var otherEntry = plugins[index - 1];
            var tmp = entry.Order;
            entry.Order = otherEntry.Order;
            otherEntry.Order = tmp;

            SavePluginSettings(plugins);
        }

        /// <inheritdoc />
        public void MoveOrderDown(ISimplyAPlugin plugin)
        {
            var plugins = LoadPluginSettings().OrderBy(x => x.Order).ToList();

            var entry = plugins.SingleOrDefault(p => p.Name == GetPluginName(plugin));

            if (entry == null)
                return;

            var index = plugins.IndexOf(entry);

            if (index < 0 || index == plugins.Count - 1)
                return;

            var otherEntry = plugins[index + 1];
            var tmp = entry.Order;
            entry.Order = otherEntry.Order;
            otherEntry.Order = tmp;

            SavePluginSettings(plugins);
        }

        /// <inheritdoc />
        public void AddPlugin(ISimplyAPlugin plugin)
        {
            var plugins = LoadPluginSettings().ToList();
            var entry = new PluginSettingEntry
            {
                Name = GetPluginName(plugin),
                IsActive = false,
                Order = plugin is ISimplyAWpfPlugin ? (plugins.Any() ? plugins.Max(x => x.Order) + 1 : 0) : -1
            };
            plugins.Add(entry);
            SavePluginSettings(plugins);
        }

        /// <summary>
        /// GetPluginName
        /// </summary>
        public string GetPluginName(ISimplyAPlugin plugin)
        {
            return plugin.GetType().Assembly.GetName().Name + ";" + plugin.GetType().FullName;
        }

        /// <inheritdoc />
        public IEnumerable<PluginSettingEntry> GetPlugins()
        {
            return LoadPluginSettings().ToArray();
        }

        private IEnumerable<PluginSettingEntry> LoadPluginSettings()
        {
            var fi = new FileInfo(Path.Combine(_windowsHelper.AppRoamingDataPath(), nameof(PluginSettings) + ".json"));
            if (!fi.Exists)
                return new PluginSettingEntry[0];

            return _jsonHelper.DeserializeFile<IEnumerable<PluginSettingEntry>>(fi.FullName);
        }

        private void SavePluginSettings(IEnumerable<PluginSettingEntry> plugins)
        {
            _jsonHelper.SerializeToFile(plugins, Path.Combine(_windowsHelper.AppRoamingDataPath(), nameof(PluginSettings) + ".json"));
        }
    }
}
