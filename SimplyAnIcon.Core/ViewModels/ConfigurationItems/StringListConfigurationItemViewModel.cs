using System.Collections.Generic;
using System.Linq;
using SimplyAnIcon.Plugins.V1.Settings;

namespace SimplyAnIcon.Core.ViewModels.ConfigurationItems
{
    /// <summary>
    /// StringListConfigurationItemViewModel
    /// </summary>
    public class StringListConfigurationItemViewModel : AbstractConfigurationItemViewModel<StringListSettingValue>
    {
        private KeyValuePair<string, string> _value;

        /// <summary>
        /// Value
        /// </summary>
        public KeyValuePair<string,string> Value
        {
            get => _value;
            set => Set(ref _value, value);
        }

        /// <inheritdoc />
        public override object ResultValue => Value.Key;

        /// <inheritdoc />
        protected override void OnInit(object defaultValue)
        {
            Value = Setting.AvailableValues.SingleOrDefault(x => x.Key == (string)defaultValue);
        }
    }
}
