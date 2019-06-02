using System.Collections.Generic;
using Com.Ericmas001.DependencyInjection.RegistrantFinders;
using SimplyAnIcon.Core.Helpers.Interfaces;
using SimplyAnIcon.Core.Models;

namespace SimplyAnIcon.Core.Services.Interfaces
{
    /// <summary>
    /// IPluginService
    /// </summary>
    public interface IPluginService
    {
        /// <summary>
        /// LoadPlugins
        /// </summary>
        IEnumerable<PluginInfo> LoadPlugins(IEnumerable<PluginInfo> currentCatalog, IEnumerable<string> pluginPaths, IInstanceResolverHelper resolverHelper, RegistrantFinderBuilder registrantFinderBuilder = null, IEnumerable<string> forcedPlugins = null);

        /// <summary>
        /// DisposePlugins
        /// </summary>
        void DisposePlugins(IEnumerable<PluginInfo> currentCatalog);

        /// <summary>
        /// ActivateNewPlugins
        /// </summary>
        void ActivateNewPlugins(IEnumerable<PluginInfo> currentCatalog);
    }
}
