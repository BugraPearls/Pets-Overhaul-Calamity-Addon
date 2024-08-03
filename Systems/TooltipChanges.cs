using CalamityMod.Items.Accessories;
using PetsOverhaul.Config;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
namespace PetsOverhaul.Systems
{
    public class ChaliceSkeletronInteraction : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ModContent.ItemType<ChaliceOfTheBloodGod>();
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.Misc.ChaliceOfTheBloodGod")));
        }
    }
}