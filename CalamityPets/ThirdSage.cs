using CalamityMod.Items.Pets;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class ThirdSageEffect : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.None;
        public override void PostUpdateEquips()
        {

        }
    }
    public sealed class HermitsBoxofOneHundredMedicinesTooltip : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ModContent.ItemType<HermitsBoxofOneHundredMedicines>();
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().DisableTooltipToggle == false && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }
            ThirdSageEffect sage = Main.LocalPlayer.GetModPlayer<ThirdSageEffect>();

            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.HermitsBoxofOneHundredMedicines")
                    .Replace("<class>", PetColors.ClassText(sage.PetClassPrimary, sage.PetClassSecondary))
                ));
        }
    }
}
