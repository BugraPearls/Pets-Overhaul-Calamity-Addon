using CalamityMod.Items.Pets;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class PineappleEffect : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.None;
        public override void PostUpdateEquips()
        {

        }
    }
    public sealed class PineappleItemTooltip : GlobalItem
    {

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ModContent.ItemType<PineapplePet>();
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().DisableTooltipToggle == false && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            PineappleEffect pineapple = Main.LocalPlayer.GetModPlayer<PineappleEffect>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.PineapplePet")
                .Replace("<class>", PetColors.ClassText(pineapple.PetClassPrimary, pineapple.PetClassSecondary))
            ));
        }
    }
}
