using System.Collections.Generic;
using SimplyAnIcon.Core.Helpers.Interfaces;

namespace SimplyAnIcon.Core.Helpers
{
    /// <summary>
    /// EmptyPluginBasicConfigHelper
    /// </summary>
    public class EmptyPluginBasicConfigHelper : IPluginBasicConfigHelper
    {
        /// <inheritdoc />
        public IEnumerable<string> GetForcedPlugins()
        {
            return new string[0];
        }

        /// <inheritdoc />
        public Dictionary<string, object> GetPluginBasicConfig()
        {
            return new Dictionary<string, object>();
        }
    }
}
