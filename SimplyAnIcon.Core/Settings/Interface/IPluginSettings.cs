using System.Collections.Generic;
using SimplyAnIcon.Core.Models;
using SimplyAnIcon.Plugins.V1;

namespace SimplyAnIcon.Core.Settings.Interface
{
    /// <summary>
    /// IPluginSettings
    /// </summary>
    public interface IPluginSettings
    {
        /// <summary>
        /// IsActive
        /// </summary>
        PluginSettingEntry GetPluginSetting(ISimplyAPlugin plugin);

        /// <summary>
        /// SetActivationStatus
        /// </summary>
        bool SetActivationStatus(ISimplyAPlugin plugin, bool value);

        /// <summary>
        /// AddPlugin
        /// </summary>
        void AddPlugin(ISimplyAPlugin plugin);

        /// <summary>
        /// MoveOrderUp
        /// </summary>
        void MoveOrderUp(ISimplyAPlugin plugin);

        /// <summary>
        /// MoveOrderDown
        /// </summary>
        void MoveOrderDown(ISimplyAPlugin plugin);

        /// <summary>
        /// GetPlugins
        /// </summary>
        IEnumerable<PluginSettingEntry> GetPlugins();

        /// <summary>
        /// GetPluginName
        /// </summary>
        string GetPluginName(ISimplyAPlugin plugin);
    }
}
