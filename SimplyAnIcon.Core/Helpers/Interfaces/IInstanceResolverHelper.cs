using System;
using System.Collections.Generic;
using Com.Ericmas001.DependencyInjection.RegisteredElements.Interface;

namespace SimplyAnIcon.Core.Helpers.Interfaces
{
    /// <summary>
    /// IInstanceResolverHelper
    /// </summary>
    public interface IInstanceResolverHelper
    {
        /// <summary>
        /// EverythingIsRegistered
        /// </summary>
        void EverythingIsRegistered(IEnumerable<IRegisteredElement> registeredElements);

        /// <summary>
        /// Resolve
        /// </summary>
        object Resolve(Type t);
    }
}
