using BizHawk.Client.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;

namespace Wisch.OoTTracker
{
    /// <summary>
    /// A static class that supplies data saving capabilities. <br/>
    /// Currently saves in INI format but could save into a binary format.
    /// </summary>
    internal static class SaveData
    {
        private interface ISaveDataProvider
        {
            T GetData<T>(string key);
            void SetData(string key, object value);
            void Save();
            void Load();
        }

        private class IniSaveDataProvider : ISaveDataProvider
        {
            [DllImport("kernel32")]
            private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
            [DllImport("kernel32")]
            private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

            public void Load() { /* not required */ }

            public void Save() { /* not required */ }

            public T GetData<T>(string key)
            {
                var romKey = GetRomKey();
                var file = GetFilePath();
                var resultBuilder = new StringBuilder();
                GetPrivateProfileString(romKey, key, "", resultBuilder, 255, file);

                try
                {
                    return (T)Convert.ChangeType(resultBuilder.ToString(), typeof(T));
                }
                catch
                {
                    return default;
                }
            }

            public void SetData(string key, object value)
            {
                var romKey = GetRomKey();
                var file = GetFilePath();
                WritePrivateProfileString(romKey, key, value?.ToString() ?? "", file);
            }

            private static string GetFilePath()
            {
                var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var dirPath = Path.Combine(appData, "ootr");
                var filePath = Path.Combine(dirPath, "tracker-data.ini");
                return filePath;
            }
        }

        private class BinarySaveDataProvider : ISaveDataProvider
        {
            private static bool loaded = false;
            private Dictionary<string, Dictionary<string, object>> data = new Dictionary<string, Dictionary<string, object>>();

            public void Load()
            {
                var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var dirPath = Path.Combine(appData, "ootr");
                var filePath = Path.Combine(dirPath, "tracker-library.bin");
                
                if (!File.Exists(filePath))
                {
                    loaded = true;
                    return;
                }
                
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                
                    using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        data = formatter.Deserialize(stream) as Dictionary<string, Dictionary<string, object>>;
                    }
                
                    loaded = true;
                    Loaded?.Invoke(null, EventArgs.Empty);
                }
                catch
                {
                    if (MessageBox.Show("Could not load tracker save data, do you want to enable saving?", "Error loading tracker", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                    {
                        loaded = true;
                        Loaded?.Invoke(null, EventArgs.Empty);
                    }
                }
            }

            public void Save()
            {
                if (!loaded)
                    return;
                
                var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var dirPath = Path.Combine(appData, "ootr");
                var filePath = Path.Combine(dirPath, "tracker-library.bin");
                Directory.CreateDirectory(dirPath);
                
                BinaryFormatter formatter = new BinaryFormatter();
                
                using (var stream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                {
                    formatter.Serialize(stream, data);
                }
            }

            public T GetData<T>(string key)
            {
                var romKey = GetRomKey();

                if (!data.ContainsKey(romKey) || !data[romKey].ContainsKey(key))
                {
                    return default;
                }

                return (T)data[romKey][key];
            }

            public void SetData(string key, object value)
            {
                var romKey = GetRomKey();

                if (!data.ContainsKey(romKey))
                {
                    data[romKey] = new Dictionary<string, object>();
                }
                
                data[romKey][key] = value;
            }

        }

        private static readonly ISaveDataProvider saveDataProvider;
        public static event Action<object, EventArgs> Loaded;
        public static event Action<object, EventArgs> Saving;

        static SaveData()
        {
            saveDataProvider = new IniSaveDataProvider();

            Application.Idle += SaveHandler;
            Application.ApplicationExit += SaveHandler;
        }

        public const string ICE_TRAPS_RECEIVED = "ice_traps_received";
        public const string ICE_TRAPS_GIVEN = "ice_traps_given";
        public const string ITEM_SLOT_PREFIX = "item_slot_";

        private static void SaveHandler(object sender, EventArgs e)
        {
            Save();
        }

        private static string GetRomKey()
        {
            return Global.Game.Name;
        }

        public static T GetData<T>(string key)
        {
            return saveDataProvider.GetData<T>(key);
        } 

        public static void SetData(string key, object value)
        {
            saveDataProvider.SetData(key, value);
        }


        public static void Save()
        {
            Saving?.Invoke(null, EventArgs.Empty);
            saveDataProvider.Save();
        }

        public static void Load()
        {
            saveDataProvider.Load();
            Loaded?.Invoke(null, EventArgs.Empty);
        }
    }
}
