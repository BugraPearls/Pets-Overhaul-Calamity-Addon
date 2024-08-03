using CalamityMod.CalPlayer;
using PetsOverhaul.Systems;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.Systems
{
    public class CalamityPetsModPlayer : ModPlayer
    {
        public override void UpdateEquips()
        {
            CalamityPlayer calamityPlayer = Player.GetModPlayer<CalamityPlayer>();
            if (Player.miscEquips[0].type == ItemID.SkeletronPetItem && calamityPlayer.chaliceOfTheBloodGod)
            {
                calamityPlayer.chaliceOfTheBloodGod = false;
            }
        }
    }
}
