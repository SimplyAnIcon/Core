using SimplyAnIcon.Plugins.V1.Settings;

namespace SimplyAnIcon.Core.ViewModels.ConfigurationItems
{
    /// <summary>
    /// BoolConfigurationItemViewModel
    /// </summary>
    public class BoolConfigurationItemViewModel : AbstractConfigurationItemViewModel<BoolSettingValue>
    {
        private bool _value;

        /// <summary>
        /// Value
        /// </summary>
        public bool Value
        {
            get => _value;
            set => Set(ref _value, value);
        }

        /// <inheritdoc />
        public override object ResultValue => Value;

        /// <inheritdoc />
        protected override void OnInit(object defaultValue)
        {
            Value = (bool)defaultValue;
        }
    }
}
