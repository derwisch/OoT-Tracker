using static Wisch.OoTTracker.ItemData;

namespace Wisch.OoTTracker.UI
{
    /// <summary>
    /// The items screen.
    /// </summary>
    class ScreenItems : ScreenBase
    {
        protected override void DiscrepancyFound(object sender, Bookkeeping.DiscrepancyEventArgs args)
        {
            foreach (var pair in args.Payload)
            {
                switch (pair.Value)
                {
                    case Upgrade upgrades:
                        HandleUpgrades(upgrades);
                        break;
                    case ItemsBlockA itemsBlockA:
                        HandleBlockA(itemsBlockA);
                        break;
                    case ItemsBlockB itemsBlockB:
                        HandleBlockB(itemsBlockB);
                        break;
                    case ItemsBlockC itemsBlockC:
                        HandleBlockC(itemsBlockC);
                        break;
                    case ItemsBlockD itemsBlockD:
                        HandleBlockD(itemsBlockD);
                        break;
                    case ItemsBlockE itemsBlockE:
                        HandleBlockE(itemsBlockE);
                        break;
                }
            }
        }

        private void HandleUpgrades(Upgrade value)
        {
            slotBombs.IsFound = value.HasFlag(Upgrade.BombBag0) || value.HasFlag(Upgrade.BombBag1) || value.HasFlag(Upgrade.BombBag2);
            slotSticks.IsFound = value.HasFlag(Upgrade.DekuSticks0) || value.HasFlag(Upgrade.DekuSticks1);
            slotNuts.IsFound = value.HasFlag(Upgrade.DekuNuts0) || value.HasFlag(Upgrade.DekuNuts1);
            slotBow.IsFound = value.HasFlag(Upgrade.Quiver0) || value.HasFlag(Upgrade.Quiver1) || value.HasFlag(Upgrade.Quiver2);
        }

        private void HandleBlockA(ItemsBlockA value)
        {
            slotArrowFire.IsFound = value.HasFlag(ItemsBlockA.FireArrow) && !value.HasFlag(ItemsBlockA.UnloadedFireArrow);
            slotSpellDin.IsFound = value.HasFlag(ItemsBlockA.DinsFire) && !value.HasFlag(ItemsBlockA.UnloadedDinsFire);
            slotSlingshot.IsFound = value.HasFlag(ItemsBlockA.Slingshot) && !value.HasFlag(ItemsBlockA.UnloadedSlingshot);

            if (!value.HasFlag(ItemsBlockA.UnloadedOcarina))
            {
                if (value.HasFlag(ItemsBlockA.OcarinaOfTime))
                {
                    slotOcarina.IsFound = true;
                    slotOcarina.SelectedAlternative = slotOcarina.Alternatives[1];
                }
                else if (value.HasFlag(ItemsBlockA.FairyOcarina))
                {
                    slotOcarina.IsFound = true;
                    slotOcarina.SelectedAlternative = slotOcarina.Alternatives[0];
                }
                else
                {
                    slotOcarina.IsFound = false;
                }
            }
            else
            {
                slotOcarina.IsFound = false;
            }
        }

        private void HandleBlockB(ItemsBlockB value)
        {
            slotBombchus.IsFound = value.HasFlag(ItemsBlockB.Bombchu) && !value.HasFlag(ItemsBlockB.UnloadedBombchu);
            slotArrowIce.IsFound = value.HasFlag(ItemsBlockB.IceArrow) && !value.HasFlag(ItemsBlockB.UnloadedIceArrow);
            slotSpellFarore.IsFound = value.HasFlag(ItemsBlockB.FaroresWind) && !value.HasFlag(ItemsBlockB.UnloadedFaroresWind);

            if (!value.HasFlag(ItemsBlockB.UnloadedHookshot))
            {
                if (value.HasFlag(ItemsBlockB.Longshot))
                {
                    slotHookshot.IsFound = true;
                    slotHookshot.SelectedAlternative = slotHookshot.Alternatives[1];
                }
                else if (value.HasFlag(ItemsBlockB.Hookshot))
                {
                    slotHookshot.IsFound = true;
                    slotHookshot.SelectedAlternative = slotHookshot.Alternatives[0];
                }
                else
                {
                    slotHookshot.IsFound = false;
                }
            }
            else
            {
                slotHookshot.IsFound = false;
            }
        }

