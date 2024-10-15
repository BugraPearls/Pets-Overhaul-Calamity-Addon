using PetsOverhaulCalamityAddon.Systems;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class ElectricTroublemakerEffect : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.None;
        public override void PostUpdateMiscEffects()
        {

        }
    }
    public sealed class TheEtomerTooltip : GlobalItem
    {

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == CalamityPetIDs.ElectricTroublemaker;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }
            ElectricTroublemakerEffect rotom = Main.LocalPlayer.GetModPlayer<ElectricTroublemakerEffect>();

            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.TheEtomer")
                    .Replace("<class>", PetTextsColors.ClassText(rotom.PetClassPrimary, rotom.PetClassSecondary))
                ));
        }
    }
}
