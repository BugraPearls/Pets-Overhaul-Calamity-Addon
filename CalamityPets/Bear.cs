using CalamityMod;
using Microsoft.Xna.Framework;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class BearEffect : PetEffect
    {
        public float rogueDmg = 0.05f;
        public float stealthDmg = 0.03f;
        public float stealthMoving = 0.2f;
        public float stealthNotMoving = 0.06f;

        public float hpTreshold = 0.2f;
        public float baseHpShield = 0.15f;
        public float bonusHpShield = 0.2f;
        public int shieldDuration = 900;
        public int cooldown = 3000;
        public override PetClasses PetClassPrimary => PetClasses.Rogue;
        public override PetClasses PetClassSecondary => PetClasses.Defensive;
        public override void PreUpdate()
        {
            if (Pet.PetInUse(CalamityPetIDs.Bear))
            {
                Pet.SetPetAbilityTimer(cooldown);
            }
        }
        public override void PostUpdateEquips()
        {
            if (Pet.PetInUseWithSwapCd(CalamityPetIDs.Bear))
            {
                Player.GetDamage<RogueDamageClass>() += rogueDmg;
                Player.Calamity().bonusStealthDamage += stealthDmg;
                Player.Calamity().stealthGenMoving += stealthMoving;
                Player.Calamity().stealthGenStandstill += stealthNotMoving;
            }
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (Pet.PetInUseWithSwapCd(CalamityPetIDs.Bear) && Pet.timer <= 0)
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
                    }
                };
            }
        }
    }
    public sealed class BearsEyeTooltip : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == CalamityPetIDs.Bear;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }

            BearEffect bear = Main.LocalPlayer.GetModPlayer<BearEffect>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.BearsEye")
                .Replace("<class>", PetTextsColors.ClassText(bear.PetClassPrimary, bear.PetClassSecondary))
                .Replace("<belowHealth>", Math.Round(bear.hpTreshold * 100, 2).ToString())
                .Replace("<baseShield>", Math.Round(bear.baseHpShield * 100, 2).ToString())
                .Replace("<bonusShield>", Math.Round(bear.bonusHpShield * 100, 2).ToString())
                .Replace("<shieldDuration>", Math.Round(bear.shieldDuration / 60f, 2).ToString())
                .Replace("<cooldown>", Math.Round(bear.cooldown / 60f, 2).ToString())
                .Replace("<rogueDmg>", Math.Round(bear.rogueDmg * 100, 2).ToString())
                .Replace("<stealthDmg>", Math.Round(bear.stealthDmg * 100, 2).ToString())
                .Replace("<stealthMoving>", Math.Round(bear.stealthMoving * 100, 2).ToString())
                .Replace("<stealthNotMoving>", Math.Round(bear.stealthNotMoving * 100, 2).ToString())
            ));
        }
    }
}
