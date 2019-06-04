using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimplyAnIcon.Core.Models;
using SimplyAnIcon.Plugins.Wpf.V1.MenuItemViewModels;

namespace SimplyAnIcon.Core.Services.Interfaces
{
    /// <summary>
    /// IIconLogicService
    /// </summary>
    public interface IIconLogicService
    {
        /// <summary>
        /// OnAppExited
        /// </summary>
        event EventHandler OnAppExited;

        /// <summary>
        /// PluginsCatalog
        /// </summary>
        IEnumerable<PluginInfo> PluginsCatalog { get; }

        /// <summary>
        /// UpdateIcon
        /// </summary>
        Task<IEnumerable<MenuItemViewModel>> UpdateIcon();

        /// <summary>
        /// Restart
        /// </summary>
        void Restart();

        /// <summary>
        /// OnDispose
        /// </summary>
        void OnDispose();
    }
}
