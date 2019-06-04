using System.Collections.Generic;
using SimplyAnIcon.Core.Models;

namespace SimplyAnIcon.Core.ViewModels.Interfaces
{
    /// <summary>
    /// IConfigViewModel
    /// </summary>
    public interface IConfigViewModel
    {
        /// <summary>
        /// Sections
        /// </summary>
        IEnumerable<IConfigurationSectionViewModel> Sections { get; }

        /// <summary>
        /// SelectedSection
        /// </summary>
        IConfigurationSectionViewModel SelectedSection { get; set; }

        /// <summary>
        /// IconSource
        /// </summary>
        string IconSource { get; set; }

        /// <summary>
        /// OnInit
        /// </summary>
        void OnInit(IEnumerable<PluginInfo> catalog);
    }
}