        private void HandleBlockC(ItemsBlockC value)
        {
            slotBoomerang.IsFound = value.HasFlag(ItemsBlockC.Boomerang) && !value.HasFlag(ItemsBlockC.UnloadedBoomerang);
            slotLens.IsFound = value.HasFlag(ItemsBlockC.Lens) && !value.HasFlag(ItemsBlockC.UnloadedLens);
            slotBeans.IsFound = value.HasFlag(ItemsBlockC.MagicBeans) && !value.HasFlag(ItemsBlockC.UnloadedMagicBeans);
            slotHammer.IsFound = value.HasFlag(ItemsBlockC.Hammer) && !value.HasFlag(ItemsBlockC.UnloadedHammer);
        }

        private void HandleBlockD(ItemsBlockD value)
        {
            slotArrowLight.IsFound = value.HasFlag(ItemsBlockD.LightArrow) && !value.HasFlag(ItemsBlockD.UnloadedLightArrow);
            slotSpellNayru.IsFound = value.HasFlag(ItemsBlockD.NayrusLove) && !value.HasFlag(ItemsBlockD.UnloadedNayrusLove);

            uint uintValue = (uint)value;

            string bottle1hex = uintValue.ToString("X8").Substring(2, 2);
            string bottle2hex = uintValue.ToString("X8").Substring(0, 2);

            slotBottle1.IsFound = IsBottle(bottle1hex);
            if (slotBottle1.IsFound)
            {
                slotBottle1.SelectedAlternative = slotBottle1.Alternatives[BottleAlternative(bottle1hex) ?? 0];
            }
            slotBottle2.IsFound = IsBottle(bottle2hex);
            if (slotBottle2.IsFound)
            {
                slotBottle2.SelectedAlternative = slotBottle2.Alternatives[BottleAlternative(bottle2hex) ?? 0];
            }
        }

        private void HandleBlockE(ItemsBlockE value)
        {
            uint uintValue = (uint)value;

            string bottle3hex = uintValue.ToString("X8").Substring(6, 2);
            string bottle4hex = uintValue.ToString("X8").Substring(4, 2);

            slotBottle3.IsFound = IsBottle(bottle3hex);
            if (slotBottle3.IsFound)
            {
                slotBottle3.SelectedAlternative = slotBottle3.Alternatives[BottleAlternative(bottle3hex) ?? 0];
            }
            slotBottle4.IsFound = IsBottle(bottle4hex);
            if (slotBottle4.IsFound)
            {
                slotBottle4.SelectedAlternative = slotBottle4.Alternatives[BottleAlternative(bottle4hex) ?? 0];
            }

            HandleChildTrading(value);
            HandleAdultTrading(value);
        }

        private void HandleChildTrading(ItemsBlockE value)
        {
            if (value.HasFlag(ItemsBlockE.UnloadedChildTrade))
                return;

            if (value.HasFlag(ItemsBlockE.MaskOfTruth))
            {
                slotTradeChild.SelectedAlternative = slotTradeChild.Alternatives[10];
                slotTradeChild.IsFound = true;
            }
            else if (value.HasFlag(ItemsBlockE.GerudoMask))
            {
                slotTradeChild.SelectedAlternative = slotTradeChild.Alternatives[9];
                slotTradeChild.IsFound = true;
            }
            else if (value.HasFlag(ItemsBlockE.ZoraMask))
            {
                slotTradeChild.SelectedAlternative = slotTradeChild.Alternatives[8];
                slotTradeChild.IsFound = true;
            }
            else if (value.HasFlag(ItemsBlockE.GoronMask))
            {
                slotTradeChild.SelectedAlternative = slotTradeChild.Alternatives[7];
                slotTradeChild.IsFound = true;
            }
            else if (value.HasFlag(ItemsBlockE.BunnyHood))
            {
                slotTradeChild.SelectedAlternative = slotTradeChild.Alternatives[6];
                slotTradeChild.IsFound = true;
            }
            else if (value.HasFlag(ItemsBlockE.SpookyMask))
            {
                slotTradeChild.SelectedAlternative = slotTradeChild.Alternatives[5];
                slotTradeChild.IsFound = true;
            }
            else if (value.HasFlag(ItemsBlockE.SkullMask))
            {
                slotTradeChild.SelectedAlternative = slotTradeChild.Alternatives[4];
                slotTradeChild.IsFound = true;
            }
            else if (value.HasFlag(ItemsBlockE.KeatonMask))
            {
                slotTradeChild.SelectedAlternative = slotTradeChild.Alternatives[3];
                slotTradeChild.IsFound = true;
            }
            else if (value.HasFlag(ItemsBlockE.ZeldasLetter))
            {
                slotTradeChild.SelectedAlternative = slotTradeChild.Alternatives[2];
                slotTradeChild.IsFound = true;
            }
            else if (value.HasFlag(ItemsBlockE.Chicken))
            {
                slotTradeChild.SelectedAlternative = slotTradeChild.Alternatives[1];
                slotTradeChild.IsFound = true;
            }
            else if (value.HasFlag(ItemsBlockE.WeirdEgg))
            {
                slotTradeChild.SelectedAlternative = slotTradeChild.Alternatives[0];
                slotTradeChild.IsFound = true;
            }
            else
            {
                slotTradeChild.SelectedAlternative = slotTradeChild.Alternatives[0];
                slotTradeChild.IsFound = false;
            }
        }

