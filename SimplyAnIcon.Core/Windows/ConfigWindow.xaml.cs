using SimplyAnIcon.Core.ViewModels;

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
        public ConfigWindow( AbstractConfigViewModel abstractConfigViewModel )
        {
            InitializeComponent();
            DataContext = abstractConfigViewModel;
        }
    }
}
