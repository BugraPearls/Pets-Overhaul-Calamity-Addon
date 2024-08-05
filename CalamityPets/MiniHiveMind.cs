using CalamityMod.Items.Pets;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class MiniHiveMindEffect : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.None;
        public override void PostUpdateEquips()
        {

        }
    }
    public sealed class RottingEyeballTooltip : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ModContent.ItemType<RottingEyeball>();
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().DisableTooltipToggle == false && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }



            MiniHiveMindEffect hive = Main.LocalPlayer.GetModPlayer<MiniHiveMindEffect>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.RottingEyeball")
                    .Replace("<class>", PetColors.ClassText(hive.PetClassPrimary, hive.PetClassSecondary))
                ));
        }
    }
}
