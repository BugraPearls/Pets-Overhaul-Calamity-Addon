using PetsOverhaulCalamityAddon.Systems;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using CalamityMod;
using CalamityMod.Projectiles.Pets;
using CalamityMod.Buffs.Pets;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class FurtasticDuoEffect : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Rogue;
        public override PetClasses PetClassSecondary => PetClasses.Defensive;

        public float stealthMoving = 0.25f;
        public float stealthNotMoving = 0.05f;
        public float hpTreshold = 0.2f;
        public float baseHpShield = 0.13f;
        public float bonusHpShield = 0.15f;
        public int shieldDuration = 900;
        public int cooldown = 3000;
        private int lifeguardMultTimer = 0;

        public float absorbPercent = 1.5f;
        public float lifeguardMult = 1.15f;
        public int currentNextDamage = 0;
        public int procShield = 3;
        public int procShieldDuration = 150;
        public override void PreUpdate()
        {
            if (Pet.PetInUse(CalamityPetIDs.FurtasticDuo))
            {
                Pet.SetPetAbilityTimer(cooldown);
                lifeguardMultTimer--;
                if (lifeguardMultTimer < 0) 
                {
                    lifeguardMultTimer = 0;
                }
            }
        }
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(CalamityPetIDs.FurtasticDuo))
            {
                Player.Calamity().stealthGenMoving += stealthMoving;
                Player.Calamity().stealthGenStandstill += stealthNotMoving;
            }
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (Pet.PetInUseWithSwapCd(CalamityPetIDs.FurtasticDuo) && Pet.timer <= 0)
            {
                int shieldAmount = (int)((Player.statLifeMax * baseHpShield + (Player.statLifeMax2 - Player.statLifeMax) * bonusHpShield) * Pet.petShieldMultiplier);

                modifiers.ModifyHurtInfo += (ref Player.HurtInfo info) =>
                {
                    if (info.Damage >= (Player.statLife - Player.statLifeMax2 * hpTreshold))
                    {
                        int belowHpDamage = (int)(Player.statLifeMax2 * hpTreshold - (Player.statLife - info.Damage));
                        int reduce = info.Damage - 1;

                        reduce = Math.Min(reduce, belowHpDamage);
                        reduce = Math.Min(reduce, shieldAmount);

                        if (reduce > 0)
                        {
                            CombatText.NewText(Player.getRect(), Color.DarkGreen, -reduce);
                        }
                        info.Damage -= reduce;
                        Pet.AddShield(shieldAmount - reduce, shieldDuration, false);
                        Pet.timer = Pet.timerMax;
                        lifeguardMultTimer = shieldDuration;
                    }
                };
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUseWithSwapCd(CalamityPetIDs.FurtasticDuo) && target.active == false && hit.DamageType is RogueDamageClass)
            {
                if ((target.damage * absorbPercent) > currentNextDamage)
                    currentNextDamage = (int)(target.damage * absorbPercent);
            }
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Pet.PetInUseWithSwapCd(CalamityPetIDs.FurtasticDuo) && GlobalPet.LifestealCheck(target) && modifiers.DamageType is RogueDamageClass)
            {
                modifiers.FlatBonusDamage += currentNextDamage * (lifeguardMultTimer > 0 ? lifeguardMult : 1f);
                currentNextDamage = 0;
                Pet.AddShield(procShield,procShieldDuration);
            }
        }
    }
    public sealed class PrimroseKeepsakeTooltip : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == CalamityPetIDs.FurtasticDuo;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }

            FurtasticDuoEffect duo = Main.LocalPlayer.GetModPlayer<FurtasticDuoEffect>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.PrimroseKeepsake")
                .Replace("<class>", PetTextsColors.ClassText(duo.PetClassPrimary, duo.PetClassSecondary))
                .Replace("<belowHealth>", Math.Round(duo.hpTreshold * 100, 2).ToString())
                .Replace("<baseShield>", Math.Round(duo.baseHpShield * 100, 2).ToString())
                .Replace("<bonusShield>", Math.Round(duo.bonusHpShield * 100, 2).ToString())
                .Replace("<shieldDuration>", Math.Round(duo.shieldDuration / 60f, 2).ToString())
                .Replace("<cooldown>", Math.Round(duo.cooldown / 60f, 2).ToString())
                .Replace("<stealthMoving>", Math.Round(duo.stealthMoving * 100, 2).ToString())
                .Replace("<stealthNotMoving>", Math.Round(duo.stealthNotMoving * 100, 2).ToString())

                .Replace("<percAbsorb>", Math.Round(duo.absorbPercent * 100, 2).ToString())
                .Replace("<lifeguardMult>", duo.lifeguardMult.ToString())
                .Replace("<storedDmg>", duo.currentNextDamage.ToString())
                .Replace("<procShield>", duo.procShield.ToString())
                .Replace("<procShieldDuration>", Math.Round(duo.procShieldDuration / 60f, 2).ToString())
            ));
        }
    }
}
