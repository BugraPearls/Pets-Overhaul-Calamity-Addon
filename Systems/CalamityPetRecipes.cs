using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Fishing.AstralCatches;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Mounts;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Items.Potions;
using CalamityMod.Items.Potions.Alcohol;
using CalamityMod.Items.SummonItems;
using CalamityMod.NPCs.TownNPCs;
using CalamityMod.Projectiles.Boss;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Projectiles.Pets;
using CalamityMod.Systems;
using CalamityMod.World;
using PetsOverhaul.Items;
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
            PetRecipes.PetRecipe(Recipe.Create(CalamityPetIDs.Akato).AddIngredient<YharonSoulFragment>(30).AddIngredient(ItemID.WizardHat).AddIngredient(ItemID.WingsSolar).AddIngredient<YharonEgg>(),5000);
            PetRecipes.PetRecipe(Recipe.Create(CalamityPetIDs.Astrophage).AddIngredient<ProcyonidPrawn>().AddIngredient(ItemID.WaterStrider).AddIngredient<StarblightSoot>(300).AddIngredient<AstralMonolith>(650), 1500);
            PetRecipes.PetRecipe(Recipe.Create(CalamityPetIDs.Bear).AddIngredient(ItemID.WhitePearl, 2).AddIngredient(ItemID.AngelStatue).AddCondition(Condition.DownedSkeletron), 9999);
            PetRecipes.MasterModePetRecipe(Recipe.Create(CalamityPetIDs.Brimling).AddIngredient<InfernalSuevite>(30).AddIngredient<ScorchedBone>(120).AddIngredient<EssenceofHavoc>(12), 2000);
            PetRecipes.MasterModePetRecipe(Recipe.Create(CalamityPetIDs.ChibiiDevourer).AddIngredient<Fabsol>().AddIngredient<CosmiliteBar>(9999).AddIngredient<FabsolsVodka>(9999).AddIngredient(ItemID.PlatinumCoin, 9999).AddIngredient(ItemID.GoldCoin, 100).AddIngredient(ItemID.SilverCoin, 100).AddIngredient(ItemID.CopperCoin, 100).AddCondition(Condition.InSpace), 9999, 9);
            PetRecipes.PetRecipe(Recipe.Create(CalamityPetIDs.DannyDevito).AddIngredient(ItemID.TrashCan, 10).AddIngredient(ItemID.FishingSeaweed, 5).AddIngredient(ItemID.OldShoe, 5).AddIngredient(ItemID.TinCan, 5).AddIngredient(ItemID.Goggles).AddIngredient<SulphuricScale>(3), 25);
            PetRecipes.PetRecipe(Recipe.Create(CalamityPetIDs.ElectricTroublemaker).AddIngredient(ItemID.FallenStar).AddIngredient(ItemID.Glass, 10).AddIngredient<PrismShard>(5).AddIngredient<DemonicBoneAsh>().AddRecipeGroup(RecipeGroupsForPets.IceBlocks, 10).AddIngredient(ItemID.JungleSpores, 3).AddIngredient(ItemID.Cloud, 10));
            PetRecipes.PetRecipe(Recipe.Create(CalamityPetIDs.EscargidolonSnail).AddIngredient(ItemID.TurtleShell, 3).AddIngredient(ItemID.JunoniaShell).AddIngredient(ItemID.LightningWhelkShell, 5).AddIngredient(ItemID.TulipShell, 5).AddIngredient(ItemID.Seashell, 200).AddIngredient(ItemID.Seashell, 200).AddIngredient<Voidstone>(100).AddIngredient<AnechoicCoating>(5).AddRecipeGroup(RecipeGroupID.Snails).AddIngredient<ReaperTooth>(6), 7500);

            PetRecipes.PetRecipe(Recipe.Create(CalamityPetIDs.FurtasticDuo).AddIngredient(ItemID.DemonWings).AddRecipeGroup(RecipeGroupID.Dragonflies).AddIngredient(ItemID.AngelWings).AddIngredient(ItemID.PixieDust,5), 9999);

            PetRecipes.MasterModePetRecipe(Recipe.Create(CalamityPetIDs.Fox));
            PetRecipes.MasterModePetRecipe(Recipe.Create(CalamityPetIDs.SupremeCalamitas));
            PetRecipes.MasterModePetRecipe(Recipe.Create(CalamityPetIDs.Levi));
        }
    }
}

//Cal recipe Feedback
//Abyss Shell Fossil: Seashell(x ?), Smooth Voidstone(x10+), Anechoic Coating(x10), Snail(x1), Pet Food(x?)
//Astrophage: Starblight Soot(x250+), Astral Monolith(x?), Water Strider(x1), Pet Food(x?)
//Bear's Eye: White Pearl (x1), Diamond (x15+), Bone (x?), Cat License (x1), Pet Food (x?)
//Bloody Vein: Blood Sample(x25+), Tissue Sample(x15+), Pet Food(x?)
//Rotting Eyeball: Rotten Matter(x25+), Shadow Scale(x15+), Pet Food(x?)
//Brimstone Jewel: Ashes of Calamity (x100+), Ashes of Annihilation (x10+), Large Ruby (x1), Arcane Crystal (x1), Mastery Shard (x1), Pet Food (x?)
//Charred Relic: Essence of Havoc (x10+), Infernal Suevite (x25), Small Spine (x15+), Mastery Shard (x1), Pet Food (x?)
//Cosmic Plushie: Cosmilite(x9999), Devourer of Gods Mask (x96), Princess Spirit in a Bottle (x1), Platinum Coin (x9999), Gold Coin (x100), Silver Coin (x100), Copper Coin (x100), Mastery Shard (5x), Pet Food (x9999)
//Forgotten Dragon Egg: Blessed Phoenix Egg (x1), Yharon Soul Fragment (x25+), McNuggets (x1), Mastery Shard (x2), Pet Food (x?)