using BizHawk.Client.EmuHawk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Wisch.OoTTracker.ItemData;
using static Wisch.OoTTracker.TextData;

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
        public const string KEY_TEXT_BOX_ID = "Text Box ID";
        public const string KEY_TEXT_BOX = "Text Box";
        public const string KEY_TEXT_BOX_CLEAN = "Text Box Clean";

        /// <summary>
        /// Event that will be fired when a data block has changed.
        /// </summary>
        public event Action<object, DiscrepancyEventArgs> DiscrepancyFound;

        /// <summary>
        /// Event that will be fired when a new rom is loaded.
        /// </summary>
        public event Action<object, EventArgs> NewRomLoaded;

        /// <summary>
        /// The values are stored in little endian byte order.
        /// </summary>
        private const bool IS_BIG_ENDIAN = false;

        /// <summary>
        /// Reads a text until the string terminator (NUL; 0x00)
        /// </summary>
        private string ReadText(uint address)
        {
            StringBuilder builder = new StringBuilder();

            byte? currentByte = null;
            int i = 0;
            
            // Strings seem to be ending in the sequence { 0x02, 0x00 }
            // it's likely that that only NUL is the string terminator
            // and I couldn't find any NUL characters in texts. Thus we
            // use only the NUL char
            while (currentByte != 0x00)
            {
                currentByte = trackerForm.MemoryDomains.MainMemory.PeekByte(address + i);

                // ENQ (0x05) seems to be the color indicator.
                // The color indicator is followed by another
                // byte indicating the actual color.
                // For our text evaluation we want to mark the
                // beginning and end of the colored parts.
                if (currentByte == 0x05)
                {
                    i += 2;
                    builder.Append(TextEvaluation.COLOR_TAG);
                    continue;
                }

                char c = (char)currentByte;

                // SOH (0x01) seens to be used as new line character
                // We don't need the line breaks here so we'll replace
                // it with a space instead.
                if (currentByte == 0x01)
                {
                    builder.Append(" ");
                }
                // All string seem to contain a few control chars,
                // besides the color indicator BS (0x08) at the start of
                // each string, STX (0x02) ironically at the end of each
                // string and NUL (0x00) as string terminator. These should
                // be safe to ignore. If we should encounter other control
                // chars we don't want them to pollute our string.
                else if (!Char.IsControl(c))
                {
                    builder.Append(c);
                }
#if DEBUG
                // If we have any other control characters we want to 
                else if (!new byte[] { 0x08, 0x02, 0x00 }.Contains(currentByte.Value))
                {
                    try
                    {
                        System.IO.File.AppendAllText(@"ootr-tracker-debug.log", $"unkn ctrl: {currentByte:X2} after '{builder}'{Environment.NewLine}");
                    }
                    catch { }
                }
#endif

                i++;
            }

            return builder.ToString();
            //*/
        }

        public Bookkeeping(CustomMainForm trackerForm)
        {
            this.trackerForm = trackerForm;
        }

        private uint? oldBootData = 0x00;

        public void Update()
        {
            bool isDirty = false;
            Dictionary<string, object> updatePayload = new Dictionary<string, object>();

            try
            {
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
                uint? newTextBoxId = trackerForm.MemoryDomains.MainMemory.PeekUshort(ADDRESS_CURRENT_TEXT_BOX, IS_BIG_ENDIAN);

                // updating inventory data
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

                // updating text data
                if (newTextBoxId != lastTextBoxId)
                {
                    if (newTextBoxId == 0)
                    {
                        lastTextBox = "";
                    }
                    else
                    {
                        lastTextBox = ReadText(ADDRESS_RECENT_TEXT);
                        lastTextBoxClean = lastTextBox.Replace(TextEvaluation.COLOR_TAG.ToString(), "");
                    }
                    updatePayload.Add(KEY_TEXT_BOX, lastTextBox);
                    isDirty = true;
                }
                lastTextBoxId = updateIfNessecary<uint>(newTextBoxId, lastTextBoxId, KEY_TEXT_BOX_ID);

                uint? newBootData = trackerForm.MemoryDomains.MainMemory.PeekByte(0x016DA0);
                if (newBootData == 0x00 && oldBootData != 0x00)
                {
                    NewRomLoaded?.Invoke(this, EventArgs.Empty);
                }
                oldBootData = newBootData;
            }
            catch { /* Most likely error with MemoryDomains */ }

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

        private uint? lastTextBoxId = null;
        private string lastTextBox = null;
        private string lastTextBoxClean = null;

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