        private void HandleAdultTrading(ItemsBlockE value)
        {
            if (value.HasFlag(ItemsBlockE.UnloadedAdultTrade))
                return;

            if (value.HasFlag(ItemsBlockE.ClaimCheck))
            {
                slotTradeAdult.SelectedAlternative = slotTradeAdult.Alternatives[10];
                slotTradeAdult.IsFound = true;
            }
            else if (value.HasFlag(ItemsBlockE.EyeDrops))
            {
                slotTradeAdult.SelectedAlternative = slotTradeAdult.Alternatives[9];
                slotTradeAdult.IsFound = true;
            }
            else if (value.HasFlag(ItemsBlockE.EyeballFrog))
            {
                slotTradeAdult.SelectedAlternative = slotTradeAdult.Alternatives[8];
                slotTradeAdult.IsFound = true;
            }
            else if (value.HasFlag(ItemsBlockE.Prescription))
            {
                slotTradeAdult.SelectedAlternative = slotTradeAdult.Alternatives[7];
                slotTradeAdult.IsFound = true;
            }
            else if (value.HasFlag(ItemsBlockE.BrokenGoronSword))
            {
                slotTradeAdult.SelectedAlternative = slotTradeAdult.Alternatives[6];
                slotTradeAdult.IsFound = true;
            }
            else if (value.HasFlag(ItemsBlockE.PoachersSaw))
            {
                slotTradeAdult.SelectedAlternative = slotTradeAdult.Alternatives[5];
                slotTradeAdult.IsFound = true;
            }
            else if (value.HasFlag(ItemsBlockE.OddPotion))
            {
                slotTradeAdult.SelectedAlternative = slotTradeAdult.Alternatives[4];
                slotTradeAdult.IsFound = true;
            }
            else if (value.HasFlag(ItemsBlockE.OddMushroom))
            {
                slotTradeAdult.SelectedAlternative = slotTradeAdult.Alternatives[3];
                slotTradeAdult.IsFound = true;
            }
            else if (value.HasFlag(ItemsBlockE.Cojiro))
            {
                slotTradeAdult.SelectedAlternative = slotTradeAdult.Alternatives[2];
                slotTradeAdult.IsFound = true;
            }
            else if (value.HasFlag(ItemsBlockE.PocketCucco))
            {
                slotTradeAdult.SelectedAlternative = slotTradeAdult.Alternatives[1];
                slotTradeAdult.IsFound = true;
            }
            else if (value.HasFlag(ItemsBlockE.PocketEgg))
            {
                slotTradeAdult.SelectedAlternative = slotTradeAdult.Alternatives[0];
                slotTradeAdult.IsFound = true;
            }
            else
            {
                slotTradeAdult.SelectedAlternative = slotTradeAdult.Alternatives[0];
                slotTradeAdult.IsFound = false;
            }
        }

        private int? BottleAlternative(string hex)
        {
            switch (hex)
            {
                case "14": return 0; // Empty Bottle 
                case "15": return 1; // Red Potion
                case "16": return 2; // Green Potion 
                case "17": return 3; // Blue Potion 
                case "18": return 4; // Bottled Fairy
                case "19": return 5; // Fish
                case "1A": return 6; // Lon Lon Milk
                case "1B": return 7; // Letter
                case "1C": return 8; // Blue Fire
                case "1D": return 9; // Bug
                case "1E": return 10; // Big Poe
                case "1F": return 11; // Lon Lon Milk (Half)
                case "20": return 12; // Poe
            }
            return null;
        }

        private bool IsBottle(string hex)
        {
            return BottleAlternative(hex) != null;
        }

        public ScreenItems(Bookkeeping bookkeeping, int offsetX, int offsetY) : base(bookkeeping, offsetX, offsetY) { }

