using System.Threading.Tasks;
using Com.Ericmas001.Mvvm.Collections;
using SimplyAnIcon.Plugins.Wpf.V1.MenuItemViewModels;

namespace SimplyAnIcon.Core.ViewModels.Interfaces
{
    /// <summary>
    /// ISimplyAnIconViewModel
    /// </summary>
    public interface ISimplyAnIconViewModel
    {
        /// <summary>
        /// Items
        /// </summary>
        FastObservableCollection<MenuItemViewModel> Items { get; }

        /// <summary>
        /// IsVisible
        /// </summary>
        bool IsVisible { get; set; }

        /// <summary>
        /// IconSource
        /// </summary>
        string IconSource { get; }

        /// <summary>
        /// IconName
        /// </summary>
        string IconName { get; }

        /// <summary>
        /// StayOpen
        /// </summary>
        bool StayOpen { get; }

        /// <summary>
        /// LoadIcon
        /// </summary>
        Task LoadIcon();
    }
}
