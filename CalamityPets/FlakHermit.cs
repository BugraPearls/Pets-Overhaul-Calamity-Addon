using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class FlakHermitEffect : PetEffect
    {
        public override int PetItemID => CalamityPetIDs.FlakHermit;
        public override PetClasses PetClassPrimary => PetClasses.Rogue;
        public float standingStealth = 0.25f;
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
                if (npc.Distance(center) < radius && npc.dontTakeDamage == false)
                {
                    npc.SimpleStrikeNPC(Pet.PetDamage(acidBaseDmg, DamageClass.Throwing), Player.direction, Main.rand.NextBool((int)Math.Min(Player.GetTotalCritChance<RogueDamageClass>(), 100), 100), 0, DamageClass.Throwing, true, Player.luck);
                    npc.AddBuff(ModContent.BuffType<Irradiated>(), irradiateDuration);
                }
            }
            if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                SoundEngine.PlaySound(SoundID.Item96 with { PitchVariance = 0.6f }, center);
            for (int i = 0; i < 25; i++)
            {
                Dust.NewDustPerfect(center + Main.rand.NextVector2Circular(radius, radius), DustID.CursedTorch, Scale: 2f);
            }
            GlobalPet.CircularDustEffect(Player.Center, DustID.CursedTorch, radius, 20, scale: 2f);
        }
        public override int PetAbilityCooldown => cooldown;
        public override void PostUpdate()
        {
            if (PetIsEquipped() && Player.Calamity().stealthStrikeThisFrame && Pet.timer <= 0)
            {
                AcidExplosion(Player.Center);
                Pet.timer = Pet.timerMax;
                nextHitIsExplosive = true;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PetIsEquipped() && hit.DamageType is RogueDamageClass && nextHitIsExplosive)
            {
                AcidExplosion(target.Center);
                nextHitIsExplosive = false;
            }
        }
        public override void PostUpdateEquips() //seems like Calamity runs rogueStealthMax checks at MiscEffects, which causes it to not work properly
        {
            if (PetIsEquipped())
            {
                Player.Calamity().stealthGenStandstill += standingStealth;
                Player.Calamity().bonusStealthDamage += stealthDmg;
                Player.Calamity().rogueStealthMax += maxStealth;
            }
        }
    }
    public sealed class GeyserShellTooltip : PetTooltip
    {
        public override PetEffect PetsEffect => hermit;
        public static FlakHermitEffect hermit
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out FlakHermitEffect pet))
                    return pet;
                else
                    return ModContent.GetInstance<FlakHermitEffect>();
            }
        }
        public override string PetsTooltip => Compatibility.LocVal("PetTooltips.GeyserShell")
                .Replace("<stealth>", Math.Round(hermit.standingStealth * 100, 2).ToString())
                .Replace("<maxStealth>", Math.Round(hermit.maxStealth * 100, 2).ToString())
                .Replace("<stealthDmg>", Math.Round(hermit.stealthDmg * 100, 2).ToString())
                .Replace("<damage>", hermit.acidBaseDmg.ToString())
                .Replace("<irradiateDuration>", Math.Round(hermit.irradiateDuration / 60f, 2).ToString())
                .Replace("<cooldown>", Math.Round(hermit.cooldown / 60f, 2).ToString());
        public override string SimpleTooltip => Compatibility.LocVal("SimpleTooltips.GeyserShell");
    }
}
