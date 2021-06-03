using BizHawk.Client.EmuHawk;
using System;
using System.Drawing;
using System.Windows.Forms;
using static Wisch.OoTTracker.ItemData;

namespace Wisch.OoTTracker.UI
{
    /// <summary>
    /// The quest screen.
    /// Also contains counters for given and received ice traps and game overs.
    /// </summary>
    class ScreenQuest : ScreenBase
    {
        private readonly Color skulltulaColor = Color.FromArgb(255, 204, 0);

        private Label labelIceTrapReceived;
        private Label labelIceTrapGiven;

        private CounterToggle toggleIceTrapsReceived;
        private CounterToggle toggleIceTrapsGiven;

        private Label labelSkulltula;
        private Label labelDeathCounter;
        private ItemSlot slotSkulltula;

        private ItemSlot slotStoneOfAgony;
        private ItemSlot slotGerudoToken;

        private ItemSlot slotMedallionForest;
        private ItemSlot slotMedallionFire;
        private ItemSlot slotMedallionWater;
        private ItemSlot slotMedallionSpirit;
        private ItemSlot slotMedallionShadow;
        private ItemSlot slotMedallionLight;
        private ItemSlot slotGemKokiri;
        private ItemSlot slotGemGoron;
        private ItemSlot slotGemZora;

        private ItemSlot slotSongZelda;
        private ItemSlot slotSongEpona;
        private ItemSlot slotSongSaria;
        private ItemSlot slotSongTime;
        private ItemSlot slotSongSun;
        private ItemSlot slotSongStorms;

        private ItemSlot slotSongWarpForest;
        private ItemSlot slotSongWarpFire;
        private ItemSlot slotSongWarpWater;
        private ItemSlot slotSongWarpSpirit;
        private ItemSlot slotSongWarpShadow;
        private ItemSlot slotSongWarpLight;

        private readonly CustomMainForm trackerForm;

        protected override void DiscrepancyFound(object sender, Bookkeeping.DiscrepancyEventArgs args)
        {
            foreach (var pair in args.Payload)
            {
                if (pair.Key == Bookkeeping.KEY_SKULLTULAS)
                {
                    labelSkulltula.Text = pair.Value.ToString();
                }
                else if (pair.Key == Bookkeeping.KEY_DEATH_COUNTER)
                {
                    labelDeathCounter.Text = ((uint)pair.Value >> 8).ToString();
                }
                else
                {
                    switch (pair.Value)
                    {
                        case QuestStateB questStateB:
                            HandleQuestStateB(questStateB);
                            break;
                        case QuestStateC questStateC:
                            HandleQuestStateC(questStateC);
                            break;
                        case QuestStateD questStateD:
                            HandleQuestStateD(questStateD);
                            break;
                    }
                }
            }
        }

        internal void RedrawSkulltulas()
        {
            float clientWidth = GlobalWin.MainForm.ClientSize.Width;
            float clientHeight = GlobalWin.MainForm.ClientSize.Height - 23;

            float aspectRatioVideo = (float)trackerForm.VideoProvider.BufferWidth / trackerForm.VideoProvider.BufferHeight;
            float aspectRatioWindow = clientWidth / clientHeight;

            float actualWidth;
            float actualHeight;

            float paddingX = 0;
            float paddingY = 0;

            if (aspectRatioWindow > aspectRatioVideo)
            {
                actualWidth = clientWidth / aspectRatioWindow * aspectRatioVideo;
                actualHeight = clientHeight;
                paddingX = (clientWidth - actualWidth) / 2;
            }
            else
            {
                actualWidth = clientWidth;
                actualHeight = clientHeight / aspectRatioVideo * aspectRatioWindow;
                paddingY = (clientHeight - actualHeight) / 2;
            }

            //font size and skulltula size are both 24 with a height of 600px
            float fontSize = (24f / 600) * actualHeight;

            float skulltulaX = paddingX + ((74f / 800) * actualWidth);
            float skulltulaY = clientHeight - paddingY - ((47f / 600) * actualHeight);

            float textX = paddingX + ((104f / 800) * actualWidth);
            float textY = clientHeight - paddingY - ((50f / 600) * actualHeight);

            GuiApi.Instance.DrawNew("native", true);

            Image image = Resources.Instance["skultula"];
            GuiApi.Instance.DrawImage(image, (int)skulltulaX, (int)skulltulaY, (int)fontSize, (int)fontSize);
            GuiApi.Instance.DrawString((int)textX, (int)textY, labelSkulltula.Text,
                    forecolor: skulltulaColor,
                    fontsize: (int)fontSize,
                    fontfamily: "Arial");
            GuiApi.Instance.DrawFinish();
        }

