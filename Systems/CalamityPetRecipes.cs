using CalamityMod.Items;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using CalamityMod.NPCs.TownNPCs;
using PetsOverhaul.Items;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.Systems
{
    public class CalamityPetRecipes : ModSystem
    {
        public override void AddRecipes()
        {
            PetRecipes.MasterPetCraft(CalamityPetIDs.Brimling, ModContent.ItemType<BrimstoneElementalTrophy>());
            PetRecipes.MasterPetCraft(CalamityPetIDs.Fox, ModContent.ItemType<YharonTrophy>());
            PetRecipes.MasterPetCraft(CalamityPetIDs.SupremeCalamitas, ModContent.ItemType<SupremeCalamitasTrophy>());
            Recipe.Create(CalamityPetIDs.Levi).AddIngredient(ModContent.ItemType<MasteryShard>(), 5).AddIngredient(ModContent.ItemType<SupremeCalamitasTrophy>()).Register();
            Recipe.Create(CalamityPetIDs.ChibiiDevourer).AddIngredient(ModContent.ItemType<MasteryShard>(), 5).AddIngredient(ModContent.ItemType<DevourerofGodsTrophy>()).AddCondition(Condition.BestiaryFilledPercent(100)).AddCondition(Condition.ZenithWorld).AddCondition(Condition.PlayerCarriesItem(ModContent.ItemType<Rock>())).AddCondition(Condition.NpcIsPresent(ModContent.NPCType<FAP>())).Register();
        }
    }
}
