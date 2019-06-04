using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Com.Ericmas001.Mvvm;
using Com.Ericmas001.Mvvm.Collections;
using SimplyAnIcon.Core.Helpers.Interfaces;
using SimplyAnIcon.Core.Services.Interfaces;
using SimplyAnIcon.Core.ViewModels.Interfaces;
using SimplyAnIcon.Core.Windows;
using SimplyAnIcon.Plugins.Wpf.V1.MenuItemViewModels;

namespace SimplyAnIcon.Core.ViewModels
{
    /// <summary>
    /// AbstractNotifyIconViewModel
    /// </summary>
    public abstract class AbstractNotifyIconViewModel : ViewModelBase, ISimplyAnIconViewModel
    {
        private bool _isVisible;
        private bool _stayOpen;

        /// <summary>
        /// IsUpdating
        /// </summary>
        protected bool IsUpdating;

        /// <summary>
        /// Logic
        /// </summary>
        protected readonly IIconLogicService Logic;

        /// <summary>
        /// ViewModelFactory
        /// </summary>
        protected readonly IViewModelFactory ViewModelFactory;

        /// <summary>
        /// ConfigWindow
        /// </summary>
        protected ConfigWindow ConfigWindow;

        /// <summary>
        /// PermanentBottomItems
        /// </summary>
        protected readonly List<MenuItemViewModel> PermanentBottomItems;

        /// <summary>
        /// Items
        /// </summary>
        public FastObservableCollection<MenuItemViewModel> Items { get; } = new FastObservableCollection<MenuItemViewModel>();

        /// <summary>
        /// IsVisible
        /// </summary>
        public virtual bool IsVisible
        {
            get => _isVisible;
            set
            {
                Set(ref _isVisible, value);
                RaisePropertyChanged(nameof(IconSource));
                RaisePropertyChanged(nameof(IconName));
                RaisePropertyChanged(nameof(Items));
            }
        }

        /// <summary>
        /// IconSource
        /// </summary>
        public abstract string IconSource { get; }

        /// <summary>
        /// IconName
        /// </summary>
        public abstract string IconName { get; }

        /// <summary>
        /// StayOpen
        /// </summary>
        public virtual bool StayOpen
        {
            get => _stayOpen;
            set => Set(ref _stayOpen, value);
        }

        /// <summary>
        /// AbstractNotifyIconViewModel
        /// </summary>
        protected AbstractNotifyIconViewModel(IIconLogicService logic, IViewModelFactory viewModelFactory, IIconConfigHelper iconConfigHelper)
        {
            Logic = logic;
            ViewModelFactory = viewModelFactory;

            PermanentBottomItems = new List<MenuItemViewModel>
            {
                new SeparatorMenuItemViewModel(null)
            };
            if (iconConfigHelper.IsUpdateActivated())
            {
                PermanentBottomItems.Add(
                    new MenuItemViewModel(null)
                    {
                        Name = "Update",
                        Action = new RelayCommand(async () => await UpdateIcon()),
                        IconPath = Application.Current.Resources["SimplyIconUpdate"]
                    });
            }

            PermanentBottomItems.Add(
                new MenuItemViewModel(null)
                {
                    Name = "Options",
                    Action = new RelayCommand(StartConfigWindow),
                    IconPath = Application.Current.Resources["SimplyIconConfig"]
                });
            PermanentBottomItems.Add(new SeparatorMenuItemViewModel(null));
            PermanentBottomItems.Add(
                new MenuItemViewModel(null)
                {
                    Name = "Restart",
                    Action = new RelayCommand(Logic.Restart),
                    IconPath = Application.Current.Resources["SimplyIconRestart"]
                });
            PermanentBottomItems.Add(
                new MenuItemViewModel(null)
                {
                    Name = "Exit",
                    Action = new RelayCommand(KillIcon),
                    IconPath = Application.Current.Resources["SimplyIconExit"]
                });

            Logic.OnAppExited += (s, e) => KillIcon();
        }

        /// <summary>
        /// LoadIcon
        /// </summary>
        public virtual async Task LoadIcon()
        {
            await UpdateIcon();
        }

        /// <summary>
        /// UpdateIcon
        /// </summary>
        public virtual async Task UpdateIcon()
        {
            if (IsUpdating)
                return;

            IsUpdating = true;
            IsVisible = false;

            try
            {
                var newItems = await Logic.UpdateIcon();

                Items.Clear();
                var addedList = newItems.ToList();
                Items.AddItems(addedList);
                Items.AddItems(PermanentBottomItems);
            }
            finally
            {
                IsVisible = true;
                IsUpdating = false;
            }
        }

        /// <summary>
        /// StartConfigWindow
        /// </summary>
        protected virtual void StartConfigWindow()
        {
            if (ConfigWindow != null)
            {
                ConfigWindow.Topmost = true;
                ConfigWindow.Activate();
                ConfigWindow.Focus();
                ConfigWindow.Topmost = false;
                return;
            }

            var confVm = ViewModelFactory.GenerateConfigViewModel();
            confVm.OnInit(Logic.PluginsCatalog);
            ConfigWindow = new ConfigWindow(confVm);
            ConfigWindow.Closed += async (sender, args) =>
            {
                ConfigWindow = null;
                await UpdateIcon();
            };
            ConfigWindow.Width = 1024;
            ConfigWindow.Height = 768;
            ConfigWindow.Show();
        }

        /// <summary>
        /// KillIcon
        /// </summary>
        protected virtual void KillIcon()
        {
            try
            {
                IsVisible = false;
                Logic?.OnDispose();
            }
            catch
            {
                //do nothing
            }
            Application.Current?.Shutdown();
        }
    }
}
