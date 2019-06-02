using System.Collections.Generic;

namespace SimplyAnIcon.Core.Helpers.Interfaces
{
    /// <summary>
    /// IPluginBasicConfigHelper
    /// </summary>
    public interface IPluginBasicConfigHelper
    {
        /// <summary>
        /// GetPluginBasicConfig
        /// </summary>
        Dictionary<string, object> GetPluginBasicConfig();

        /// <summary>
        /// GetForcedPlugins
        /// </summary>
        IEnumerable<string> GetForcedPlugins();
    }
}
