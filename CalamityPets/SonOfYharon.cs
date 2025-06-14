using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class SonOfYharonEffect : PetEffect
    {
        public override int PetItemID => CalamityPetIDs.SonOfYharon;
        public float dmgRebirth = 0.18f;
        public int defRebirth = 15;
        public float msRebirth = 0.1f;
        public int fireTime = 90;
        public int rebirthDuration = 900;
        public int rebirthCooldown = 7200;
        private int timer = 0;
        private int deadTimer = 0;
        private float healthToMult = 1f;
        private int damageToTakeAfterReborn = 0;
        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override PetClasses PetClassSecondary => PetClasses.Utility;
        public override int PetAbilityCooldown => rebirthCooldown;
        public override int PetStackCurrent => timer + deadTimer + 2; //One or the other will be shown, and neither should be different than 0 at anytime when another is active, so should be fine. And a +2 because both default to -1 every frame.
        public override int PetStackMax => 0;
        public override string PetStackSpecial => PetTextsColors.SecondsOutOfText(timer + deadTimer + 2, 0);
        public override string PetStackText => Compatibility.LocVal("PetTooltips.McNuggetsStack");
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
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
                        string reason = Main.rand.Next(4) switch
                        {
                            0 => Compatibility.LocVal("PetTooltips.YharonDeath1"),
                            1 => Compatibility.LocVal("PetTooltips.YharonDeath2"),
                            2 => Compatibility.LocVal("PetTooltips.YharonDeath3"),
                            3 => Compatibility.LocVal("PetTooltips.YharonDeath4"),
                            _ => Compatibility.LocVal("PetTooltips.YharonDeath1"),
                        };
                        Player.KillMe(PlayerDeathReason.ByCustomReason(reason.Replace("<name>", Player.name)), Player.statLifeMax2, 0);
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
                        string reason = Main.rand.Next(4) switch
                        {
                            0 => Compatibility.LocVal("PetTooltips.YharonDeath5"),
                            1 => Compatibility.LocVal("PetTooltips.YharonDeath6"),
                            2 => Compatibility.LocVal("PetTooltips.YharonDeath7"),
                            3 => Compatibility.LocVal("PetTooltips.YharonDeath8"),
                            _ => Compatibility.LocVal("PetTooltips.YharonDeath5"),
                        };
                        Player.Hurt(PlayerDeathReason.ByCustomReason(reason.Replace("<name>", Player.name)), damageToTakeAfterReborn, 0, dodgeable: true, knockback: 0);
                        damageToTakeAfterReborn = 0;
                    }
                    timer = -1;
                    healthToMult = 1f;
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PetIsEquipped() && (timer > 0 || deadTimer > 0))
            {
                target.AddBuff(ModContent.BuffType<Dragonfire>(), (int)Math.Ceiling(fireTime * (deadTimer > 0 ? 1f : healthToMult)));
            }
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.AbilityPressCheck() && PetIsEquipped())
            {
                if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                {
                    SoundEngine.PlaySound(new SoundStyle("CalamityMod/Sounds/Custom/Yharon/YharonRoar") with { PitchVariance = 0.2f }, Player.Center);
                }
                PopupText.NewText(new AdvancedPopupRequest() with { Text = Compatibility.LocVal("PetTooltips.YharonReborn"), DurationInFrames = 150, Velocity = new Vector2(0, -10), Color = new Color(209, 107, 75) }, Player.Center);
                float playerCurrentHp = Player.statLife;
                if (Player.statLifeMax2 / Player.statLife > 4)
                {
                    playerCurrentHp = Player.statLifeMax2 / 4;
                }
                healthToMult = Player.statLifeMax2 / playerCurrentHp / 2;
                int missingHp = Player.statLifeMax2 - Player.statLife;
                damageToTakeAfterReborn = missingHp;
                Pet.AddShield(missingHp, (int)Math.Ceiling(rebirthDuration * healthToMult));
                timer = (int)Math.Ceiling(rebirthDuration * healthToMult);
                Pet.timer = Pet.timerMax;
            }
        }
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource)
        {
            timer = 0;
            if (PetIsEquipped() && Pet.timer <= 0)
            {
                if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                {
                    SoundEngine.PlaySound(new SoundStyle("CalamityMod/Sounds/Custom/Yharon/YharonRoarShort") with { PitchVariance = 0.3f }, Player.Center);
                }
                PopupText.NewText(new AdvancedPopupRequest() with { Text = Compatibility.LocVal("PetTooltips.YharonDelay"), DurationInFrames = 150, Velocity = new Vector2(0, -10), Color = new Color(209, 107, 75) }, Player.Center);
                Pet.PetRecovery(Player.statLifeMax2, 1f, isLifesteal: false);
                deadTimer = rebirthDuration;
                Pet.timer = Pet.timerMax;
                return false;
            }
            else
                return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genDust, ref damageSource);
        }
    }
    public sealed class McNuggetsTooltip : PetTooltip
    {
        public override PetEffect PetsEffect => yharon;
        public static SonOfYharonEffect yharon
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out SonOfYharonEffect pet))
                    return pet;
                else
                    return ModContent.GetInstance<SonOfYharonEffect>();
            }
        }
        public override string PetsTooltip => Compatibility.LocVal("PetTooltips.McNuggets")
                        .Replace("<keybind>", PetTextsColors.KeybindText(PetKeybinds.UsePetAbility))
                        .Replace("<dmgRebirth>", Math.Round(yharon.dmgRebirth * 100, 2).ToString())
                        .Replace("<defRebirth>", yharon.defRebirth.ToString())
                        .Replace("<msRebirth>", Math.Round(yharon.msRebirth * 100, 2).ToString())
                        .Replace("<fireTime>", Math.Round(yharon.fireTime / 60f, 2).ToString())
                        .Replace("<rebornDuration>", Math.Round(yharon.rebirthDuration / 60f, 2).ToString())
                        .Replace("<cooldown>", Math.Round(yharon.rebirthCooldown / 3600f, 2).ToString());
        public override string SimpleTooltip => Compatibility.LocVal("SimpleTooltips.McNuggets").Replace("<keybind>", PetTextsColors.KeybindText(PetKeybinds.UsePetAbility));
    }
}
