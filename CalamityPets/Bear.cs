using CalamityMod;
using Microsoft.Xna.Framework;
using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System;
using Terraria;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class BearEffect : PetEffect
    {
        public override int PetItemID => CalamityPetIDs.Bear;
        public float rogueDmg = 0.05f;
        public float stealthDmg = 0.03f;
        public float stealthMoving = 0.2f;
        public float stealthNotMoving = 0.06f;

        public float hpTreshold = 0.2f;
        public float baseHpShield = 0.15f;
        public float bonusHpShield = 0.2f;
        public int shieldDuration = 900;
        public int cooldown = 3000;
        public override PetClass PetClassPrimary => RoguePetClass.Rogue;
        public override PetClass PetClassSecondary => PetClassID.Defensive;
        public override int PetAbilityCooldown => cooldown;
        public override void PostUpdateEquips()
        {
            if (PetIsEquipped())
            {
                Player.GetDamage<RogueDamageClass>() += rogueDmg;
                Player.Calamity().bonusStealthDamage += stealthDmg;
                Player.Calamity().stealthGenMoving += stealthMoving;
                Player.Calamity().stealthGenStandstill += stealthNotMoving;
            }
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (PetIsEquipped() && Pet.timer <= 0)
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
    public sealed class BearsEyeTooltip : PetTooltip
    {
        public override PetEffect PetsEffect => bear;
        public static BearEffect bear
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out BearEffect pet))
                    return pet;
                else
                    return ModContent.GetInstance<BearEffect>();
            }
        }
        public override string PetsTooltip => Compatibility.LocVal("PetTooltips.BearsEye")
                .Replace("<belowHealth>", Math.Round(bear.hpTreshold * 100, 2).ToString())
                .Replace("<baseShield>", Math.Round(bear.baseHpShield * 100, 2).ToString())
                .Replace("<bonusShield>", Math.Round(bear.bonusHpShield * 100, 2).ToString())
                .Replace("<shieldDuration>", Math.Round(bear.shieldDuration / 60f, 2).ToString())
                .Replace("<cooldown>", Math.Round(bear.cooldown / 60f, 2).ToString())
                .Replace("<rogueDmg>", Math.Round(bear.rogueDmg * 100, 2).ToString())
                .Replace("<stealthDmg>", Math.Round(bear.stealthDmg * 100, 2).ToString())
                .Replace("<stealthMoving>", Math.Round(bear.stealthMoving * 100, 2).ToString())
                .Replace("<stealthNotMoving>", Math.Round(bear.stealthNotMoving * 100, 2).ToString());
        public override string SimpleTooltip => Compatibility.LocVal("SimpleTooltips.BearsEye");
    }
}
