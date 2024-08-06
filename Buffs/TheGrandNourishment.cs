using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.Buffs
{
    public class TheGrandNourishment : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsWellFed[Type] = true;
        }
        public override LocalizedText Description => Language.GetText("Mods.PetsOverhaulCalamityAddon.Buffs.TheGrandNourishmentTooltip");
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(BuffID.WellFed)|| player.HasBuff(BuffID.WellFed2) || player.HasBuff(BuffID.WellFed3))
            {
                player.ClearBuff(BuffID.WellFed);
                player.ClearBuff(BuffID.WellFed2);
                player.ClearBuff(BuffID.WellFed3);
            }
        }
    }
}