        private void HandleQuestStateB(QuestStateB value)
        {
            slotGerudoToken.IsFound = value.HasFlag(QuestStateB.GerudoToken);
            slotStoneOfAgony.IsFound = value.HasFlag(QuestStateB.StoneOfAgony);
            slotGemZora.IsFound = value.HasFlag(QuestStateB.ZoraSapphire);
            slotGemGoron.IsFound = value.HasFlag(QuestStateB.GoronRuby);
            slotGemKokiri.IsFound = value.HasFlag(QuestStateB.KokiriEmerald);
            slotSongStorms.IsFound = value.HasFlag(QuestStateB.SongOfStorm);
            slotSongTime.IsFound = value.HasFlag(QuestStateB.SongOfTime);
        }

        private void HandleQuestStateC(QuestStateC value)
        {
            slotSongSun.IsFound = value.HasFlag(QuestStateC.SunSong);
            slotSongSaria.IsFound = value.HasFlag(QuestStateC.SariaSong);
            slotSongEpona.IsFound = value.HasFlag(QuestStateC.EponaSong);
            slotSongZelda.IsFound = value.HasFlag(QuestStateC.ZeldaLullaby);
            slotSongWarpLight.IsFound = value.HasFlag(QuestStateC.PreludeOfLight);
            slotSongWarpShadow.IsFound = value.HasFlag(QuestStateC.NocturneOfShadow);
            slotSongWarpSpirit.IsFound = value.HasFlag(QuestStateC.RequiemOfSpirit);
            slotSongWarpWater.IsFound = value.HasFlag(QuestStateC.SerenadeOfWater);
        }

        private void HandleQuestStateD(QuestStateD value)
        {
            slotSongWarpFire.IsFound = value.HasFlag(QuestStateD.BoleroOfFire);
            slotSongWarpForest.IsFound = value.HasFlag(QuestStateD.MinuetOfForest);
            slotMedallionLight.IsFound = value.HasFlag(QuestStateD.LightMedallion);
            slotMedallionShadow.IsFound = value.HasFlag(QuestStateD.ShadowMedallion);
            slotMedallionSpirit.IsFound = value.HasFlag(QuestStateD.SpiritMedallion);
            slotMedallionWater.IsFound = value.HasFlag(QuestStateD.WaterMedallion);
            slotMedallionFire.IsFound = value.HasFlag(QuestStateD.FireMedallion);
            slotMedallionForest.IsFound = value.HasFlag(QuestStateD.ForestMedallion);
        }

        public ScreenQuest(CustomMainForm trackerForm, Bookkeeping bookkeeping, int offsetX, int offsetY) : base(bookkeeping, offsetX, offsetY)
        {
            this.trackerForm = trackerForm;
            SaveData.Loaded += SaveData_Loaded;
        }

        private void SaveData_Loaded(object arg1, EventArgs arg2)
        {
            labelIceTrapReceived.Text = SaveData.GetData<string>(SaveData.ICE_TRAPS_RECEIVED);
            labelIceTrapGiven.Text = SaveData.GetData<string>(SaveData.ICE_TRAPS_GIVEN);
            toggleIceTrapsReceived.Value = SaveData.GetData<int>(SaveData.ICE_TRAPS_RECEIVED);
            toggleIceTrapsGiven.Value = SaveData.GetData<int>(SaveData.ICE_TRAPS_GIVEN);
        }

