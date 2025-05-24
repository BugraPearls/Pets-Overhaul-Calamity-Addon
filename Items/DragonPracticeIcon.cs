using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.Items
{
    /// <summary>
    /// Credit of Icon goes to Riot Games, taken by Smolder's Passive Icon and resized to 30x32. This Items only purpose is for Akato's tooltip to have Icons for representing Stack buffs. 
    /// </summary>
    public class DragonPracticeIcon : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatShouldNotBeInInventory[Type] = true;
            //ItemID.Sets.Deprecated[Type] = true; //This doesn't work; since it doesn't let us see the icon of item when this is set to true.
        }
        public override void PostUpdate()
        {
            Item.TurnToAir(true);
        }
        public override void UpdateInventory(Player player)
        {
            Item.TurnToAir(true);
        }
    }
}
