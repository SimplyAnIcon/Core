using System;
using Com.Ericmas001.Mvvm;
using SimplyAnIcon.Plugins.V1.Settings;

namespace SimplyAnIcon.Core.ViewModels.ConfigurationItems
{
    /// <summary>
    /// AbstractConfigurationItemViewModel
    /// </summary>
    public abstract class AbstractConfigurationItemViewModel : ViewModelBase
    {
        /// <summary>
        /// Setting
        /// </summary>
        public AbstractSettingValue Setting { get; private set; }

        /// <summary>
        /// IsValid
        /// </summary>
        public virtual bool IsValid() => true;

        /// <summary>
        /// ResultValue
        /// </summary>
        public abstract object ResultValue { get; }

        /// <summary>
        /// OnInit
        /// </summary>
        public virtual void OnInit(AbstractSettingValue setting, object defaultValue)
        {
            Setting = setting;
        }
    }

    /// <summary>
    /// AbstractConfigurationItemViewModel Of AbstractSettingValue
    /// </summary>
    public abstract class AbstractConfigurationItemViewModel<T> : AbstractConfigurationItemViewModel where T : AbstractSettingValue
    {
        /// <summary>
        /// Setting
        /// </summary>
        public new T Setting { get; private set; }

        /// <inheritdoc />
        public override void OnInit(AbstractSettingValue setting, object defaultValue)
        {
            base.OnInit(setting, defaultValue);
            if (setting is T tSetting)
                Setting = tSetting;
            else
                throw new ArgumentException("Wrong Setting Type. Should be of type " + typeof(T).FullName,
                    nameof(setting));
            OnInit(defaultValue);
        }

        /// <summary>
        /// OnInit
        /// </summary>
        protected abstract void OnInit(object defaultValue);
    }
}