        protected override void CreateControls()
        {
            labelIceTrapReceived = BuildLabel(190, 24, 60, 48, "0", Color.CornflowerBlue, fontSize: 24, bold: true, alignment: ContentAlignment.MiddleCenter);
            labelIceTrapReceived.TextChanged += (sender, e) => SaveData.SetData(SaveData.ICE_TRAPS_RECEIVED, labelIceTrapReceived.Text);

            labelIceTrapGiven = BuildLabel(190, 80, 60, 48, "0", Color.CornflowerBlue, fontSize: 24, bold: true, alignment: ContentAlignment.MiddleCenter);
            labelIceTrapGiven.TextChanged += (sender, e) => SaveData.SetData(SaveData.ICE_TRAPS_GIVEN, labelIceTrapGiven.Text);

            toggleIceTrapsReceived = BuildCounterToggle(134, 24, 48, 48, "ice_trap_received", labelIceTrapReceived);
            toggleIceTrapsGiven = BuildCounterToggle(134, 80, 48, 48, "ice_trap_given", labelIceTrapGiven);

            slotSkulltula = BuildItemSlot(26, 101, 32, 32, "Skulltula", "skultula");
            slotSkulltula.IsFound = true;
            slotSkulltula.CanToggle = false;
            labelSkulltula = BuildLabel(65, 107, 44, 20, "0", Color.FromArgb(255, 204, 0),
                                        fontSize: 12F,
                                        bold: true,
                                        alignment: ContentAlignment.MiddleCenter);

            slotStoneOfAgony = BuildItemSlot(26, 49, 32, 32, "Stone of Agony", "rumble_pack");
            slotGerudoToken = BuildItemSlot(77, 49, 32, 32, "Gerudo Token", "gerudo_card");

            slotMedallionForest = BuildItemSlot(392, 88, 40, 40, "Forest Medallion", "amulet_forest");
            slotMedallionFire = BuildItemSlot(392, 152, 40, 40, "Fire Medallion", "amulet_fire");
            slotMedallionWater = BuildItemSlot(336, 188, 40, 40, "Water Medallion", "amulet_water");
            slotMedallionSpirit = BuildItemSlot(280, 152, 40, 40, "Spirit Medallion", "amulet_spirit");
            slotMedallionShadow = BuildItemSlot(280, 88, 40, 40, "Shadow Medallion", "amulet_shadow");
            slotMedallionLight = BuildItemSlot(336, 52, 40, 40, "Light Medallion", "amulet_light");

            slotGemKokiri = BuildItemSlot(288, 260, 32, 32, "Kokiri\'s Emerald", "gem_kokiri");
            slotGemGoron = BuildItemSlot(342, 260, 32, 32, "Goron\'s Ruby", "gem_goron");
            slotGemZora = BuildItemSlot(392, 260, 32, 32, "Zora\'s Sapphire", "gem_zora");

            slotSongZelda = BuildItemSlot(27, 160, 22, 32, "Zelda\'s Lullaby", "note_regular");
            slotSongEpona = BuildItemSlot(61, 160, 22, 32, "Epona\'s Song", "note_regular");
            slotSongSaria = BuildItemSlot(98, 160, 22, 32, "Saria\'s Song", "note_regular");
            slotSongSun = BuildItemSlot(134, 160, 22, 32, "Sun\'s Song", "note_regular");
            slotSongTime = BuildItemSlot(171, 160, 22, 32, "Song of Time", "note_regular");
            slotSongStorms = BuildItemSlot(207, 160, 22, 32, "Song of Storms", "note_regular");

            slotSongWarpForest = BuildItemSlot(27, 204, 22, 32, "Minuet of Forest", "note_warp_forest");
            slotSongWarpFire = BuildItemSlot(61, 204, 22, 32, "Bolero of Fire", "note_warp_mountain");
            slotSongWarpWater = BuildItemSlot(98, 204, 22, 32, "Serenade of Water", "note_warp_lake");
            slotSongWarpSpirit = BuildItemSlot(134, 204, 22, 32, "Requiem of Spirit", "note_warp_desert");
            slotSongWarpShadow = BuildItemSlot(171, 204, 22, 32, "Nocturne of Shadow", "note_warp_graveyard");
            slotSongWarpLight = BuildItemSlot(207, 204, 22, 32, "Prelude of Light", "note_warp_citadel");

            labelDeathCounter = BuildLabel(104, 248, 140, 54, "0", Color.FromArgb(255, 204, 0),
                                        fontSize: 24F,
                                        bold: true,
                                        alignment: ContentAlignment.MiddleCenter);
        }
    }
}