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

        private static Bookkeeping bookkeeping;

        private static readonly ISaveDataProvider saveDataProvider;
        public static event Action<object, EventArgs> Loaded;
        public static event Action<object, EventArgs> Saving;

        private static string romName;

        static SaveData()
        {
            saveDataProvider = new CustomIniSaveDataProvider();

            Application.Idle += SaveHandler;
            Application.ApplicationExit += SaveHandler;
        }

        public const string ICE_TRAPS_RECEIVED = "ice_traps_received";
        public const string ICE_TRAPS_GIVEN = "ice_traps_given";
        public const string ITEM_SLOT_PREFIX = "item_slot_";

        public static void SetBookkeeping(Bookkeeping bookkeeping)
        {
            if (SaveData.bookkeeping != null)
            {
                throw new InvalidOperationException("Bookkeeping was already set in SaveData");
            }

            SaveData.bookkeeping = bookkeeping;
            romName = Global.Game.Name;
            bookkeeping.NewRomLoaded += NewRomLoaded;
        }

        private static void NewRomLoaded(object sender, EventArgs e)
        {
            if (romName != null)
            {
                Save();
            }
            romName = Global.Game.Name;
            Load();
        }

        private static void SaveHandler(object sender, EventArgs e)
        {
            Save();
        }

        private static string GetRomKey()
        {
            return romName;
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
