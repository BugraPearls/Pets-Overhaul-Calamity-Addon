using CalamityMod.Items.Armor.Demonshade;
using CalamityMod.Items.Fishing.AstralCatches;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Mounts;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Items.Potions;
using CalamityMod.Items.Potions.Alcohol;
using CalamityMod.Items.SummonItems;
using CalamityMod.Items.Weapons.Ranged;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.Systems
{
    public class CalamityPetRecipes : ModSystem
    {
        public override void AddRecipes()
        {
            PetRecipes.PetRecipe(Recipe.Create(CalamityPetIDs.Akato).AddIngredient<YharonSoulFragment>(30).AddIngredient<EffulgentFeather>(15).AddIngredient(ItemID.WizardHat).AddIngredient(ItemID.WingsSolar).AddIngredient<YharonEgg>(),15000);
            PetRecipes.PetRecipe(Recipe.Create(CalamityPetIDs.Astrophage).AddIngredient<ProcyonidPrawn>().AddIngredient(ItemID.WaterStrider).AddIngredient<StarblightSoot>(300).AddIngredient<AstralMonolith>(650), 1500);
            PetRecipes.PetRecipe(Recipe.Create(CalamityPetIDs.Bear).AddIngredient(ItemID.WhitePearl, 2).AddIngredient(ItemID.AngelStatue).AddCondition(Condition.DownedSkeletron), 9999);
            PetRecipes.MasterModePetRecipe(Recipe.Create(CalamityPetIDs.Brimling).AddIngredient<InfernalSuevite>(30).AddIngredient<ScorchedBone>(120).AddIngredient<EssenceofHavoc>(12), 2000);
            PetRecipes.MasterModePetRecipe(Recipe.Create(CalamityPetIDs.ChibiiDevourer).AddIngredient<Fabsol>().AddIngredient<CosmiliteBar>(9999).AddIngredient<FabsolsVodka>(9999).AddIngredient(ItemID.PlatinumCoin, 9999).AddIngredient(ItemID.GoldCoin, 99).AddIngredient(ItemID.SilverCoin, 99).AddIngredient(ItemID.CopperCoin, 99).AddCondition(Condition.InSpace), 99999, 9);
            PetRecipes.PetRecipe(Recipe.Create(CalamityPetIDs.DannyDevito).AddIngredient(ItemID.TrashCan, 10).AddIngredient(ItemID.FishingSeaweed, 5).AddIngredient(ItemID.OldShoe, 5).AddIngredient(ItemID.TinCan, 5).AddIngredient(ItemID.Goggles).AddIngredient<SulphuricScale>(3), 25);
            PetRecipes.PetRecipe(Recipe.Create(CalamityPetIDs.ElectricTroublemaker).AddIngredient(ItemID.FallenStar).AddIngredient(ItemID.Glass, 10).AddIngredient<PrismShard>(5).AddIngredient<DemonicBoneAsh>().AddRecipeGroup(RecipeGroupsForPets.IceBlocks, 10).AddIngredient(ItemID.JungleSpores, 3).AddIngredient(ItemID.Cloud, 10));
            PetRecipes.PetRecipe(Recipe.Create(CalamityPetIDs.EscargidolonSnail).AddIngredient(ItemID.TurtleShell, 3).AddIngredient(ItemID.JunoniaShell).AddIngredient(ItemID.LightningWhelkShell, 5).AddIngredient(ItemID.TulipShell, 5).AddIngredient(ItemID.Seashell, 200).AddIngredient(ItemID.Seashell, 200).AddIngredient<Voidstone>(100).AddIngredient<AnechoicCoating>(5).AddRecipeGroup(RecipeGroupID.Snails).AddIngredient<ReaperTooth>(6), 9000);
            PetRecipes.PetRecipe(Recipe.Create(CalamityPetIDs.FlakHermit).AddIngredient<CorrodedFossil>(10).AddIngredient<SulphurousSand>(100).AddIngredient<Acidwood>(125).AddIngredient(ItemID.GeyserTrap),100);
            PetRecipes.MasterModePetRecipe(Recipe.Create(CalamityPetIDs.Fox).AddIngredient<AuricBar>(5).AddIngredient<EffulgentFeather>(70).AddIngredient(ItemID.BrownDye).AddIngredient(ItemID.SilverDye).AddIngredient(ItemID.BlackDye),20000);
            PetRecipes.PetRecipe(Recipe.Create(CalamityPetIDs.FurtasticDuo).AddIngredient(ItemID.DemonWings).AddRecipeGroup(RecipeGroupID.Dragonflies).AddIngredient(ItemID.AngelWings).AddIngredient(ItemID.PixieDust,5), 9999);
            PetRecipes.PetRecipe(Recipe.Create(CalamityPetIDs.Kendra).AddIngredient(ItemID.Feather, 8).AddIngredient(ItemID.AngelStatue), 800);
            PetRecipes.PetRecipe(Recipe.Create(CalamityPetIDs.LadShark).AddIngredient(ItemID.SharkFin,6).AddIngredient(ItemID.LifeCrystal).AddIngredient(ItemID.HeartStatue), 100);
            PetRecipes.MasterModePetRecipe(Recipe.Create(CalamityPetIDs.Levi).AddIngredient<DepthCells>(100).AddIngredient<Lumenyl>(150).AddIngredient<ShadowspecBar>(3),20000,5);
            PetRecipes.PetRecipe(Recipe.Create(CalamityPetIDs.MiniHiveMind).AddIngredient<RottenMatter>(125).AddIngredient(ItemID.VilePowder,100).AddIngredient<BlightedGel>(25), 300);
            PetRecipes.PetRecipe(Recipe.Create(CalamityPetIDs.MiniPerforator).AddIngredient<BloodSample>(125).AddIngredient(ItemID.ViciousPowder, 100).AddIngredient<BlightedGel>(25), 300);
            PetRecipes.PetRecipe(Recipe.Create(CalamityPetIDs.Pineapple).AddIngredient(ItemID.Pineapple).AddIngredient(ItemID.Goldfish,400), 1000);
            PetRecipes.PetRecipe(Recipe.Create(CalamityPetIDs.PlagueBringerBab).AddIngredient<PlagueCellCanister>(225).AddIngredient(ItemID.Stinger,50).AddIngredient<InfectedArmorPlating>(100),3000);
            PetRecipes.PetRecipe(Recipe.Create(CalamityPetIDs.SonOfYharon).AddIngredient(ItemID.ChickenNugget,3).AddIngredient<EffulgentFeather>(40).AddIngredient<YharonSoulFragment>(30).AddIngredient<ScoriaBar>(25).AddIngredient<AshesofCalamity>(25),15000);
            PetRecipes.MasterModePetRecipe(Recipe.Create(CalamityPetIDs.SupremeCalamitas).AddIngredient<AshesofAnnihilation>(25).AddIngredient<AshesofCalamity>(100).AddIngredient(ItemID.LargeRuby).AddIngredient(ItemID.ArcaneCrystal,50),25000);
            PetRecipes.PetRecipe(Recipe.Create(CalamityPetIDs.ThirdSage).AddIngredient(ItemID.SoulofLight,5).AddIngredient(ItemID.SoulofNight,5).AddIngredient(ItemID.GuideVoodooDoll).AddIngredient<BloodOrb>(100).AddIngredient(ItemID.BottledWater,100),100);

            PetRecipes.PetRecipe(Recipe.Create(CalamityLightPetIDs.BabyGhostBell).AddIngredient<PrismShard>(4).AddIngredient<SeaPrism>(6).AddIngredient<Navystone>(25).AddIngredient<EutrophicSand>(8),30);
            PetRecipes.PetRecipe(Recipe.Create(CalamityLightPetIDs.Lilorde).AddIngredient(ItemID.NeonTetra,4).AddIngredient(ItemID.OrichalcumHalberd,2).AddIngredient(ItemID.UnicornonaStick).AddIngredient<DemonshadeBreastplate>(6).AddIngredient<HalibutCannon>(9).AddCondition(Condition.BestiaryFilledPercent(100)).AddCondition(Condition.ZenithWorld),2);
            PetRecipes.PetRecipe(Recipe.Create(CalamityLightPetIDs.Goldie).AddIngredient(ItemID.GoldCoin,99).AddIngredient(ItemID.GoldenPlatform,10));
            PetRecipes.PetRecipe(Recipe.Create(CalamityLightPetIDs.LittleLight).AddIngredient<ArmoredShell>().AddIngredient<DarkPlasma>().AddIngredient<TwistingNether>().AddIngredient(ItemID.Nanites,450).AddIngredient<UnholyEssence>(5),200);
            PetRecipes.PetRecipe(Recipe.Create(CalamityLightPetIDs.OceanSpirit).AddIngredient(ItemID.WhitePearl).AddIngredient<AbyssGravel>(10).AddIngredient<Voidstone>(2).AddIngredient<PearlShard>(3).AddCondition(Condition.DownedSkeletron),40);
            PetRecipes.PetRecipe(Recipe.Create(CalamityLightPetIDs.Radiator).AddIngredient<SulphuricScale>(12).AddIngredient<Acidwood>(25).AddIngredient<SulphurousSand>(10),50);
            PetRecipes.PetRecipe(Recipe.Create(CalamityLightPetIDs.Sparks).AddIngredient(ItemID.ButterflyWings),100);
            PetRecipes.PetRecipe(Recipe.Create(CalamityLightPetIDs.Yuu).AddIngredient<AstralBar>(6).AddIngredient<GalacticaSingularity>(4).AddIngredient<MeldConstruct>(2).AddIngredient<LifeAlloy>(5).AddIngredient<CoreofHavoc>().AddIngredient<CoreofEleum>().AddIngredient<CoreofSunlight>().AddIngredient<CoreofCalamity>(),200);
        }
    }
}