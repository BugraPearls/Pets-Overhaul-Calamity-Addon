using CalamityMod.Buffs.Summon;
using PetsOverhaulCalamityAddon.Systems;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Projectiles.Summon;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Threading;
using CalamityMod.Buffs.DamageOverTime;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using CalamityMod.Sounds;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class SonOfYharonEffect : PetEffect
    {
        public float dmgRebirth = 0.2f;
        public int defRebirth = 16;
        public float msRebirth = 0.12f;
        public int fireTime = 90;
        public int rebirthDuration = 900;
        public int rebirthCooldown = 7200;
        private int timer = 0;
        private int deadTimer = 0;
        private float healthToMult = 1f;
        private int damageToTakeAfterReborn = 0;
        private PlayerDeathReason reason;
        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override PetClasses PetClassSecondary => PetClasses.Utility;
        public override void PreUpdate()
        {
            if (Pet.PetInUse(CalamityPetIDs.SonOfYharon))
            {
                Pet.SetPetAbilityTimer(rebirthCooldown);
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (Pet.PetInUseWithSwapCd(CalamityPetIDs.SonOfYharon))
            {
                timer--;
                deadTimer--;
                if (timer > 0)
                {
                    Player.GetDamage<GenericDamageClass>() += dmgRebirth * healthToMult;
                    Player.statDefense += (int)Math.Ceiling(healthToMult * defRebirth);
                    Player.moveSpeed += healthToMult * msRebirth;
                }
                if (deadTimer <= 0)
                {
                    if (deadTimer == 0)
                    {
                        switch (Main.rand.Next(4))
                        {
                            case 0:
                                reason = PlayerDeathReason.ByCustomReason(Player.name + " ran out of flames, and all that is left is ashes.");
                                break;
                            case 1:
                                reason = PlayerDeathReason.ByCustomReason(Player.name + " was punished by Yharon for abusing his kid.");
                                break;
                            case 2:
                                reason = PlayerDeathReason.ByCustomReason("Son of Yharon seems like isn't capable enough, and couldn't delay " + Player.name + "'s death any longer.");
                                break;
                            case 3:
                                reason = PlayerDeathReason.ByCustomReason(Player.name + " should've learned to Reborn before its was too late.");
                                break;
                            default:
                                break;
                        }
                        Player.KillMe(reason, Player.statLifeMax2, 0);
                    }
                    deadTimer = -1;
                }
                if (deadTimer > 0)
                {
                    Player.GetDamage<GenericDamageClass>() += dmgRebirth;
                    Player.statDefense += defRebirth;
                    Player.moveSpeed += msRebirth;
                }
                if (timer <= 0)
                {
                    if (timer == 0)
                    {
                        switch (Main.rand.Next(4))
                        {
                            case 0:
                                reason = PlayerDeathReason.ByCustomReason(Player.name + " could not contain temporary power given by Yharon.");
                                break;
                            case 1:
                                reason = PlayerDeathReason.ByCustomReason("Yharon's son is sad for "+Player.name);
                                break;
                            case 2:
                                reason = PlayerDeathReason.ByCustomReason("Powers of Rebirth should've been handled only by the Eternal Phoenixes, " + Player.name + ", was not the one to handle.");
                                break;
                            case 3:
                                reason = PlayerDeathReason.ByCustomReason(Player.name + " tried to eat the nuggies???");
                                break;
                            default:
                                break;
                        }
                        Player.Hurt(reason, damageToTakeAfterReborn, 0, dodgeable: true, knockback: 0);
                        damageToTakeAfterReborn = 0;
                    }
                    timer = -1;
                    healthToMult = 1f;
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUseWithSwapCd(CalamityPetIDs.SonOfYharon) && (timer > 0 || deadTimer > 0))
            {
                target.AddBuff(ModContent.BuffType<Dragonfire>(), (int)Math.Ceiling(fireTime * (deadTimer > 0 ? 1f : healthToMult)));
            }
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.AbilityPressCheck() && Pet.PetInUseWithSwapCd(CalamityPetIDs.SonOfYharon))
            {
                if (ModContent.GetInstance<Personalization>().AbilitySoundEnabled)
                {
                    SoundEngine.PlaySound(new SoundStyle("CalamityMod/Sounds/Custom/Yharon/YharonRoar") with { PitchVariance = 0.2f }, Player.Center) ;
                }
                PopupText.NewText(new AdvancedPopupRequest() with { Text = "REBORN!!", DurationInFrames = 150, Velocity = new Vector2(0, -10), Color = new Color(209, 107, 75) }, Player.Center);
                float playerCurrentHp = Player.statLife;
                if (Player.statLifeMax2 / Player.statLife > 4)
                {
                    playerCurrentHp = Player.statLifeMax2 / 4;
                }
                healthToMult = Player.statLifeMax2 / playerCurrentHp / 2;
                int missingHp = Player.statLifeMax2 - Player.statLife;
                damageToTakeAfterReborn = missingHp;
                Pet.petShield.Add((missingHp, (int)Math.Ceiling(rebirthDuration * healthToMult)));
                timer = (int)Math.Ceiling(rebirthDuration * healthToMult);
                Pet.timer = Pet.timerMax;
            }
        }
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource)
        {
            timer = 0;
            if (Pet.PetInUseWithSwapCd(CalamityPetIDs.SonOfYharon) && Pet.timer<= 0)
            {
                if (ModContent.GetInstance<Personalization>().AbilitySoundEnabled)
                {
                    SoundEngine.PlaySound(new SoundStyle("CalamityMod/Sounds/Custom/Yharon/YharonRoarShort") with { PitchVariance = 0.3f }, Player.Center);
                }
                PopupText.NewText(new AdvancedPopupRequest() with { Text = "Son of Yharon is delaying your death!", DurationInFrames = 150, Velocity = new Vector2(0, -10), Color = new Color(209, 107, 75) }, Player.Center);
                Pet.PetRecovery(Player.statLifeMax2, 1f, isLifesteal: false);
                deadTimer = rebirthDuration;
                Pet.timer = Pet.timerMax;
                return false;
            }
            else
                return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genDust, ref damageSource);
        }
    }
    public sealed class McNuggetsTooltip : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == CalamityPetIDs.SonOfYharon;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().EnableTooltipToggle && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }
            SonOfYharonEffect yharon = Main.LocalPlayer.GetModPlayer<SonOfYharonEffect>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.McNuggets")
                .Replace("<class>", PetTextsColors.ClassText(yharon.PetClassPrimary, yharon.PetClassSecondary))
                        .Replace("<keybind>", PetTextsColors.KeybindText(Keybinds.UsePetAbility))
                        .Replace("<dmgRebirth>", Math.Round(yharon.dmgRebirth * 100, 2).ToString())
                        .Replace("<defRebirth>", yharon.defRebirth.ToString())
                        .Replace("<msRebirth>", Math.Round(yharon.msRebirth * 100, 2).ToString())
                        .Replace("<fireTime>", Math.Round(yharon.fireTime / 60f, 2).ToString())
                        .Replace("<rebornDuration>", Math.Round(yharon.rebirthDuration / 60f, 2).ToString())
                        .Replace("<cooldown>", Math.Round(yharon.rebirthCooldown / 3600f, 2).ToString())
            ));
        }
    }
}
