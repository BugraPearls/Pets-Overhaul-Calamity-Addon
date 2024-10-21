using PetsOverhaulCalamityAddon.Systems;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Buffs.StatDebuffs;
using Terraria.ID;
using Terraria.Audio;
using CalamityMod.Projectiles.Enemy;
using CalamityMod.NPCs.AcidRain;
using System;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class FlakHermitEffect : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.None;
        public float standingStealth = 0.4f;
        public float maxStealth = 0.1f;
        public float stealthDmg = 0.08f;
        public int acidBaseDmg = 30;
        public int irradiateDuration = 240;
        public int cooldown = 300;
        public int radius = 140;
        private bool nextHitIsExplosive = false;
        public void AcidExplosion(Vector2 center)
        {
            foreach (NPC npc in Main.ActiveNPCs)
            {
                if (npc.Distance(center) < radius)
                {
                    npc.SimpleStrikeNPC(acidBaseDmg, Player.direction, damageType: new RogueDamageClass(), damageVariation: true, luck: Player.luck);
                    npc.AddBuff(ModContent.BuffType<Irradiated>(), irradiateDuration);
                }
            }
            if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                SoundEngine.PlaySound(SoundID.Item96 with { PitchVariance = 0.6f }, center);
            for (int i = 0; i < 35; i++)
            {
                Dust.NewDustPerfect(center + Main.rand.NextVector2Circular(radius, radius), DustID.CursedTorch,Scale: 2f);
            }
        }
        public override void PreUpdate()
        {
            if (Pet.PetInUse(CalamityPetIDs.FlakHermit))
            {
                Pet.SetPetAbilityTimer(cooldown);
            }
        }
        public override void PostUpdate()
        {
            if (Pet.PetInUseWithSwapCd(CalamityPetIDs.FlakHermit) && Player.Calamity().stealthStrikeThisFrame && Pet.timer <= 0)
            {   
                AcidExplosion(Player.Center);
                Pet.timer = Pet.timerMax;
                nextHitIsExplosive = true;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUseWithSwapCd(CalamityPetIDs.FlakHermit) && hit.DamageType is RogueDamageClass && nextHitIsExplosive)
            {
                AcidExplosion(target.Center);
                nextHitIsExplosive = false;
            }
        }
        public override void PostUpdateEquips() //seems like Calamity runs rogueStealthMax checks at MiscEffects, which causes it to not work properly
        {
            if (Pet.PetInUseWithSwapCd(CalamityPetIDs.FlakHermit))
            {
                Player.Calamity().stealthGenStandstill += standingStealth;
                Player.Calamity().stealthDamage += stealthDmg;
                Player.Calamity().rogueStealthMax += maxStealth;
            }
        }
    }
    public sealed class GeyserShellTooltip : GlobalItem
    {

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == CalamityPetIDs.FlakHermit;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }

            FlakHermitEffect hermit = Main.LocalPlayer.GetModPlayer<FlakHermitEffect>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.GeyserShell")
                .Replace("<class>", PetTextsColors.ClassText(hermit.PetClassPrimary, hermit.PetClassSecondary))
                .Replace("<stealth>", Math.Round(hermit.standingStealth * 100, 2).ToString())
                .Replace("<maxStealth>", Math.Round(hermit.maxStealth * 100, 2).ToString())
                .Replace("<stealthDmg>", Math.Round(hermit.stealthDmg * 100, 2).ToString())
                .Replace("<damage>", hermit.acidBaseDmg.ToString())
                .Replace("<irradiateDuration>", Math.Round(hermit.irradiateDuration / 60f, 2).ToString())
                .Replace("<cooldown>", Math.Round(hermit.cooldown / 60f, 2).ToString())
            ));
        }
    }
}
