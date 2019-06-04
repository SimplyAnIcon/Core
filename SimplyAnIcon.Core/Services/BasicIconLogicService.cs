using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Com.Ericmas001.DependencyInjection.RegistrantFinders;
using SimplyAnIcon.Common.Helpers.Interfaces;
using SimplyAnIcon.Core.Helpers.Interfaces;
using SimplyAnIcon.Core.Models;
using SimplyAnIcon.Core.Services.Interfaces;
using SimplyAnIcon.Plugins.Wpf.V1.MenuItemViewModels;

namespace SimplyAnIcon.Core.Services
{
    /// <summary>
    /// BasicIconLogicService
    /// </summary>
    public class BasicIconLogicService : IIconLogicService
    {
        /// <summary>
        /// InstanceResolverHelper
        /// </summary>
        protected readonly IInstanceResolverHelper InstanceResolverHelper;

        /// <summary>
        /// PluginService
        /// </summary>
        protected readonly IPluginService PluginService;

        /// <summary>
        /// PluginBasicConfigHelper
        /// </summary>
        protected readonly IPluginBasicConfigHelper PluginBasicConfigHelper;

        /// <summary>
        /// IconConfigHelper
        /// </summary>
        protected readonly IIconConfigHelper IconConfigHelper;

        /// <summary>
        /// ProcessHelper
        /// </summary>
        protected readonly IProcessHelper ProcessHelper;

        /// <inheritdoc />
        public event EventHandler OnAppExited = delegate { };

        /// <inheritdoc />
        public IEnumerable<PluginInfo> PluginsCatalog { get; private set; }

        /// <summary>
        /// BasicIconLogicService
        /// </summary>
        public BasicIconLogicService(IInstanceResolverHelper instanceResolverHelper, IPluginService pluginService, IPluginBasicConfigHelper pluginBasicConfigHelper, IIconConfigHelper iconConfigHelper, IProcessHelper processHelper)
        {
            InstanceResolverHelper = instanceResolverHelper;
            PluginService = pluginService;
            PluginBasicConfigHelper = pluginBasicConfigHelper;
            IconConfigHelper = iconConfigHelper;
            ProcessHelper = processHelper;
        }


        /// <inheritdoc />
        public virtual async Task<IEnumerable<MenuItemViewModel>> UpdateIcon()
        {
            if (IconConfigHelper.IsUpdateActivated())
            {
                await OnUpdate();
                return FinalizeUpdateIcon();
            }

            return ReloadEverything();
        }

        /// <summary>
        /// OnUpdate
        /// </summary>
        protected virtual async Task OnUpdate() => await Task.FromResult(true);

        /// <summary>
        /// ReloadEverything
        /// </summary>
        protected virtual IEnumerable<MenuItemViewModel> ReloadEverything()
        {
            LoadPlugins();
            PluginsCatalog.Where(x => x.IsActivated).ToList().ForEach(x =>
            {
                try
                {
                    x.Plugin.OnRefresh();
                }
                catch
                {
                    //Just too bad !
                }
            });
            return BuildMenu();
        }

        /// <summary>
        /// FinalizeUpdateIcon
        /// </summary>
        protected virtual IEnumerable<MenuItemViewModel> FinalizeUpdateIcon() => ReloadEverything();

        /// <inheritdoc />
        public virtual void Restart()
        {
            ProcessHelper.ExecuteApp(IconConfigHelper.GetAppPath());
            ExitApp();
        }

        /// <summary>
        /// ExitApp
        /// </summary>
        protected virtual void ExitApp()
        {
            OnAppExited(this, new EventArgs());
        }

        /// <inheritdoc />
        public virtual void OnDispose() => PluginService.DisposePlugins(PluginsCatalog);

        /// <summary>
        /// LoadPlugins
        /// </summary>
        protected virtual void LoadPlugins()
        {
            PluginsCatalog = PluginService.LoadPlugins(PluginsCatalog, GetPluginPaths(), InstanceResolverHelper, CreateRegistrantFinderBuilder(), PluginBasicConfigHelper.GetForcedPlugins());

            var catalog = PluginsCatalog.ToArray();

            foreach (var resourceDictionary in catalog.Where(x => x.IsNew && x.IsForeground).SelectMany(x => x.ForegroundPlugin.ResourceDictionaries))
                Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);

            PluginService.ActivateNewPlugins(catalog);
        }

        /// <summary>
        /// CreateRegistrantFinderBuilder
        /// </summary>
        protected virtual RegistrantFinderBuilder CreateRegistrantFinderBuilder() => new RegistrantFinderBuilder().AddAssemblyPrefix("SimplyAnIcon");

        /// <summary>
        /// GetPluginPaths
        /// </summary>
        protected virtual IEnumerable<string> GetPluginPaths()
        {
            var pluginPath = Path.Combine(Path.GetDirectoryName(IconConfigHelper.GetAppPath()) ?? throw new NoNullAllowedException(), "Plugins");
            if (!Directory.Exists(pluginPath))
                Directory.CreateDirectory(pluginPath);
            return new[] { pluginPath };
        }

        /// <summary>
        /// BuildMenu
        /// </summary>
        protected virtual IEnumerable<MenuItemViewModel> BuildMenu()
        {
            var items = new List<MenuItemViewModel>();
            var itemsOfPlugin = PluginsCatalog.Where(x => x.IsActivated && x.IsForeground).Select(x => x.ForegroundPlugin.MenuItems).ToList();
            var first = true;
            foreach (var it in itemsOfPlugin)
            {
                if (first)
                    first = false;
                else
                    items.Add(new SeparatorMenuItemViewModel(null));
                items.AddRange(it.ToList());
            }

            return items;
        }
    }
}
