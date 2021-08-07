using BizHawk.Client.Common;
using System;
using System.Windows.Forms;

namespace Wisch.OoTTracker
{
    /// <summary>
    /// A static class that supplies data saving capabilities. <br/>
    /// Currently saves in INI format but could save into a binary format.
    /// </summary>
    internal static partial class SaveData
    {
        private interface ISaveDataProvider
        {
            T GetData<T>(string key);
            void SetData(string key, object value);
            void Save();
            void Load();
        }

        private static readonly ISaveDataProvider saveDataProvider;
        public static event Action<object, EventArgs> Loaded;
        public static event Action<object, EventArgs> Saving;

        static SaveData()
        {
            saveDataProvider = new CustomIniSaveDataProvider();

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
