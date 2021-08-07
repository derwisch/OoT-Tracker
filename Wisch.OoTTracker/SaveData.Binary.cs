using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Wisch.OoTTracker
{
    internal static partial class SaveData
    {
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

    }
}
