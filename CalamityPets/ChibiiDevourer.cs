using PetsOverhaulCalamityAddon.Systems;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using System;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Audio;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class ChibiiDevourerEffect : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Supportive;
        public int dmg = 99999;
        public int block = 320;
        public bool check = false;
        public override void PreUpdate()
        {
            if (Pet.PetInUse(CalamityPetIDs.ChibiiDevourer) && Player.difficulty == PlayerDifficultyID.Hardcore)
                check = true;
            else 
                check = false;
        }
        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (check)
            {
                SoundEngine.PlaySound(SoundID.Meowmere, Player.Center);
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Player.Distance(Main.npc[i].Center) < block)
                    {
                        Main.npc[i].SimpleStrikeNPC(dmg, Player.direction, true, 10f);
                    }
                }
            }
        }
    }
    public sealed class CosmicPlushieTooltip : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == CalamityPetIDs.ChibiiDevourer;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }

            ChibiiDevourerEffect chibiiDevourer = Main.LocalPlayer.GetModPlayer<ChibiiDevourerEffect>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.CosmicPlushie")
                .Replace("<class>", PetTextsColors.ClassText(chibiiDevourer.PetClassPrimary, chibiiDevourer.PetClassSecondary))
                .Replace("<color>", PetTextsColors.SupportiveClass.Hex3())
                .Replace("<damage>", chibiiDevourer.dmg.ToString())
                .Replace("<block>", Math.Round(chibiiDevourer.block / 16f, 2).ToString())
            ));
        }
    }
}
