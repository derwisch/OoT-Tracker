using BizHawk.Client.EmuHawk;
using static Wisch.OoTTracker.ItemData;

namespace Wisch.OoTTracker.UI
{
    /// <summary>
    /// The equipment screen.
    /// Also contains slots for the way of the hero, foolish places and personal notes.
    /// </summary>
    class ScreenEquipment : ScreenBase
    {
        bool isLongSwordBiggoron = false;
        bool isLongSwordFound = false;
        bool isLongSwordBroken = false;

        protected override void DiscrepancyFound(object sender, Bookkeeping.DiscrepancyEventArgs args)
        {
            foreach (var pair in args.Payload)
            {
                if (pair.Key == Bookkeeping.KEY_GIANT_KNIFE_FLAG)
                {
                    if (slotSwordBiggoron.IsFound)
                    {
                        if ((uint)pair.Value == 0)
                        {
                            // is giants knife
                            isLongSwordBiggoron = false;
                        }
                        else
                        {
                            // is biggoron sword
                            isLongSwordBiggoron = true;
                        }
                    }
                }
                else
                {
                    switch (pair.Value)
                    {
                        case Upgrade upgrades:
                            HandleUpgrades(upgrades);
                            break;
                        case Equipment equipment:
                            HandleEquipment(equipment);
                            break;
                    }
                }
            }
            HandleBiggoronSword();
        }

        private void HandleBiggoronSword()
        {
            slotSwordBiggoron.IsFound = isLongSwordFound;
            if (isLongSwordBiggoron)
            {
                slotSwordBiggoron.SelectedAlternative = slotSwordBiggoron.Alternatives[2];
            }
            else if (isLongSwordBroken)
            {
                slotSwordBiggoron.SelectedAlternative = slotSwordBiggoron.Alternatives[1];
            }
            else
            {
                slotSwordBiggoron.SelectedAlternative = slotSwordBiggoron.Alternatives[0];
            }
        }

        public ScreenEquipment(Bookkeeping bookkeeping, int offsetX, int offsetY) : base(bookkeeping, offsetX, offsetY) { }

        private void HandleEquipment(Equipment equipment)
        {
            slotBootsRegular.IsFound = equipment.HasFlag(Equipment.RegularBoots);
            slotBootsIron.IsFound = equipment.HasFlag(Equipment.IronBoots);
            slotBootsHover.IsFound = equipment.HasFlag(Equipment.HoverBoots);

            slotTunicKokiri.IsFound = equipment.HasFlag(Equipment.KokiriTunic);
            slotTunicGoron.IsFound = equipment.HasFlag(Equipment.GoronTunic);
            slotTunicZora.IsFound = equipment.HasFlag(Equipment.ZoraTunic);

            slotShieldDeku.IsFound = equipment.HasFlag(Equipment.DekuShield);
            slotShieldHylia.IsFound = equipment.HasFlag(Equipment.HyliaShield);
            slotShieldMirror.IsFound = equipment.HasFlag(Equipment.MirrorShield);

            slotSwordKokiri.IsFound = equipment.HasFlag(Equipment.KokiriSword);
            slotSwordMaster.IsFound = equipment.HasFlag(Equipment.MasterSword);

            isLongSwordFound = false;
            isLongSwordBroken = false;
            if (equipment.HasFlag(Equipment.LongSword))
            {
                isLongSwordFound = true;
                isLongSwordBroken = false;
            }
            if (equipment.HasFlag(Equipment.BrokenSword))
            {
                isLongSwordFound = true;
                isLongSwordBroken = true;
            }
        }

