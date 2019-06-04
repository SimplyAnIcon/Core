using System;
using SimplyAnIcon.Core.ViewModels.Interfaces;

namespace SimplyAnIcon.Core.ViewModels
{
    /// <summary>
    /// ViewModelFactory
    /// </summary>
    public class ViewModelFactory : IViewModelFactory
    {
        private readonly Func<IConfigViewModel> _funcGenerateConfigViewModel;

        /// <summary>
        /// ViewModelFactory
        /// </summary>
        public ViewModelFactory(Func<IConfigViewModel> funcGenerateConfigViewModel)
        {
            _funcGenerateConfigViewModel = funcGenerateConfigViewModel;
        }

        /// <inheritdoc />
        public IConfigViewModel GenerateConfigViewModel()
        {
            return _funcGenerateConfigViewModel();
        }
    }
}
