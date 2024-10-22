using CalamityMod.Items.Accessories;
using CalamityMod.Items.Accessories.Vanity;
using CalamityMod.Items.Fishing;
using CalamityMod.Items.Fishing.AstralCatches;
using CalamityMod.Items.Fishing.BrimstoneCragCatches;
using CalamityMod.Items.Fishing.SulphurCatches;
using CalamityMod.Items.Fishing.SunkenSeaCatches;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.NPCs.HiveMind;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.NPCs.OldDuke;
using CalamityMod.NPCs.Perforator;
using CalamityMod.NPCs.ProfanedGuardians;
using CalamityMod.NPCs.Providence;
using CalamityMod.NPCs.SlimeGod;
using MonoMod.Utils;
using PetsOverhaul.NPCs;
using PetsOverhaul.PetEffects;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.Systems
{
    /// <summary>
    /// Class mostly for making already existing systems in Pets Overhaul work with Calamity's added content aswell.
    /// </summary>
    internal class Compatibility //All initiated in PetsOverhaulCalamityAddon.cs
    {
        public static Dictionary<string, int> CalamityLightPetItemIDs = new()
        {
            { "RustedJingleBell", CalamityLightPetIDs.BabyGhostBell },
            { "ThiefsDime", CalamityLightPetIDs.Goldie },
            { "SuspiciousLookingNOU", CalamityLightPetIDs.Lilorde },
            { "LittleLight", CalamityLightPetIDs.LittleLight },
            { "StrangeOrb", CalamityLightPetIDs.OceanSpirit },
            { "RadiatingCrystal", CalamityLightPetIDs.Radiator },
            { "EnchantedButterfly", CalamityLightPetIDs.Sparks },
            { "ChromaticOrb", CalamityLightPetIDs.Yuu },
        };
        public static Dictionary<string, int> CalamityPetItemIDs = new()
        {
            { "ForgottenDragonEgg",CalamityPetIDs.Akato },
            { "AstrophageItem", CalamityPetIDs.Astrophage },
            { "BearsEye", CalamityPetIDs.Bear },
            { "CharredRelic", CalamityPetIDs.Brimling },
            { "CosmicPlushie", CalamityPetIDs.ChibiiDevourer },
            { "TrashmanTrashcan", CalamityPetIDs.DannyDevito },
            { "TheEtomer", CalamityPetIDs.ElectricTroublemaker },
            { "AbyssShellFossil", CalamityPetIDs.EscargidolonSnail },
            { "GeyserShell", CalamityPetIDs.FlakHermit },
            { "FoxDrive", CalamityPetIDs.Fox },
            { "PrimroseKeepsake", CalamityPetIDs.FurtasticDuo },
            { "RomajedaOrchid", CalamityPetIDs.Kendra },
            { "JoyfulHeart", CalamityPetIDs.LadShark },
            { "Levi", CalamityPetIDs.Levi },
            { "RottingEyeball", CalamityPetIDs.MiniHiveMind },
            { "BloodyVein", CalamityPetIDs.MiniPerforator },
            { "Pineapple", CalamityPetIDs.Pineapple},
            { "PlagueCaller", CalamityPetIDs.PlagueBringerBab },
            { "McNuggets", CalamityPetIDs.SonOfYharon },
            { "BrimstoneJewel", CalamityPetIDs.SupremeCalamitas },
            { "HermitsBoxofOneHundredMedicines", CalamityPetIDs.ThirdSage },
        };
        /// <summary>
        /// Adds both Light Pets and Regular Pets to their respective lists.
        /// </summary>
        public static void AddPetItemNames()
        {
            PetItemIDs.PetNamesAndItems.AddRange(CalamityPetItemIDs);
            PetItemIDs.LightPetNamesAndItems.AddRange(CalamityLightPetItemIDs);
        }

        public static List<(int, int[])> CalamityHarvestingItems = new()
        {
            (165, [ModContent.ItemType<Acidwood>()]),
            (400,[ModContent.ItemType<AstralGrassSeeds>(), ModContent.ItemType<CinderBlossomSeeds>(), ModContent.ItemType<SulphuricScale>(), ModContent.ItemType<CorrodedFossil>(), ModContent.ItemType<PlantyMush>()]),
            (600, [ModContent.ItemType<AstralMonolith>()]),
            (950,[ModContent.ItemType<SpineSapling>(), ModContent.ItemType<ScorchedBone>()]),
            (1000, [ModContent.ItemType<StarblightSoot>(), ItemID.FallenStar]),
            (12500, [ModContent.ItemType<HapuFruit>()]),
        };
        public static List<(int, int[])> CalamityMiningBlocks = new()
        {
            (300,[ModContent.ItemType<CelestialRemains>(), ModContent.ItemType<NovaeSlag>()]),
            (425,[ModContent.ItemType<SeaPrism>(), ModContent.ItemType<PrismShard>()]),
            (550,[ModContent.ItemType<AerialiteOre>(), ModContent.ItemType<AerialiteOreDisenchanted>()]),
            (950,[ModContent.ItemType<InfernalSuevite>()]),
            (1100,[ModContent.ItemType<CryonicOre>()]),
            (1200,[ModContent.ItemType<HallowedOre>(), ModContent.ItemType<Voidstone>()]),
            (1300,[ModContent.ItemType<PerennialOre>()]),
            (1400,[ModContent.ItemType<ScoriaOre>()]),
            (1500,[ModContent.ItemType<AstralOre>(), ModContent.ItemType<ExodiumCluster>()]),
            (1700,[ModContent.ItemType<UelibloomOre>()]),
            (2000,[ModContent.ItemType<AuricOre>()]),
        };
        public static List<(int, int[])> CalamityFishingCatches = new()
        {
            (150, [ModContent.ItemType<PrismaticGuppy>()]),
            (350, [ModContent.ItemType<CragBullhead>(), ModContent.ItemType<TwinklingPollox>(), ModContent.ItemType<PlantyMush>()]),
            (450, [ModContent.ItemType<CoastalDemonfish>(),ModContent.ItemType<Shadowfish>(),ModContent.ItemType<SunkenSailfish>()]),
            (500, [ModContent.ItemType<EnchantedStarfish>(),ModContent.ItemType<FishofEleum>(),ModContent.ItemType<FishofNight>(),ModContent.ItemType<FishofLight>(),ModContent.ItemType<FishofFlight>(),ModContent.ItemType<GlimmeringGemfish>(),ModContent.ItemType<Gorecodile>(),ModContent.ItemType<Havocfish>(),ModContent.ItemType<StuffedFish>(),ModContent.ItemType<SunbeamFish>()]),
            (550, [ModContent.ItemType<AldebaranAlewife>(),ModContent.ItemType<ArcturusAstroidean>(),ModContent.ItemType<ProcyonidPrawn>()]),
            (600, [ModContent.ItemType<CharredLasher>(),ModContent.ItemType<GreenwaveLoach>()]),
            (650, [ModContent.ItemType<Bloodfin>(), ModContent.ItemType<Spadefish>()]),
            (700, [ModContent.ItemType<BrimstoneCrate>(),ModContent.ItemType<SlagCrate>(), ModContent.ItemType<PrismCrate>(), ModContent.ItemType<EutrophicCrate>(),ModContent.ItemType<HydrothermalCrate>(),ModContent.ItemType<SulphurousCrate>()]),
            (850, [ModContent.ItemType<AstralCrate>(),ModContent.ItemType<MonolithCrate>()]),
            (1350, [ModContent.ItemType<AbyssalAmulet>(), ModContent.ItemType<AlluringBait>(), ModContent.ItemType<RustedJingleBell>()]),
            (1500, [ModContent.ItemType<SparklingEmpress>(), ModContent.ItemType<DragoonDrizzlefish>()]),
            (2000, [ModContent.ItemType<SerpentsBite>()]),
            (2500, [ModContent.ItemType<Floodtide>(), ModContent.ItemType<GacruxianMollusk>(),ModContent.ItemType<PolarisParrotfish>(),ModContent.ItemType<UrsaSergeant>()])
        };
        public static void AddCalamityItemsToGatheringLists()
        {
            Junimo.HarvestingXpPerGathered.AddRange(CalamityHarvestingItems);
            Junimo.MiningXpPerBlock.AddRange(CalamityMiningBlocks);
            Junimo.FishingXpPerCaught.AddRange(CalamityFishingCatches);
            Junimo.FishingXpPerKill.Add((100000, [ModContent.NPCType<OldDuke>()]));
        }

        public static List<int> CalamityNonBossTrueBosses = [ModContent.NPCType<EbonianPaladin>(), ModContent.NPCType<CrimulanPaladin>(), ModContent.NPCType<SplitEbonianPaladin>(), ModContent.NPCType<SplitCrimulanPaladin>()];
        public static void AddCalamityNonBossTrueBosses()
        {
            NpcPet.NonBossTrueBosses.AddRange(CalamityNonBossTrueBosses);
        }

        public static List<int> CalamityCorruptEnemies = [ModContent.NPCType<HiveTumor>(), ModContent.NPCType<EbonianBlightSlime>(), ModContent.NPCType<HiveMind>(), ModContent.NPCType<DankCreeper>(), ModContent.NPCType<DarkHeart>(), ModContent.NPCType<HiveBlob>(), ModContent.NPCType<HiveBlob2>(), ModContent.NPCType<EbonianPaladin>(), ModContent.NPCType<SplitEbonianPaladin>(), ModContent.NPCType<CorruptSlimeSpawn>(), ModContent.NPCType<CorruptSlimeSpawn2>()];
        public static void AddCalamityCorruptEnemies()
        {
            GlobalPet.CorruptEnemies.AddRange(CalamityCorruptEnemies);
        }

        public static List<int> CalamityCrimsonEnemies = [ModContent.NPCType<PerforatorCyst>(), ModContent.NPCType<CrimulanBlightSlime>(), ModContent.NPCType<PerforatorBodyLarge>(), ModContent.NPCType<PerforatorBodyMedium>(), ModContent.NPCType<PerforatorBodySmall>(), ModContent.NPCType<PerforatorHeadLarge>(), ModContent.NPCType<PerforatorHeadMedium>(), ModContent.NPCType<PerforatorHeadSmall>(), ModContent.NPCType<PerforatorHive>(), ModContent.NPCType<PerforatorTailLarge>(), ModContent.NPCType<PerforatorTailMedium>(), ModContent.NPCType<PerforatorTailSmall>(), ModContent.NPCType<CrimulanPaladin>(), ModContent.NPCType<SplitCrimulanPaladin>(), ModContent.NPCType<CrimsonSlimeSpawn>(), ModContent.NPCType<CrimsonSlimeSpawn2>()];
        public static void AddCalamityCrimsonEnemies()
        {
            GlobalPet.CrimsonEnemies.AddRange(CalamityCrimsonEnemies);
        }

        public static List<int> CalamityHallowEnemies = [ModContent.NPCType<CrawlerCrystal>(), ModContent.NPCType<ImpiousImmolator>(), ModContent.NPCType<ProfanedEnergyLantern>(), ModContent.NPCType<ProfanedEnergyBody>(), ModContent.NPCType<ScornEater>(), ModContent.NPCType<ProfanedGuardianCommander>(), ModContent.NPCType<ProfanedGuardianDefender>(), ModContent.NPCType<ProfanedGuardianHealer>(), ModContent.NPCType<ProfanedRocks>(), ModContent.NPCType<ProvSpawnDefense>(), ModContent.NPCType<ProvSpawnHealer>(), ModContent.NPCType<ProvSpawnOffense>(), ModContent.NPCType<Providence>()];
        public static void AddCalamityHallowEnemies()
        {
            GlobalPet.HallowEnemies.AddRange(CalamityHallowEnemies);
        }
    }
}
