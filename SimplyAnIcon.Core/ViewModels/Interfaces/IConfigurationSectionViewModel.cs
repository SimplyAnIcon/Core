using System.Collections.Generic;

namespace SimplyAnIcon.Core.ViewModels.Interfaces
{
    /// <summary>
    /// IConfigurationSectionViewModel
    /// </summary>
    public interface IConfigurationSectionViewModel
    {
        /// <summary>
        /// Name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// IconPath
        /// </summary>
        object IconPath { get; }

        /// <summary>
        /// ChildrenSections
        /// </summary>
        IEnumerable<IConfigurationSectionViewModel> ChildrenSections { get; }
    }
}
