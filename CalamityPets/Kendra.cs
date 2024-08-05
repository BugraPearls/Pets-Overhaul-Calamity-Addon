using CalamityMod.Items.Pets;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class KendraEffect : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.None;
        public override void PostUpdateEquips()
        {

        }
    }
    public sealed class RomajedaOrchidTooltip : GlobalItem
    {

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ModContent.ItemType<RomajedaOrchid>();
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().DisableTooltipToggle == false && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            KendraEffect kendra = Main.LocalPlayer.GetModPlayer<KendraEffect>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.RomajedaOrchid")
                .Replace("<class>", PetColors.ClassText(kendra.PetClassPrimary, kendra.PetClassSecondary))
            ));
        }
    }
}
