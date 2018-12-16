using System;
using System.IO;

namespace BeFitMod
{
    public static class Preferences
    {
        private static PrefIniFile Instance
        {
                get
                {

                    if (Preferences._instance == null)
                    {
                        Preferences._instance = new PrefIniFile(Path.Combine(Environment.CurrentDirectory, "UserData/" + Plugin.alias + ".ini"));
                    }
                    return Preferences._instance;
                }     
            
        }

        public static string GetString(string section, string name, string defaultValue = "", bool autoSave = false)
        {
            string text = Preferences.Instance.IniReadValue(section, name);
            if (text != "")
            {
                return text;
            }
            if (autoSave)
            {
                Preferences.SetString(section, name, defaultValue);
            }
            return defaultValue;
        }

        public static int GetInt(string section, string name, int defaultValue = 0, bool autoSave = false)
        {
            int result;
            if (int.TryParse(Preferences.Instance.IniReadValue(section, name), out result))
            {
                return result;
            }
            if (autoSave)
            {
                Preferences.SetInt(section, name, defaultValue);
            }
            return defaultValue;
        }
        public static float GetFloat(string section, string name, float defaultValue = 0f, bool autoSave = false)
        {
            float result;
            if (float.TryParse(Preferences.Instance.IniReadValue(section, name), out result))
            {
                return result;
            }
            if (autoSave)
            {
                Preferences.SetFloat(section, name, defaultValue);
            }
            return defaultValue;
        }
        public static bool GetBool(string section, string name, bool defaultValue = false, bool autoSave = false)
        {
            string @string = Preferences.GetString(section, name, null, false);
            if (@string == "1" || @string == "0")
            {
                return @string == "1";
            }
            if (autoSave)
            {
                Preferences.SetBool(section, name, defaultValue);
            }
            return defaultValue;
        }

        public static bool HasKey(string section, string name)
        {
            return Preferences.Instance.IniReadValue(section, name) != null;
        }
        public static void SetFloat(string section, string name, float value)
        {
            Preferences.Instance.IniWriteValue(section, name, value.ToString());
        }
        public static void SetInt(string section, string name, int value)
        {
            Preferences.Instance.IniWriteValue(section, name, value.ToString());
        }
        public static void SetString(string section, string name, string value)
        {
            Preferences.Instance.IniWriteValue(section, name, value);
        }
        public static void SetBool(string section, string name, bool value)
        {
            Preferences.Instance.IniWriteValue(section, name, value ? "1" : "0");
        }
        private static PrefIniFile _instance;
    }
}
