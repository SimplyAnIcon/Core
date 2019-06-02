using SimplyAnIcon.Plugins.V1;
using SimplyAnIcon.Plugins.Wpf.V1;

namespace SimplyAnIcon.Core.Models
{
    /// <summary>
    /// PluginInfo
    /// </summary>
    public class PluginInfo
    {
        /// <summary>
        /// Plugin
        /// </summary>
        public ISimplyAPlugin Plugin { get; set; }

        /// <summary>
        /// IsActivated
        /// </summary>
        public bool IsActivated { get; set; }

        /// <summary>
        /// IsNew
        /// </summary>
        public bool IsNew { get; set; }

        /// <summary>
        /// ForegroundPlugin
        /// </summary>
        public ISimplyAWpfPlugin ForegroundPlugin => Plugin as ISimplyAWpfPlugin;

        /// <summary>
        /// IsForeground
        /// </summary>
        public bool IsForeground => ForegroundPlugin != null;
    }
}
