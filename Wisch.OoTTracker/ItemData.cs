namespace Wisch.OoTTracker
{
    /// <summary>
    /// A class containing all data flags and their memory addresses.
    /// </summary>
    public class ItemData
    {
        /// <summary>
        /// Memory address of the field that contains the current skulltula count.
        /// </summary>
        public const uint ADDRESS_SKULLTULA_FIELD = 0x11A6A1;

        /// <summary>
        /// Memory address of the field that determines if the giant's knife is a biggoron sword or not.
        /// </summary>
        public const uint ADDRESS_GIANT_KNIFE_FLAG = 0x11A60E;

        /// <summary>
        /// Memory address of the field that contains the number of deaths/game overs.
        /// </summary>
        public const uint ADDRESS_DEATH_COUNTER = 0x11A5F2;

        /// <summary>
        /// Fire Arrow, Din's Fire, Slingshot, Ocarina<br />
        /// Memory Block 0x11A648
        /// </summary>
        public enum ItemsBlockA : uint
        {
            FairyOcarina = 0x07000000,
            OcarinaOfTime = 0x08000000,
            Slingshot = 0x00060000,
            DinsFire = 0x00000500,
            FireArrow = 0x00000004,

            UnloadedOcarina = 0xFF000000,
            UnloadedSlingshot = 0x00FF0000,
            UnloadedDinsFire = 0x0000FF00,
            UnloadedFireArrow = 0x000000FF,
        }

        /// <summary>
        /// Memory address of the <see cref="ItemsBlockA"/> values
        /// </summary>
        public const uint ADDRESS_ITEMS_BLOCK_A = 0x11A648;

        /// <summary>
        /// Bombchu, Hookshot, Ice Arrow, Farore's Wind<br />
        /// Memory Block 0x11A64C
        /// </summary>
        public enum ItemsBlockB : uint
        {
            FaroresWind = 0x0D000000,
            IceArrow = 0x000C0000,
            Hookshot = 0x00000A00,
            Longshot = 0x00000B00,
            Bombchu = 0x00000009,

            UnloadedFaroresWind = 0xFF000000,
            UnloadedIceArrow = 0x00FF0000,
            UnloadedHookshot = 0x0000FF00,
            UnloadedBombchu = 0x000000FF,
        }

        /// <summary>
        /// Memory address of the <see cref="ItemsBlockB"/> values
        /// </summary>
        public const uint ADDRESS_ITEMS_BLOCK_B = 0x11A64C;

        /// <summary>
        /// Boomerang, Lens of Truth, Beans, Hammer<br />
        /// Memory Block 0x11A650
        /// </summary>
        public enum ItemsBlockC : uint
        {
            Hammer = 0x11000000,
            MagicBeans = 0x00100000,
            Lens = 0x00000F00,
            Boomerang = 0x0000000E,

            UnloadedHammer = 0xFF000000,
            UnloadedMagicBeans = 0x00FF0000,
            UnloadedLens = 0x0000FF00,
            UnloadedBoomerang = 0x000000FF,
        }

        /// <summary>
        /// Memory address of the <see cref="ItemsBlockC"/> values
        /// </summary>
        public const uint ADDRESS_ITEMS_BLOCK_C = 0x11A650;

        /// <summary>
        /// Light Arrow, Nayru's Love, Bottle 1, Bottle 2<br />
        /// Memory Block 0x11A654
        /// </summary>
        public enum ItemsBlockD : uint
        {
            NayrusLove = 0x00001300,
            LightArrow = 0x00000012,

            UnloadedNayrusLove = 0x0000FF00,
            UnloadedLightArrow = 0x000000FF,
        }

        /// <summary>
        /// Memory address of the <see cref="ItemsBlockD"/> values
        /// </summary>
        public const uint ADDRESS_ITEMS_BLOCK_D = 0x11A654;

        /// <summary>
        /// Bottle 3, Bottle 4, Adult Trade, Child Trade <br />
        /// Memory Block 0x11A658
        /// </summary>
        public enum ItemsBlockE : uint
        {
            //child
            WeirdEgg = 0x21000000,
            Chicken = 0x22000000,
            ZeldasLetter = 0x23000000,
            KeatonMask = 0x24000000,
            SkullMask = 0x25000000,
            SpookyMask = 0x26000000,
            BunnyHood = 0x27000000,
            GoronMask = 0x28000000,
            ZoraMask = 0x29000000,
            GerudoMask = 0x2A000000,
            MaskOfTruth = 0x2B000000,

            //adult
            PocketEgg = 0x002D0000,
            PocketCucco = 0x002E0000,
            Cojiro = 0x002F0000,
            OddMushroom = 0x00300000,
            OddPotion = 0x00310000,
            PoachersSaw = 0x00320000,
            BrokenGoronSword = 0x00330000,
            Prescription = 0x00340000,
            EyeballFrog = 0x00350000,
            EyeDrops = 0x00360000,
            ClaimCheck = 0x00370000,

            UnloadedChildTrade = 0xFF000000,
            UnloadedAdultTrade = 0x00FF0000,
        }

        /// <summary>
        /// Memory address of the <see cref="ItemsBlockE"/> values
        /// </summary>
        public const uint ADDRESS_ITEMS_BLOCK_E = 0x11A658;

        public enum Upgrade : uint
        {
            DekuSticks0 = 0x00000200,
            DekuSticks1 = 0x00000400,

            DekuNuts0 = 0x00001000,
            DekuNuts1 = 0x00002000,

            BombBag0 = 0x08000000,
            BombBag1 = 0x10000000,
            BombBag2 = 0x18000000,

            BulletBag0 = 0x00400000,
            BulletBag1 = 0x00800000,
            BulletBag2 = 0x00C00000,

            Strength0 = 0x40000000,
            Strength1 = 0x80000000,
            Strength2 = 0xC0000000,

            Quiver0 = 0x01000000,
            Quiver1 = 0x02000000,
            Quiver2 = 0x03000000,

            Wallet1 = 0x00100000,
            Wallet2 = 0x00200000,

            Scale0 = 0x00020000,
            Scale1 = 0x00040000,
        }

        /// <summary>
        /// Memory address of the <see cref="Upgrade"/> block
        /// </summary>
        public const uint ADDRESS_UPGRADE = 0x11A670;

        public enum Equipment : uint
        {
            KokiriTunic = 0x0001,
            GoronTunic = 0x0002,
            ZoraTunic = 0x0004,

            RegularBoots = 0x0010,
            IronBoots = 0x0020,
            HoverBoots = 0x0040,

            KokiriSword = 0x0100,
            MasterSword = 0x0200,
            LongSword = 0x0400,
            BrokenSword = 0x0B00,

            DekuShield = 0x1000,
            HyliaShield = 0x2000,
            MirrorShield = 0x4000,
        }

        /// <summary>
        /// Memory address of the <see cref="Equipment"/> block
        /// </summary>
        public const uint ADDRESS_EQUIPMENT = 0x11A66C;

        public enum QuestStateA : uint
        {

        }

        /// <summary>
        /// Memory address of the <see cref="QuestStateA"/> block
        /// </summary>
        public const uint ADDRESS_QUEST_STATE_A = 0x11A674;

        public enum QuestStateB : uint
        {
            GerudoToken = 0x40,
            StoneOfAgony = 0x20,
            ZoraSapphire = 0x10,
            GoronRuby = 0x08,
            KokiriEmerald = 0x04,
            SongOfStorm = 0x02,
            SongOfTime = 0x01,
        }

        /// <summary>
        /// Memory address of the <see cref="QuestStateB"/> block
        /// </summary>
        public const uint ADDRESS_QUEST_STATE_B = 0x11A675;

        public enum QuestStateC : uint
        {
            SunSong = 0x80,
            SariaSong = 0x40,
            EponaSong = 0x20,
            ZeldaLullaby = 0x10,
            PreludeOfLight = 0x08,
            NocturneOfShadow = 0x04,
            RequiemOfSpirit = 0x02,
            SerenadeOfWater = 0x01,
        }

        /// <summary>
        /// Memory address of the <see cref="QuestStateC"/> block
        /// </summary>
        public const uint ADDRESS_QUEST_STATE_C = 0x11A676;

        public enum QuestStateD : uint
        {
            BoleroOfFire = 0x80,
            MinuetOfForest = 0x40,
            LightMedallion = 0x20,
            ShadowMedallion = 0x10,
            SpiritMedallion = 0x08,
            WaterMedallion = 0x04,
            FireMedallion = 0x02,
            ForestMedallion = 0x01,
        }

        /// <summary>
        /// Memory address of the <see cref="QuestStateD"/> block
        /// </summary>
        public const uint ADDRESS_QUEST_STATE_D = 0x11A677;
    }
}