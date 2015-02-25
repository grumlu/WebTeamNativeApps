using System;
using Windows.Storage;

namespace WebTeamWindows.Resources
{
    public class AppSettings
    {
        // Our settings
        ApplicationDataContainer settings;

        // The key names of our settings
        const string RememberWebTeamPasswordSettingKeyName = "RememberWebTeamPassword";
        const string UseRagotsSettingKeyName = "UseRagots";
        const string WebTeamUsernameSettingKeyName = "WebTeamUsernameSetting";
        const string WebTeamPasswordSettingKeyName = "WebTeamPasswordSetting";

        // The default value of our settings
        const bool RememberWebTeamPasswordSettingDefault = false;
        const bool UseRagotsSettingDefault = true;
        const string WebTeamUsernameSettingDefault = "";
        const string WebTeamPasswordSettingDefault = "";

        /*
        /// <summary>
        /// Constructor that gets the application settings.
        /// </summary>
        public AppSettings()
        {
            if (!System.ComponentModel.DesignerProperties.IsInDesignTool)
                // Get the settings for this application.
                settings = IsolatedStorageSettings.ApplicationSettings;
        }

        /// <summary>
        /// Update a setting value for our application. If the setting does not
        /// exist, then add the setting.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool AddOrUpdateValue(string Key, Object value)
        {

            bool valueChanged = false;

            // If the key exists
            if (settings.Contains(Key))
            {
                // If the value has changed
                if (settings[Key] != value)
                {
                    // Store the new value
                    settings[Key] = value;
                    valueChanged = true;
                }
            }
            // Otherwise create the key.
            else
            {
                settings.Add(Key, value);
                valueChanged = true;
            }
            return valueChanged;
        }

        /// <summary>
        /// Get the current value of the setting, or if it is not found, set the 
        /// setting to the default setting.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetValueOrDefault<T>(string Key, T defaultValue)
        {
            T value;

            // If the key exists, retrieve the value.
            if (settings.Contains(Key))
            {
                value = (T)settings[Key];
            }
            // Otherwise, use the default value.
            else
            {
                value = defaultValue;
            }
            return value;
        }

        /// <summary>
        /// Save the settings.
        /// </summary>
        public void Save()
        {
            settings.Save();
        }


        /// <summary>
        /// Property to get and set a CheckBox Setting Key.
        /// </summary>
        public bool RememberWebTeamPasswordSetting
        {
            get
            {
                return GetValueOrDefault<bool>(RememberWebTeamPasswordSettingKeyName, RememberWebTeamPasswordSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(RememberWebTeamPasswordSettingKeyName, value))
                {
                    Save();
                }
            }
        }


        /// <summary>
        /// Property to get and set a ListBox Setting Key.
        /// </summary>
        public bool UseRagotsSetting
        {
            get
            {
                return GetValueOrDefault<bool>(UseRagotsSettingKeyName, UseRagotsSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(UseRagotsSettingKeyName, value))
                {
                    Save();
                }
            }
        }


        /// <summary>
        /// Property to get and set a Username Setting Key.
        /// </summary>
        public string WebTeamUsernameSetting
        {
            get
            {
                return GetValueOrDefault<string>(WebTeamUsernameSettingKeyName, WebTeamUsernameSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(WebTeamUsernameSettingKeyName, value))
                {
                    Save();
                }
            }
        }

        /// <summary>
        /// Property to get and set a Password Setting Key.
        /// </summary>
        public string WebTeamPasswordSetting
        {
            get
            {
                return GetValueOrDefault<string>(WebTeamPasswordSettingKeyName, WebTeamPasswordSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(WebTeamPasswordSettingKeyName, value))
                {
                    Save();
                }
            }
        }*/
    }
}
