using PetsOverhaul.TownPets;
using System;
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
    }
}