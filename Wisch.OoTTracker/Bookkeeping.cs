using BizHawk.Client.EmuHawk;
using System;
using System.Collections.Generic;
using static Wisch.OoTTracker.ItemData;

namespace Wisch.OoTTracker
{
    /// <summary>
    /// A class handling all the book keeping so the tracker doesn't need to update every frame.
    /// </summary>
    class Bookkeeping
    {
        public const string KEY_SKULLTULAS = "Skultullas";
        public const string KEY_GIANT_KNIFE_FLAG = "Giant's Knife Flag";
        public const string KEY_DEATH_COUNTER = "Death Counter";

        /// <summary>
        /// Event that will be fired when a data block has changed.
        /// </summary>
        public event Action<object, DiscrepancyEventArgs> DiscrepancyFound;

        /// <summary>
        /// The values are stored in little endian byte order.
        /// </summary>
        private const bool IS_BIG_ENDIAN = false;

        public Bookkeeping(CustomMainForm trackerForm)
        {
            this.trackerForm = trackerForm;
        }

        public void Update()
        {
            bool isDirty = false;
            Dictionary<string, object> updatePayload = new Dictionary<string, object>();

            uint? newUpgradeData = trackerForm.MemoryDomains.MainMemory.PeekUint(ADDRESS_UPGRADE, IS_BIG_ENDIAN);
            uint? newEquipmentData = trackerForm.MemoryDomains.MainMemory.PeekUshort(ADDRESS_EQUIPMENT, IS_BIG_ENDIAN);
            uint? newItemBlockA = trackerForm.MemoryDomains.MainMemory.PeekUint(ADDRESS_ITEMS_BLOCK_A, IS_BIG_ENDIAN);
            uint? newItemBlockB = trackerForm.MemoryDomains.MainMemory.PeekUint(ADDRESS_ITEMS_BLOCK_B, IS_BIG_ENDIAN);
            uint? newItemBlockC = trackerForm.MemoryDomains.MainMemory.PeekUint(ADDRESS_ITEMS_BLOCK_C, IS_BIG_ENDIAN);
            uint? newItemBlockD = trackerForm.MemoryDomains.MainMemory.PeekUint(ADDRESS_ITEMS_BLOCK_D, IS_BIG_ENDIAN);
            uint? newItemBlockE = trackerForm.MemoryDomains.MainMemory.PeekUint(ADDRESS_ITEMS_BLOCK_E, IS_BIG_ENDIAN);
            uint? newQuestStateA = trackerForm.MemoryDomains.MainMemory.PeekByte(ADDRESS_QUEST_STATE_A);
            uint? newQuestStateB = trackerForm.MemoryDomains.MainMemory.PeekByte(ADDRESS_QUEST_STATE_B);
            uint? newQuestStateC = trackerForm.MemoryDomains.MainMemory.PeekByte(ADDRESS_QUEST_STATE_C);
            uint? newQuestStateD = trackerForm.MemoryDomains.MainMemory.PeekByte(ADDRESS_QUEST_STATE_D);
            uint? newSkultullaState = trackerForm.MemoryDomains.MainMemory.PeekByte(ADDRESS_SKULLTULA_FIELD);
            uint? newGiantKnifeFlag = trackerForm.MemoryDomains.MainMemory.PeekByte(ADDRESS_GIANT_KNIFE_FLAG);
            uint? newDeathCounter = trackerForm.MemoryDomains.MainMemory.PeekUshort(ADDRESS_DEATH_COUNTER, IS_BIG_ENDIAN);

            lastItemBlockA = updateIfNessecary<ItemsBlockA>(newItemBlockA, lastItemBlockA);
            lastItemBlockB = updateIfNessecary<ItemsBlockB>(newItemBlockB, lastItemBlockB);
            lastItemBlockC = updateIfNessecary<ItemsBlockC>(newItemBlockC, lastItemBlockC);
            lastItemBlockD = updateIfNessecary<ItemsBlockD>(newItemBlockD, lastItemBlockD);
            lastItemBlockE = updateIfNessecary<ItemsBlockE>(newItemBlockE, lastItemBlockE);
            lastUpgradeData = updateIfNessecary<Upgrade>(newUpgradeData, lastUpgradeData);
            lastEquipmentData = updateIfNessecary<Equipment>(newEquipmentData, lastEquipmentData);
            lastQuestStateA = updateIfNessecary<QuestStateA>(newQuestStateA, lastQuestStateA);
            lastQuestStateB = updateIfNessecary<QuestStateB>(newQuestStateB, lastQuestStateB);
            lastQuestStateC = updateIfNessecary<QuestStateC>(newQuestStateC, lastQuestStateC);
            lastQuestStateD = updateIfNessecary<QuestStateD>(newQuestStateD, lastQuestStateD);
            lastSkultullaState = updateIfNessecary<uint>(newSkultullaState, lastSkultullaState, KEY_SKULLTULAS);
            lastGiantKnifeFlag = updateIfNessecary<uint>(newGiantKnifeFlag, lastGiantKnifeFlag, KEY_GIANT_KNIFE_FLAG);
            lastDeathCounter = updateIfNessecary<uint>(newDeathCounter, lastDeathCounter, KEY_DEATH_COUNTER);

            if (isDirty)
            {
                OnDiscrepancyFound(updatePayload);
            }

            uint? updateIfNessecary<T>(uint? newData, uint? oldData, string key = null)
            {
                if (newData != null && newData != oldData)
                {
                    updatePayload.Add(key ?? typeof(T).ToString(), (T)((object)newData));
                    isDirty = true;

                    return newData;
                }
                else
                {
                    return oldData;
                }
            }
        }

        private void OnDiscrepancyFound(Dictionary<string, object> updatePayload)
        {
            DiscrepancyFound?.Invoke(this, new DiscrepancyEventArgs(updatePayload));
        }

        private uint? lastItemBlockA = null;
        private uint? lastItemBlockB = null;
        private uint? lastItemBlockC = null;
        private uint? lastItemBlockD = null;
        private uint? lastItemBlockE = null;
        private uint? lastUpgradeData = null;
        private uint? lastEquipmentData = null;
        private uint? lastQuestStateA = null;
        private uint? lastQuestStateB = null;
        private uint? lastQuestStateC = null;
        private uint? lastQuestStateD = null;
        private uint? lastSkultullaState = null;
        private uint? lastGiantKnifeFlag = null;
        private uint? lastDeathCounter = null;

        private readonly CustomMainForm trackerForm;

        public class DiscrepancyEventArgs
        {
            public IEnumerable<KeyValuePair<string, object>> Payload { get; }

            public DiscrepancyEventArgs(Dictionary<string, object> payload)
            {
                Payload = payload;
            }
        }
    }
}