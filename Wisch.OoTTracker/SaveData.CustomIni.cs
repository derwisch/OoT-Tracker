using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Wisch.OoTTracker
{
    internal static partial class SaveData
    {
        private class CustomIniSaveDataProvider : ISaveDataProvider
        {
            private readonly Dictionary<string, string> data = new Dictionary<string, string>();

            public T GetData<T>(string key)
            {
                if (!data.TryGetValue(key, out string value))
                {
                    return default;
                }

                try
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch
                {
                    return default;
                }
            }

            public void SetData(string key, object value)
            {
                if (value != null)
                {
                    data[key] = value.ToString();
                }
            }

            public void Load()
            {
                if (!File.Exists(GetFilePath()))
                    return;

                string[] lines = File.ReadAllLines(GetFilePath());

                if (!FindSectionId(lines, out int sectionId))
                    return;

                int i = sectionId + 1;
                while (i < lines.Length && !lines[i].StartsWith("["))
                {
                    if (!lines[i].Contains("="))
                    {
                        ++i;
                        continue;
                    }

                    string[] lineData = lines[i].Split('=');
                    data[lineData[0]] = Decode(lineData[1]);

                    ++i;
                }
            }

            private string Decode(string encoded)
            {
                string decoded = encoded;

                decoded = decoded.Replace("{LINEBREAK}", Environment.NewLine);
                decoded = decoded.Replace("{LB}", Environment.NewLine);
                decoded = decoded.Replace("{EQ}", "=");

                return decoded;
            }

            private string Encode(string decoded)
            {
                string encoded = decoded;

                encoded = encoded.Replace(Environment.NewLine, "{LB}");
                encoded = encoded.Replace("=", "{EQ}");

                return encoded;
            }

            public void Save()
            {
                List<string> newLines = new List<string>();

                if (File.Exists(GetFilePath()))
                {
                    string[] oldLines = File.ReadAllLines(GetFilePath());
                    if (FindSectionId(oldLines, out int sectionId))
                    {
                        int i = sectionId + 1;
                        
                        while (i < oldLines.Length && !oldLines[i].StartsWith("["))
                        {
                            ++i;
                        }

                        newLines.AddRange(oldLines.Take(sectionId));
                        if (i != oldLines.Length)
                        {
                            newLines.AddRange(oldLines.Skip(i));
                        }
                    }
                    else
                    {
                        newLines.AddRange(oldLines);
                    }
                }

                newLines.Add($"[{GetRomKey()}]");

                foreach (var pair in data)
                {
                    string line = $"{pair.Key}={Encode(pair.Value)}";
                    newLines.Add(line);
                }

                File.WriteAllLines(GetFilePath(), newLines);
            }

            private bool FindSectionId(string[] lines, out int id)
            {
                for (int i = 0; i < lines.Length; ++i)
                {
                    if (lines[i] == $"[{GetRomKey()}]")
                    {
                        id = i;
                        return true;
                    }
                }

                id = default;
                return false;
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
