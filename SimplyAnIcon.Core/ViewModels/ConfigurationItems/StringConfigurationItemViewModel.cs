using SimplyAnIcon.Plugins.V1.Settings;

namespace SimplyAnIcon.Core.ViewModels.ConfigurationItems
{
    /// <summary>
    /// StringConfigurationItemViewModel
    /// </summary>
    public class StringConfigurationItemViewModel : AbstractConfigurationItemViewModel<StringSettingValue>
    {
        private string _value;

        /// <summary>
        /// Value
        /// </summary>
        public string Value
        {
            get => _value;
            set => Set(ref _value, ApplyModifiers(value));
        }

        /// <inheritdoc />
        public override bool IsValid()
        {
            if (Value.Length < Setting.MinimumLength)
                return false;
            return base.IsValid();
        }

        /// <inheritdoc />
        public override object ResultValue => Value;

        /// <inheritdoc />
        protected override void OnInit(object defaultValue)
        {
            Value = ApplyModifiers((string)defaultValue);
        }

        private string ApplyModifiers(string value)
        {
            switch (Setting.StringType)
            {
                case StringSettingValue.StringTypeEnum.AllLower:
                    return value.ToLower();
                case StringSettingValue.StringTypeEnum.AllUpper:
                    return value.ToUpper();
            }

            return value;
        }
    }
}
