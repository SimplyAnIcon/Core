using SimplyAnIcon.Plugins.V1.Settings;

namespace SimplyAnIcon.Core.ViewModels.ConfigurationItems
{
    /// <summary>
    /// IntConfigurationItemViewModel
    /// </summary>
    public class IntConfigurationItemViewModel : AbstractConfigurationItemViewModel<IntSettingValue>
    {
        private int _value;

        /// <summary>
        /// Value
        /// </summary>
        public int Value
        {
            get => _value;
            set => Set(ref _value, value);
        }

        /// <inheritdoc />
        public override object ResultValue => Value;

        /// <inheritdoc />
        protected override void OnInit(object defaultValue)
        {
            Value = (int)defaultValue;
        }
    }
}
