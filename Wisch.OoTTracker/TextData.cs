using BizHawk.Emulation.Common;
using System.Collections;
using System.Collections.Generic;

namespace Wisch.OoTTracker
{
    /// <summary>
    /// A class containing text value memory addresses.
    /// </summary>
    public class TextData
    {
        /// <summary>
        /// Memory address of the field that contains the id of the currently displayed text.
        /// Resets to 0 when no textbox is displayed.
        /// Length: 16-bit
        /// </summary>
        public const uint ADDRESS_CURRENT_TEXT_BOX = 0x1D8870;

        /// <summary>
        /// Memory address of the field that contains the id of the most recent displayed text.
        /// Will stay filled until the next textbox is displayed.
        /// Length: 16-bit
        /// </summary>
        public const uint ADDRESS_RECENT_TEXT_BOX = 0x1DAB3E;

        /// <summary>
        /// Memory address of the field that contains the most recent displayed text. 
        /// Will stay filled until the next textbox is displayed.
        /// </summary>
        public const uint ADDRESS_RECENT_TEXT = 0x1D8328;
    }
}
