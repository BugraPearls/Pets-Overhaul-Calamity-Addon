using CalamityMod.CalPlayer;
using PetsOverhaul.PetEffects;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.Systems
{
    public class CalamityPetsModPlayer : ModPlayer
    {
        CalamityPlayer Calamity => Player.GetModPlayer<CalamityPlayer>();
        GlobalPet Pet => Player.GetModPlayer<GlobalPet>();
        public override void UpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(ItemID.SkeletronPetItem) && Calamity.chaliceOfTheBloodGod)
            {
                Calamity.chaliceOfTheBloodGod = false;
            }
        }
    }
}
