using CalamityMod;
using CalamityMod.Items.Pets;
using CalamityMod.NPCs.HiveMind;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.NPCs.Perforator;
using CalamityMod.NPCs.ProfanedGuardians;
using CalamityMod.NPCs.Providence;
using CalamityMod.NPCs.SlimeGod;
using MonoMod.Utils;
using PetsOverhaul.NPCs;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.Systems
{
    /// <summary>
    /// Class mostly for making already existing systems in Pets Overhaul work with Calamity's added content aswell.
    /// </summary>
    internal class Compatibility
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

        public static List<int> CalamityHallowEnemies = [ModContent.NPCType<CrawlerCrystal>(), ModContent.NPCType<ImpiousImmolator>(), ModContent.NPCType<ProfanedEnergyLantern>(), ModContent.NPCType<ProfanedEnergyBody>(), ModContent.NPCType<ScornEater>(), ModContent.NPCType<ProfanedGuardianCommander>(),ModContent.NPCType<ProfanedGuardianDefender>(), ModContent.NPCType<ProfanedGuardianHealer>(), ModContent.NPCType<ProfanedRocks>(), ModContent.NPCType<ProvSpawnDefense>(), ModContent.NPCType<ProvSpawnHealer>(), ModContent.NPCType<ProvSpawnOffense>(), ModContent.NPCType<Providence>()];
        public static void AddCalamityHallowEnemies()
        {
            GlobalPet.HallowEnemies.AddRange(CalamityHallowEnemies);
        }
    }
}