        private void HandleUpgrades(Upgrade upgrades)
        {
            if (upgrades.HasFlag(Upgrade.Strength2))
            {
                slotStrength.IsFound = true;
                slotStrength.SelectedAlternative = slotStrength.Alternatives[2];
            }
            else if (upgrades.HasFlag(Upgrade.Strength1))
            {
                slotStrength.IsFound = true;
                slotStrength.SelectedAlternative = slotStrength.Alternatives[1];
            }
            else if (upgrades.HasFlag(Upgrade.Strength0))
            {
                slotStrength.IsFound = true;
                slotStrength.SelectedAlternative = slotStrength.Alternatives[0];
            }
            else
            {
                slotStrength.IsFound = false;
            }

            if (upgrades.HasFlag(Upgrade.Scale1))
            {
                slotScale.IsFound = true;
                slotScale.SelectedAlternative = slotScale.Alternatives[1];
            }
            else if (upgrades.HasFlag(Upgrade.Scale0))
            {
                slotScale.IsFound = true;
                slotScale.SelectedAlternative = slotScale.Alternatives[0];
            }
            else
            {
                slotScale.IsFound = false;
            }

            if (upgrades.HasFlag(Upgrade.Wallet2))
            {
                slotWallet.SelectedAlternative = slotWallet.Alternatives[2];
            }
            else if (upgrades.HasFlag(Upgrade.Wallet1))
            {
                slotWallet.SelectedAlternative = slotWallet.Alternatives[1];
            }
            else
            {
                slotWallet.SelectedAlternative = slotWallet.Alternatives[0];
            }

            if (upgrades.HasFlag(Upgrade.BombBag2))
            {
                slotBombBag.IsFound = true;
                slotBombBag.SelectedAlternative = slotBombBag.Alternatives[2];
            }
            else if (upgrades.HasFlag(Upgrade.BombBag1))
            {
                slotBombBag.IsFound = true;
                slotBombBag.SelectedAlternative = slotBombBag.Alternatives[1];
            }
            else if (upgrades.HasFlag(Upgrade.BombBag0))
            {
                slotBombBag.IsFound = true;
                slotBombBag.SelectedAlternative = slotBombBag.Alternatives[0];
            }
            else
            {
                slotBombBag.IsFound = false;
            }

            if (upgrades.HasFlag(Upgrade.BulletBag2))
            {
                slotBulletBag.IsFound = true;
                slotBulletBag.SelectedAlternative = slotBulletBag.Alternatives[2];
            }
            else if (upgrades.HasFlag(Upgrade.BulletBag1))
            {
                slotBulletBag.IsFound = true;
                slotBulletBag.SelectedAlternative = slotBulletBag.Alternatives[1];
            }
            else if (upgrades.HasFlag(Upgrade.BulletBag0))
            {
                slotBulletBag.IsFound = true;
                slotBulletBag.SelectedAlternative = slotBulletBag.Alternatives[0];
            }
            else
            {
                slotBulletBag.IsFound = false;
            }

            if (upgrades.HasFlag(Upgrade.Quiver2))
            {
                slotQuiver.IsFound = true;
                slotQuiver.SelectedAlternative = slotQuiver.Alternatives[2];
            }
            else if (upgrades.HasFlag(Upgrade.Quiver1))
            {
                slotQuiver.IsFound = true;
                slotQuiver.SelectedAlternative = slotQuiver.Alternatives[1];
            }
            else if (upgrades.HasFlag(Upgrade.Quiver0))
            {
                slotQuiver.IsFound = true;
                slotQuiver.SelectedAlternative = slotQuiver.Alternatives[0];
            }
            else
            {
                slotQuiver.IsFound = false;
            }
        }

        // Upgrades
        private ItemSlot slotQuiver;
        private ItemSlot slotBombBag;
        private ItemSlot slotScale;
        private ItemSlot slotStrength;
        private ItemSlot slotBulletBag;
        private ItemSlot slotWallet;

        // Equipment
        private ItemSlot slotSwordKokiri;
        private ItemSlot slotSwordMaster;
        private ItemSlot slotSwordBiggoron;

        private ItemSlot slotShieldDeku;
        private ItemSlot slotShieldHylia;
        private ItemSlot slotShieldMirror;

