using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Wisch.OoTTracker
{
    internal static partial class SaveData
    {
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

                var data = value?.ToString() ?? "";

                data = data.Replace(Environment.NewLine, "{LINEBREAK}");
                data = data.Replace(Environment.NewLine, "{LB}");
                data = data.Replace("=", "{EQ}");


                WritePrivateProfileString(romKey, key, data, file);
            }

            private static string GetFilePath()
            {
                var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var dirPath = Path.Combine(appData, "ootr");
                var filePath = Path.Combine(dirPath, "tracker-data.ini");
                return filePath;
            }
        }
    }
}