        protected override void CreateControls()
        {
            string[] bottleAlternatives = new string[]
            {
                "Empty Bottle", "bottle_empty",
                "Red Potion", "bottle_potion_red",
                "Green Potion", "bottle_potion_green",
                "Blue Potion", "bottle_potion_blue",
                "Bottled Fairy", "bottle_fairy",
                "Fish", "bottle_fish",
                "Lon Lon Milk", "bottle_milk",
                "Letter in a Bottle", "bottle_letter",
                "Blue Fire", "bottle_blue_ice",
                "Bug", "bottle_bug",
                "Big Poe", "bottle_poe_big",
                "Lon Lon Milk (Half)", "bottle_milk_half",
                "Poe", "bottle_poe_small"
            };

            slotSlingshot = BuildItemSlot(55, 111, "Slingshot", "slingshot");
            slotBoomerang = BuildItemSlot(55, 175, "Boomerang", "boomerang");
            slotOcarina = BuildItemSlot(118, 111, "Fairy Ocarina", "ocarina_regular", "Ocarina of Time", "ocarina_time");
            slotLens = BuildItemSlot(118, 175, "Lens of Truth", "lens");
            slotBow = BuildItemSlot(246, 48, "Fairy Bow", "bow");
            slotHookshot = BuildItemSlot(246, 112, "Hookshot", "hookshot", "Longshot", "longshot");
            slotHammer = BuildItemSlot(246, 176, "Megaton Hammer", "hammer");

            slotSticks = BuildItemSlot(54, 48, "Deku Stick", "deku_stick");
            slotNuts = BuildItemSlot(118, 48, "Deku Nut", "deku_nut");
            slotBombs = BuildItemSlot(182, 48, "Bombs", "bomb");
            slotBombchus = BuildItemSlot(182, 111, "Bombchu", "bombchu");

            slotBeans = BuildItemSlot(182, 175, "Magic Beans", "beans");

            slotBottle1 = BuildItemSlot(55, 239, bottleAlternatives);
            slotBottle2 = BuildItemSlot(118, 239, bottleAlternatives);
            slotBottle3 = BuildItemSlot(182, 239, bottleAlternatives);
            slotBottle4 = BuildItemSlot(246, 240, bottleAlternatives);

            slotArrowFire = BuildItemSlot(310, 48, "Fire Arrows", "arrow_fire");
            slotArrowIce = BuildItemSlot(310, 112, "Ice Arrows", "arrow_ice");
            slotArrowLight = BuildItemSlot(310, 176, "Light Arrows", "arrow_light");

            slotSpellDin = BuildItemSlot(374, 48, "Din\'s Fire", "din_fire");
            slotSpellFarore = BuildItemSlot(374, 112, "Faore\'s Wind", "farore_wind");
            slotSpellNayru = BuildItemSlot(374, 176, "Nayru\'s Love", "nayru_love");

            slotTradeChild = BuildItemSlot(374, 240,
                "Weird Egg", "egg",
                "Pocket Cucco", "cuckoo",
                "Zelda\'s Letter", "zelda_letter",
                "Keaton Mask", "mask_fox",
                "Skull Mask", "mask_bone",
                "Spooky Mask", "mask_spooky",
                "Bunny Hood", "mask_bunny",
                "Goron Mask", "mask_goron",
                "Zora Mask", "mask_zora",
                "Gerudo Mask", "mask_gerudo",
                "Mask of Truth", "mask_truth");

            slotTradeAdult = BuildItemSlot(310, 240,
                "Pocket Egg", "egg",
                "Pocket Cucco", "cuckoo",
                "Cojiro", "cuckoo_blue",
                "Odd Mushroom", "mushroom",
                "Odd Poultice", "medicine",
                "Poacher\'s Saw", "saw",
                "Broken Goron\'s Sword", "broken_sword",
                "Prescription", "prescription",
                "Eyeball Frog", "frog",
                "World\'s Finest Eye Drops", "eyedrops",
                "Claim Check", "claim");
        }

        private ItemSlot slotSlingshot;
        private ItemSlot slotBoomerang;
        private ItemSlot slotOcarina;
        private ItemSlot slotLens;
        private ItemSlot slotBow;
        private ItemSlot slotHookshot;
        private ItemSlot slotHammer;

        private ItemSlot slotSticks;
        private ItemSlot slotNuts;
        private ItemSlot slotBombs;
        private ItemSlot slotBombchus;

        private ItemSlot slotBeans;

        private ItemSlot slotBottle1;
        private ItemSlot slotBottle2;
        private ItemSlot slotBottle3;
        private ItemSlot slotBottle4;

        private ItemSlot slotArrowFire;
        private ItemSlot slotArrowIce;
        private ItemSlot slotArrowLight;

        private ItemSlot slotSpellDin;
        private ItemSlot slotSpellFarore;
        private ItemSlot slotSpellNayru;

        private ItemSlot slotTradeChild;
        private ItemSlot slotTradeAdult;
    }
}