        private ItemSlot slotBootsRegular;
        private ItemSlot slotBootsIron;
        private ItemSlot slotBootsHover;

        private ItemSlot slotTunicKokiri;
        private ItemSlot slotTunicGoron;
        private ItemSlot slotTunicZora;

        private ItemSlot slotNoteWotH;
        private ItemSlot slotNoteFoolish;
        private ItemSlot slotNoteMisc;


        protected override void CreateControls()
        {
            slotQuiver = BuildItemSlot(19, 52, "Quiver", "quiver_0", 
                                               "Big Quiver", "quiver_1", 
                                               "Biggest Quiver", "quiver_2");
            slotBombBag = BuildItemSlot(19, 116, "Bomb Bag", "bomb_bag_0",
                                                 "Big Bomb Bag", "bomb_bag_1",
                                                 "Biggest Bomb Bag", "bomb_bag_2");
            slotStrength = BuildItemSlot(19, 180, "Bracelet of Strength", "strength_upgrade_0",
                                                  "Silver Gauntlets", "strength_upgrade_1",
                                                  "Golden Gauntlets", "strength_upgrade_2");
            slotScale = BuildItemSlot(19, 244, "Silver Scale", "scale_silver", 
                                               "Golden Scale", "scale_gold");
            slotBulletBag = BuildItemSlot(76, 52, "Bullet Bag", "bullet_pouch_0", 
                                                  "Bigger Bullet Bag", "bullet_pouch_1", 
                                                  "Biggest Bullet Bag", "bullet_pouch_2");
            slotWallet = BuildItemSlot(76, 116, "Child Wallet", "wallet_0", 
                                                "Adult Wallet", "wallet_1", 
                                                "Tycoon Wallet", "wallet_2");
            slotWallet.IsFound = true;
            slotWallet.CanToggle = false;

            slotSwordKokiri = BuildItemSlot(274, 52, "Kokiri Sword", "sword_kokiri");
            slotSwordMaster = BuildItemSlot(338, 52, "Master Sword", "sword_master");
            slotSwordBiggoron = BuildItemSlot(402, 52, "Giant\'s Knife", "sword_big",
                                                       "Broken Giant\'s Knife", "broken_sword_2",
                                                       "Biggoron Sword", "sword_big");
            slotShieldDeku = BuildItemSlot(274, 116, "Deku Shield", "shield_deku");
            slotShieldHylia = BuildItemSlot(338, 116, "Hylian Shield", "shield_hylia");
            slotShieldMirror = BuildItemSlot(402, 116, "Mirror Shield", "shield_mirror");
            slotBootsRegular = BuildItemSlot(274, 244, "Regular Boots", "boots_regular");
            slotBootsIron = BuildItemSlot(338, 244, "Iron Boots", "boots_iron");
            slotBootsHover = BuildItemSlot(402, 244, "Hover Boots", "boots_hover");
            slotTunicKokiri = BuildItemSlot(274, 180, "Kokiri Tunic", "tunic_kokiri");
            slotTunicGoron = BuildItemSlot(338, 180, "Goron Tunic", "tunic_goron");
            slotTunicZora = BuildItemSlot(402, 180, "Zora Tunic", "tunic_zora");

            slotNoteWotH = BuildItemSlot(158, 64, 80, 68, "Way of the Hero", "button_woth");
            slotNoteWotH.IsFound = true;
            slotNoteWotH.CanToggle = false;

            slotNoteFoolish = BuildItemSlot(158, 138, 80, 68, "Foolish", "button_fool");
            slotNoteFoolish.IsFound = true;
            slotNoteFoolish.CanToggle = false;

            slotNoteMisc = BuildItemSlot(158, 212, 80, 68, "Notes", "button_note");
            slotNoteMisc.IsFound = true;
            slotNoteMisc.CanToggle = false;
        }
    }
}