using CalamityMod.Items.Fishing.AstralCatches;
using CalamityMod.Items.Fishing.SunkenSeaCatches;
using CalamityMod.Items.Pets;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.Systems
{
    /// <summary>
    /// Class that includes ItemIDs of Pets of Calamity, working with similar purpose of ItemID class.
    /// </summary>
    public class CalamityPetIDs
    {
        public static int Akato => ModContent.GetInstance<ForgottenDragonEgg>().Type;
        public static int Astrophage => ModContent.GetInstance<AstrophageItem>().Type;
        public static int Bear => ModContent.GetInstance<BearsEye>().Type;
        public static int Brimling => ModContent.GetInstance<CharredRelic>().Type;
        public static int ChibiiDevourer => ModContent.GetInstance<CosmicPlushie>().Type;
        public static int DannyDevito => ModContent.GetInstance<TrashmanTrashcan>().Type;
        public static int ElectricTroublemaker => ModContent.GetInstance<TheEtomer>().Type;
        public static int EscargidolonSnail => ModContent.GetInstance<AbyssShellFossil>().Type;
        public static int FlakHermit => ModContent.GetInstance<GeyserShell>().Type;
        public static int Fox => ModContent.GetInstance<FoxDrive>().Type;
        public static int FurtasticDuo => ModContent.GetInstance<PrimroseKeepsake>().Type;
        public static int Kendra => ModContent.GetInstance<RomajedaOrchid>().Type;
        public static int LadShark => ModContent.GetInstance<JoyfulHeart>().Type;
        public static int Levi => ModContent.GetInstance<Levi>().Type;
        public static int MiniHiveMind => ModContent.GetInstance<RottingEyeball>().Type;
        public static int MiniPerforator => ModContent.GetInstance<BloodyVein>().Type;
        public static int Pineapple => ModContent.GetInstance<PineapplePet>().Type;
        public static int PlagueBringerBab => ModContent.GetInstance<PlagueCaller>().Type;
        public static int SonOfYharon => ModContent.GetInstance<McNuggets>().Type;
        public static int SupremeCalamitas => ModContent.GetInstance<BrimstoneJewel>().Type;
        public static int ThirdSage => ModContent.GetInstance<HermitsBoxofOneHundredMedicines>().Type;
    }
    /// <summary>
    /// Class that includes ItemIDs of Light Pets of Calamity, working with similar purpose of ItemID class.
    /// </summary>
    public class CalamityLightPetIDs
    {
        public static int BabyGhostBell => ModContent.GetInstance<RustedJingleBell>().Type;
        public static int Goldie => ModContent.GetInstance<ThiefsDime>().Type;
        public static int Lilorde => ModContent.GetInstance<SuspiciousLookingNOU>().Type;
        public static int LittleLight => ModContent.GetInstance<LittleLight>().Type;
        public static int OceanSpirit => ModContent.GetInstance<StrangeOrb>().Type;
        public static int Radiator => ModContent.GetInstance<RadiatingCrystal>().Type;
        public static int Sparks => ModContent.GetInstance<EnchantedButterfly>().Type;
        public static int Yuu => ModContent.GetInstance<ChromaticOrb>().Type;
    }
    /// <summary>
    /// CalamitySlows starts from 20 just in case.
    /// </summary>
    public class CalSlows
    {
        public const int AstrophageSlow = 20;
        public const int PlagueSlow = 21;
        public const int trashmanSignatureMove = 22;
        public const int rotomThunderWave = 23;
        public const int rotomBlizzard = 24;
    }
}
