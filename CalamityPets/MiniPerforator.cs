using CalamityMod.Items.Pets;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class MiniPerforatorEffect : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.None;
        public override void PostUpdateEquips()
        {

        }
    }
    public sealed class BloodyVeinTooltip : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ModContent.ItemType<BloodyVein>();
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().DisableTooltipToggle == false && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            MiniPerforatorEffect perforator = Main.LocalPlayer.GetModPlayer<MiniPerforatorEffect>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.BloodyVein")
                    .Replace("<class>", PetColors.ClassText(perforator.PetClassPrimary, perforator.PetClassSecondary))
                ));
        }
    }
}
