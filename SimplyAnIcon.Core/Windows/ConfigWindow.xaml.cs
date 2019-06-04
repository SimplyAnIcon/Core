using SimplyAnIcon.Core.ViewModels.Interfaces;

namespace SimplyAnIcon.Core.Windows
{
    /// <summary>
    /// ConfigWindow
    /// </summary>
    public partial class ConfigWindow
    {
        /// <summary>
        /// ConfigWindow
        /// </summary>
        public ConfigWindow(IConfigViewModel configViewModel)
        {
            InitializeComponent();
            DataContext = configViewModel;
        }
    }
}
