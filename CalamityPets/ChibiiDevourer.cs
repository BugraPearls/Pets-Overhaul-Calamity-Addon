using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class ChibiiDevourerEffect : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Supportive;
        public int dmg = 99999;
        public int block = 320;
        public float kb = 100f;
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
                        Main.npc[i].SimpleStrikeNPC(dmg, Player.direction, true, kb);
                    }
                }
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    if (Player.Distance(Main.player[i].Center) < block)
                    {
                        string reason;
                        switch (Main.rand.Next(4))
                        {
                            case 0:
                                reason = Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.CosmicDeath1");
                                break;
                            case 1:
                                reason = Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.CosmicDeath2");
                                break;
                            case 2:
                                reason = Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.CosmicDeath3");
                                break;
                            case 3:
                                reason = Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.CosmicDeath4");
                                break;
                            default:
                                reason = Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.CosmicDeath1");
                                break;
                        }
                        Main.player[i].Hurt(PlayerDeathReason.ByCustomReason(reason.Replace("<name>", Player.name)), dmg, Player.direction, true, dodgeable: false, scalingArmorPenetration: 1f, knockback: kb);
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
