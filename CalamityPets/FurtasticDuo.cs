using CalamityMod;
using Microsoft.Xna.Framework;
using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class FurtasticDuoEffect : PetEffect
    {
        public override int PetItemID => CalamityPetIDs.FurtasticDuo;
        public override PetClasses PetClassPrimary => PetClasses.Rogue;
        public override PetClasses PetClassSecondary => PetClasses.Defensive;

        public float stealthMoving = 0.16f;
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
        public override int PetAbilityCooldown => cooldown;
        public override int PetStackCurrent => currentNextDamage;
        public override int PetStackMax => 0;
        public override string PetStackText => Compatibility.LocVal("PetTooltips.RomajedaOrchidStack");
        public override void ExtraPreUpdate()
        {
            lifeguardMultTimer--;
            if (lifeguardMultTimer < 0)
            {
                lifeguardMultTimer = 0;
            }
        }
        public override void PostUpdateEquips()
        {
            if (PetIsEquipped())
            {
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
                        lifeguardMultTimer = shieldDuration;
                    }
                };
            }
        }
        public override void Load()
        {
            GlobalPet.OnEnemyDeath += OnKillEffect;
        }
        public override void Unload()
        {
            GlobalPet.OnEnemyDeath -= OnKillEffect;
        }
        public static void OnKillEffect(NPC npc, Player player)
        {
            if (player.TryGetModPlayer(out FurtasticDuoEffect duo) && duo.PetIsEquipped())
            {
                if ((npc.defDamage * duo.absorbPercent) > duo.currentNextDamage)
                {
                    duo.currentNextDamage = (int)(npc.defDamage * duo.absorbPercent);
                }
            }
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (currentNextDamage > 0 && PetIsEquipped() && GlobalPet.LifestealCheck(target) && modifiers.DamageType is RogueDamageClass)
            {
                modifiers.FlatBonusDamage += currentNextDamage * (lifeguardMultTimer > 0 ? lifeguardMult : 1f);
                currentNextDamage = 0;
                Pet.AddShield(procShield, procShieldDuration);
            }
        }
    }
    public sealed class PrimroseKeepsakeTooltip : PetTooltip
    {
        public override PetEffect PetsEffect => duo;
        public static FurtasticDuoEffect duo
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out FurtasticDuoEffect pet))
                    return pet;
                else
                    return ModContent.GetInstance<FurtasticDuoEffect>();
            }
        }
        public override string PetsTooltip => Compatibility.LocVal("PetTooltips.PrimroseKeepsake")
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
                .Replace("<procShieldDuration>", Math.Round(duo.procShieldDuration / 60f, 2).ToString());
    }
}
