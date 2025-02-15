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
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.NPCs.HiveMind;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.NPCs.OldDuke;
using CalamityMod.NPCs.Perforator;
using CalamityMod.NPCs.ProfanedGuardians;
using CalamityMod.NPCs.Providence;
using CalamityMod.NPCs.SlimeGod;
using CalamityMod.Sounds;
using MonoMod.Utils;
using PetsOverhaul.NPCs;
using PetsOverhaul.PetEffects;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria.Audio;
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
            (300,[ModContent.ItemType<CelestialRemains>(), ModContent.ItemType<NovaeSlag>(), ModContent.ItemType<PrismShard>()]),
            (450,[ModContent.ItemType<SeaPrism>()]),
            (550,[ModContent.ItemType<AerialiteOre>(), ModContent.ItemType<AerialiteOreDisenchanted>()]),
            (950,[ModContent.ItemType<InfernalSuevite>()]),
            (1100,[ModContent.ItemType<CryonicOre>()]),
            (1200,[ModContent.ItemType<HallowedOre>(), ModContent.ItemType<Voidstone>()]),
            (1300,[ModContent.ItemType<PerennialOre>()]),
            (1400,[ModContent.ItemType<ScoriaOre>(),ModContent.ItemType<Lumenyl>()]),
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

        public static Dictionary<int, SoundStyle[]> CalamityPetHurtSounds = new()
        {
                        {
                CalamityPetIDs.Astrophage,
                [CommonCalamitySounds.AstralNPCHitSound with {PitchVariance = 0.6f}]
            },
                        {
                CalamityPetIDs.SonOfYharon,
                [new SoundStyle("CalamityMod/Sounds/NPCHit/YharonHurt") with { PitchVariance = 0.6f, Volume = 2f}]
            },
                        {
                CalamityPetIDs.Bear,
                [SoundID.Meowmere]
            },
                        {
                CalamityPetIDs.Kendra,
                [new SoundStyle("CalamityMod/Sounds/Item/KendraBark") with { PitchVariance = 0.4f, Pitch = -0.2f}]
            },
                        {
                ItemID.DogWhistle,
                [new SoundStyle("CalamityMod/Sounds/Item/KendraBark") with { PitchVariance = 0.4f, Pitch = 0.8f}] //Puppy Pet will also use Kendra's bark, with higher pitch
            },
                        {
                CalamityPetIDs.FurtasticDuo,
                [new SoundStyle("CalamityMod/Sounds/Item/KendraBark") with { PitchVariance = 0.4f, Pitch = -0.2f},
                SoundID.Meowmere]
            },
                        {
                CalamityPetIDs.Brimling,
                [SoundID.NPCHit29 with { PitchVariance = 0.5f}]
            },
                        {
                CalamityPetIDs.ChibiiDevourer,
                [                new SoundStyle("CalamityMod/Sounds/NPCKilled/DevourerSegmentBreak1") with { PitchVariance = 0.3f, Volume = 0.8f },
                                new SoundStyle("CalamityMod/Sounds/NPCKilled/DevourerSegmentBreak2") with { PitchVariance = 0.3f, Volume = 0.8f },
                                new SoundStyle("CalamityMod/Sounds/NPCKilled/DevourerSegmentBreak3") with { PitchVariance = 0.3f, Volume = 0.8f },
                                new SoundStyle("CalamityMod/Sounds/NPCKilled/DevourerSegmentBreak4") with { PitchVariance = 0.3f, Volume = 0.8f }]
            },
                        {
                CalamityPetIDs.Akato,
                [                new SoundStyle("PetsOverhaulCalamityAddon/Sounds/Akato/AkatoHurt0") with { PitchVariance = 0.3f },
                                new SoundStyle("PetsOverhaulCalamityAddon/Sounds/Akato/AkatoHurt1") with { PitchVariance = 0.3f },
                                new SoundStyle("PetsOverhaulCalamityAddon/Sounds/Akato/AkatoHurt2") with { PitchVariance = 0.3f }]
            },
                        {
                CalamityPetIDs.ElectricTroublemaker,
                [new SoundStyle("PetsOverhaulCalamityAddon/Sounds/ElectricTroublemaker/PokemonNeutralHit") with { PitchVariance = 0.5f }]
            },
                        {
                CalamityPetIDs.FlakHermit,
                [SoundID.NPCHit41 with { PitchVariance = 0.5f }]
            },
                        {
                CalamityPetIDs.MiniPerforator,
                [new SoundStyle("CalamityMod/Sounds/NPCHit/PerfHiveHit1") with { PitchVariance = 0.3f },
                new SoundStyle("CalamityMod/Sounds/NPCHit/PerfHiveHit2") with { PitchVariance = 0.3f },
                new SoundStyle("CalamityMod/Sounds/NPCHit/PerfHiveHit3") with { PitchVariance = 0.3f }]
            },
                        {
                CalamityPetIDs.PlagueBringerBab,
                [SoundID.NPCHit4 with { PitchVariance = 0.5f }]
            },
                        {
                CalamityPetIDs.SupremeCalamitas,
                [new SoundStyle("CalamityMod/Sounds/Custom/SCalSounds/BrothersHurt1") with { PitchVariance = 0.4f},
                new SoundStyle("CalamityMod/Sounds/Custom/SCalSounds/BrothersHurt2") with { PitchVariance = 0.4f}]
            },

        };

        public static Dictionary<int, SoundStyle[]> CalamityPetAmbientSounds = new()
        {
                        {
                CalamityPetIDs.SonOfYharon,
                [new SoundStyle("CalamityMod/Sounds/Custom/Yharon/YharonRoarShort") with { PitchVariance = 0.6f, Volume = 0.3f}]
            },
                        {
                CalamityPetIDs.Kendra,
                [new SoundStyle("CalamityMod/Sounds/Item/KendraBark") with { PitchVariance = 0.4f, Pitch = -0.2f, Volume = 0.3f}]
            },
                        {
                ItemID.DogWhistle,
                [new SoundStyle("CalamityMod/Sounds/Item/KendraBark") with { PitchVariance = 0.4f, Pitch = 0.8f, Volume = 0.3f }] //Puppy Pet will also use Kendra's bark, with higher pitch
            },
                        {
                CalamityPetIDs.FurtasticDuo,
                [new SoundStyle("CalamityMod/Sounds/Item/KendraBark") with { PitchVariance = 0.4f, Pitch = -0.2f, Volume = 0.3f},
                SoundID.Meowmere with {Volume = 0.3f}]
            },
            {
                CalamityPetIDs.ChibiiDevourer,
                [                new SoundStyle("CalamityMod/Sounds/Custom/DevourerSpawn") with { PitchVariance = 0.5f, Pitch = -0.4f },
                                new SoundStyle("CalamityMod/Sounds/Custom/DevourerAttack") with { PitchVariance = 0.7f }]
            },
        };

        public static Dictionary<int, SoundStyle> CalamityPetKillSounds = new()
        {
            {
                CalamityPetIDs.Astrophage,
                CommonCalamitySounds.AstralNPCDeathSound with {PitchVariance = 0.6f}
            },
            {
                CalamityPetIDs.SonOfYharon,
                new SoundStyle("CalamityMod/Sounds/NPCKilled/YharonDeath") with { PitchVariance = 0.6f }
            },
            {
                CalamityPetIDs.Brimling,
                SoundID.NPCDeath39 with { PitchVariance = 0.5f }
            },
            {
                CalamityPetIDs.ChibiiDevourer,
                new SoundStyle("CalamityMod/Sounds/NPCKilled/DevourerDeath") with { PitchVariance = 0.6f }
            },
            {
                CalamityPetIDs.Akato,
                new SoundStyle("PetsOverhaulCalamityAddon/Sounds/Akato/AkatoDeath") with { PitchVariance = 1f }
            },
            {
                CalamityPetIDs.ElectricTroublemaker,
                new SoundStyle("PetsOverhaulCalamityAddon/Sounds/ElectricTroublemaker/RotomCry") with { PitchVariance = 0.6f, Pitch = -0.7f }
            },
            {
                CalamityPetIDs.FlakHermit,
                SoundID.NPCDeath43 with { PitchVariance = 0.6f }
            },
            {
                CalamityPetIDs.MiniPerforator,
                new SoundStyle("CalamityMod/Sounds/NPCKilled/PerfHiveDeath") with { PitchVariance = 0.6f }
            },
            {
                CalamityPetIDs.PlagueBringerBab,
                CommonCalamitySounds.PlagueBoomSound with { PitchVariance = 0.2f }
            },
            {
                CalamityPetIDs.SupremeCalamitas,
                new SoundStyle("CalamityMod/Sounds/NPCKilled/SepulcherDeath") with { PitchVariance = 0.6f }
            }
        };
        public static void AddCalamitySoundEffects()
        {
            PetSounds.PetItemIdToHurtSound.AddRange(CalamityPetHurtSounds);
            PetSounds.PetItemIdToAmbientSound.AddRange(CalamityPetAmbientSounds);
            PetSounds.PetItemidToKillSound.AddRange(CalamityPetKillSounds);
        }
        public static void AddCalamityPetSlows()
        {
            PetSlowIDs.SicknessBasedSlows.Add(CalSlows.AstrophageSlow);
            PetSlowIDs.SicknessBasedSlows.Add(CalSlows.PlagueSlow);
            PetSlowIDs.SicknessBasedSlows.Add(CalSlows.trashmanSignatureMove);
            PetSlowIDs.ElectricBasedSlows.Add(CalSlows.rotomThunderWave);
            PetSlowIDs.ColdBasedSlows.Add(CalSlows.rotomBlizzard);
        }
        /// <summary>
        /// All Items listed under 'soil blocks' in Calamities official Wiki.
        /// </summary>
        public static List<int> CalSoilBlocks = [ModContent.ItemType<EutrophicSand>(), ModContent.ItemType<Navystone>(), ModContent.ItemType<BrimstoneSlag>(), ModContent.ItemType<ScorchedRemains>(), ModContent.ItemType<SulphurousSand>(), ModContent.ItemType<SulphurousSandstone>(), ModContent.ItemType<HardenedSulphurousSandstone>(), ModContent.ItemType<SulphurousShale>(), ModContent.ItemType<AbyssGravel>(), ModContent.ItemType<PlantyMush>(), ModContent.ItemType<PyreMantle>(), ModContent.ItemType<PyreMantleMolten>(), ModContent.ItemType<Voidstone>(), ModContent.ItemType<AstralDirt>(), ModContent.ItemType<AstralClay>(), ModContent.ItemType<AstralStone>(), ModContent.ItemType<AstralSand>(), ModContent.ItemType<HardenedAstralSand>(), ModContent.ItemType<AstralSandstone>(), ModContent.ItemType<CelestialRemains>(), ModContent.ItemType<AstralSnow>(), ModContent.ItemType<AstralIce>(), ModContent.ItemType<NovaeSlag>(), ModContent.ItemType<VernalSoil>()];
        /// <summary>
        /// All weapons crafted with Living Shard + Blossom Flux.
        /// </summary>
        public static List<int> CalPlanteraWeapons = [ItemID.TerraBlade,ModContent.ItemType<BlossomFlux>(), ModContent.ItemType<BotanicPiercer>(), ModContent.ItemType<Photosynthesis>(), ModContent.ItemType<PlantationStaff>(), ModContent.ItemType<SamsaraSlicer>(), ModContent.ItemType<StygianShield>(), ModContent.ItemType<VernalBolter>(), ModContent.ItemType<WildfireBloom>()];
        /// <summary>
        /// All weapons crafted with Nightmare Fuel.
        /// </summary>
        public static List<int> CalNightmareWeapons = [ModContent.ItemType<LightGodsBrilliance>(), ModContent.ItemType<RecitationoftheBeast>(), ModContent.ItemType<Ataraxia>(), ModContent.ItemType<CorvidHarbringerStaff>(), ModContent.ItemType<DevilsDevastation>(), ModContent.ItemType<FaceMelter>(), ModContent.ItemType<Karasawa>(), ModContent.ItemType<Penumbra>(), ModContent.ItemType<Phangasm>(), ModContent.ItemType<RubicoPrime>()];
        /// <summary>
        /// All weapons crafted with Endothermic Energy.
        /// </summary>
        public static List<int> CalEndothermicWeapons = [ModContent.ItemType<PrimordialAncient>(), ModContent.ItemType<Alluvion>(), ModContent.ItemType<EndoHydraStaff>(), ModContent.ItemType<Hypothermia>(), ModContent.ItemType<Orderbringer>(), ModContent.ItemType<PrismaticBreaker>(), ModContent.ItemType<SDFMG>(), ModContent.ItemType<ThePack>(), ModContent.ItemType<Endogenesis>(), ModContent.ItemType<NanoblackReaper>(), ModContent.ItemType<IceBarrage>()];
        public static void AddCalamityItemLists()
        {
            BabyPenguin.IceFishingDrops.Add(ModContent.ItemType<FishofEleum>());
            DirtiestBlock.CommonBlock.AddRange(CalSoilBlocks);
            Sapling.PlanteraWeapon.AddRange(CalPlanteraWeapons);
            CursedSapling.PumpkinMoonWeapons.AddRange(CalNightmareWeapons);
            BabyGrinch.FrostMoonWeapons.AddRange(CalEndothermicWeapons);
        }
    }
}
