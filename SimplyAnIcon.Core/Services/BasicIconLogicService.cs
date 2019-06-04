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
        private readonly IInstanceResolverHelper _instanceResolverHelper;
        private readonly IPluginService _pluginService;
        private readonly IPluginBasicConfigHelper _pluginBasicConfigHelper;
        private readonly IIconConfigHelper _iconConfigHelper;
        private readonly IProcessHelper _processHelper;

        /// <inheritdoc />
        public event EventHandler OnAppExited = delegate { };

        /// <inheritdoc />
        public IEnumerable<PluginInfo> PluginsCatalog { get; private set; }

        /// <summary>
        /// BasicIconLogicService
        /// </summary>
        public BasicIconLogicService(IInstanceResolverHelper instanceResolverHelper, IPluginService pluginService, IPluginBasicConfigHelper pluginBasicConfigHelper, IIconConfigHelper iconConfigHelper, IProcessHelper processHelper)
        {
            _instanceResolverHelper = instanceResolverHelper;
            _pluginService = pluginService;
            _pluginBasicConfigHelper = pluginBasicConfigHelper;
            _iconConfigHelper = iconConfigHelper;
            _processHelper = processHelper;
        }


        /// <inheritdoc />
        public virtual async Task<IEnumerable<MenuItemViewModel>> UpdateIcon()
        {
            if (_iconConfigHelper.IsUpdateActivated())
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
            _processHelper.ExecuteApp(_iconConfigHelper.GetAppPath());
            OnAppExited(this, new EventArgs());
        }

        /// <inheritdoc />
        public virtual void OnDispose() => _pluginService.DisposePlugins(PluginsCatalog);

        /// <summary>
        /// LoadPlugins
        /// </summary>
        protected virtual void LoadPlugins()
        {
            PluginsCatalog = _pluginService.LoadPlugins(PluginsCatalog, GetPluginPaths(), _instanceResolverHelper, CreateRegistrantFinderBuilder(), _pluginBasicConfigHelper.GetForcedPlugins());

            var catalog = PluginsCatalog.ToArray();

            foreach (var resourceDictionary in catalog.Where(x => x.IsNew && x.IsForeground).SelectMany(x => x.ForegroundPlugin.ResourceDictionaries))
                Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);

            _pluginService.ActivateNewPlugins(catalog);
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
            var pluginPath = Path.Combine(Path.GetDirectoryName(_iconConfigHelper.GetAppPath()) ?? throw new NoNullAllowedException(), "Plugins");
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
