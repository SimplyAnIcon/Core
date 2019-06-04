using System.Reflection;
using SimplyAnIcon.Core.Helpers.Interfaces;

namespace SimplyAnIcon.Core.Helpers
{
    /// <summary>
    /// DefaultIconConfigHelper
    /// </summary>
    public class DefaultIconConfigHelper : IIconConfigHelper
    {
        /// <inheritdoc />
        public bool IsUpdateActivated() => true;

        /// <inheritdoc />
        public string GetAppPath() => Assembly.GetEntryAssembly()?.Location;
    }
}
