using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Com.Ericmas001.DependencyInjection.RegistrantFinders;
using SimplyAnIcon.Core.Helpers.Interfaces;
using SimplyAnIcon.Core.Models;
using SimplyAnIcon.Core.Services.Interfaces;
using SimplyAnIcon.Core.Settings.Interface;
using SimplyAnIcon.Plugins.V1;

namespace SimplyAnIcon.Core.Services
{
    /// <summary>
    /// PluginService
    /// </summary>
    public class PluginService : IPluginService
    {
        private readonly IPluginSettings _pluginSettings;
        private readonly IPluginBasicConfigHelper _pluginBasicConfigHelper;

        /// <summary>
        /// PluginService
        /// </summary>
        public PluginService(IPluginSettings pluginSettings, IPluginBasicConfigHelper pluginBasicConfigHelper)
        {
            _pluginSettings = pluginSettings;
            _pluginBasicConfigHelper = pluginBasicConfigHelper;
        }

        /// <inheritdoc />
        public void ActivateNewPlugins(IEnumerable<PluginInfo> currentCatalog)
        {
            if (currentCatalog == null)
                return;

            foreach (var plugin in currentCatalog.Where(x => x.IsNew && x.IsActivated))
            {
                try
                {
                    plugin.Plugin.OnActivation();
                }
                catch
                {
                    plugin.IsActivated = false;
                }
            }
        }

        /// <inheritdoc />
        public void DisposePlugins(IEnumerable<PluginInfo> currentCatalog)
        {
            if (currentCatalog == null)
                return;

            foreach (var plugin in currentCatalog)
            {
                if (plugin.IsActivated)
                {
                    try
                    {
                        plugin.Plugin.OnDeactivation();
                    }
                    catch
                    {
                        //Just too bad !
                    }
                }

                try
                {
                    plugin.Plugin.OnDispose();
                }
                catch
                {
                    //Just too bad !
                }
            }
        }

        /// <inheritdoc />
        public IEnumerable<PluginInfo> LoadPlugins(IEnumerable<PluginInfo> currentCatalog, IEnumerable<string> pluginPaths, IInstanceResolverHelper resolverHelper, RegistrantFinderBuilder registrantFinderBuilder = null, IEnumerable<string> forcedPlugins = null)
        {
            var registrantBuilder = registrantFinderBuilder ?? new RegistrantFinderBuilder();

            var forced = forcedPlugins?.ToArray() ?? new string[0];
            var catalog = currentCatalog?.ToArray() ?? new PluginInfo[0];
            var dirs = pluginPaths.Select(x => new DirectoryInfo(x)).Where(x => x.Exists);
            var excludedPrefix = new[]
            {
                "System.",
                "Microsoft.",
                "SimplyAnIcon.Plugins",
                "SimplyAnIcon.Core",
                "netstandard.dll"
            };

            var dlls = dirs.SelectMany(dir => dir.GetFiles("*.dll", SearchOption.AllDirectories))
                .Where(d => excludedPrefix.All(x => !d.Name.StartsWith(x))).ToArray();

            if (!dlls.Any())
                return catalog;

            var assemblies = dlls.Select(x => Assembly.LoadFile(x.FullName)).ToList();
            assemblies.ForEach(x => registrantBuilder.AddAssembly(x));

            var pTypes = assemblies.SelectMany(x =>
                    x.DefinedTypes.Where(p =>
                        p.IsClass && !p.IsAbstract && typeof(ISimplyAPlugin).IsAssignableFrom(p)))
                .ToArray();

            resolverHelper.EverythingIsRegistered(registrantBuilder.Build().GetAllRegistrations());

            var plugins = pTypes
                .Select(resolverHelper.Resolve)
                .Cast<ISimplyAPlugin>()
                .ToArray();

            var pluginSettings = _pluginSettings.GetPlugins().ToArray();

            foreach (var plugin in plugins.Where(p => pluginSettings.All(x => x.Name != _pluginSettings.GetPluginName(p))))
                _pluginSettings.AddPlugin(plugin);

            var newCatalog = plugins
                .Select(x => new { Plugin = x, Setting = _pluginSettings.GetPluginSetting(x) })
                .OrderBy(x => x.Setting?.Order ?? -1)
                .Select(x => new PluginInfo
                {
                    Plugin = catalog.FirstOrDefault(o => o.Plugin.Name == x.Plugin.Name)?.Plugin ?? x.Plugin,
                    IsActivated = forced.Contains(x.Setting?.Name) || (x.Setting?.IsActive ?? false),
                    IsNew = catalog.All(o => o.Plugin.Name != x.Plugin.Name)
                })
                .ToArray();

            foreach (var plugin in newCatalog.Where(x => x.IsNew).ToArray())
            {
                try
                {
                    plugin.Plugin.OnInit(_pluginBasicConfigHelper.GetPluginBasicConfig());
                }
                catch
                {
                    newCatalog = newCatalog.Except(new[] { plugin }).ToArray();
                }
            }

            return newCatalog;
        }
    }
}
