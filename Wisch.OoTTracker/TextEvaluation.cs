using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Wisch.OoTTracker.UI;

namespace Wisch.OoTTracker
{
    class TextEvaluation
    {
        public const char COLOR_TAG = (char)0x1A;

        private readonly Bookkeeping bookkeeping;
        private readonly ScreenEquipment equipmentScreen;
        private readonly ScreenQuest questScreen;
        private uint textBoxCurrentId;
        private string textBoxCurrent;

        public TextEvaluation(Bookkeeping bookkeeping, ScreenEquipment equipmentScreen, ScreenQuest questScreen)
        {
            this.bookkeeping = bookkeeping;
            this.equipmentScreen = equipmentScreen;
            this.questScreen = questScreen;
            bookkeeping.DiscrepancyFound += DiscrepancyFound;
        }

        private void DiscrepancyFound(object sender, Bookkeeping.DiscrepancyEventArgs args)
        {
            foreach (var pair in args.Payload)
            {
                if (pair.Key == Bookkeeping.KEY_TEXT_BOX_ID)
                {
                    textBoxCurrentId = (uint)pair.Value;
                }
                else if (pair.Key == Bookkeeping.KEY_TEXT_BOX)
                {
                    textBoxCurrent = (string)pair.Value;
                }
            }

            if (textBoxCurrentId != 0)
            {
                Evaluate(textBoxCurrent);
            }
        }

        Regex wayOfTheHero = new Regex($@"They say .*{COLOR_TAG}([^{COLOR_TAG}]+){COLOR_TAG}.*way of the hero\.", RegexOptions.Compiled);
        Regex foolish = new Regex($@"They say .*{COLOR_TAG}([^{COLOR_TAG}]+){COLOR_TAG}.*foolish choice\.", RegexOptions.Compiled);
        Regex skulltulas = new Regex($@"They say .*(\d\d) Gold Skulltulas.*{COLOR_TAG}([^{COLOR_TAG}]+){COLOR_TAG}\.", RegexOptions.Compiled);
        Regex iceTrapNetwork = new Regex($@"{COLOR_TAG}([^{COLOR_TAG}]*){COLOR_TAG} is a {COLOR_TAG}FOOL{COLOR_TAG}!", RegexOptions.Compiled);
        // TODO: Verify that this is the text for when you receive
        Regex iceTrapLocal = new Regex($@"You are a {COLOR_TAG}FOOL{COLOR_TAG}!", RegexOptions.Compiled);

        private void Evaluate(string text)
        {
            Match match = wayOfTheHero.Match(text);
            if (match.Success)
            {
                equipmentScreen.WayOfTheHero = AddInfo(equipmentScreen.WayOfTheHero, match.Groups[1].Value);
                return;
            }

            match = foolish.Match(text);
            if (match.Success)
            {
                equipmentScreen.Foolish = AddInfo(equipmentScreen.Foolish, match.Groups[1].Value);
                return;
            }

            match = skulltulas.Match(text);
            if (match.Success)
            {
                string gsCount = match.Groups[1].Value;
                string reward = match.Groups[2].Value;

                string info = $"{gsCount} GS -> {reward}";

                equipmentScreen.Notes = AddInfo(equipmentScreen.Notes, info);
                return;
            }

            match = iceTrapNetwork.Match(text);
            if (match.Success)
            {
                questScreen.IceTrapsGiven++;
                return;
            }

            match = iceTrapLocal.Match(text);
            if (match.Success)
            {
                questScreen.IceTrapsReceived++;
                return;
            }

#if DEBUG
            try
            {
                System.IO.File.AppendAllText(@"ootr-tracker-debug.log", $"miss eval: {text}{Environment.NewLine}");
            }
            catch { }
#endif
        }

        private string AddInfo(string original, string info)
        {
            List<string> lines;
            if (String.IsNullOrWhiteSpace(original))
            {
                lines = new List<string>();
            }
            else
            {
                lines = new List<string>(original.Replace("\r", "").Split('\n'));
            }

            foreach (string line in lines)
            {
                if (line == info)
                {
                    return original;
                }
            }

            lines.Add(info);

            return String.Join(Environment.NewLine, lines);
        }
    }
}
