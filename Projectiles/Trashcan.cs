using CalamityMod;
using Microsoft.Xna.Framework;
using PetsOverhaul.Config;
using PetsOverhaul.NPCs;
using PetsOverhaulCalamityAddon.Buffs;
using PetsOverhaulCalamityAddon.CalamityPets;
using PetsOverhaulCalamityAddon.Systems;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.Projectiles
{
    public class Trashcan : ModProjectile
    {
        public DannyDevitoEffect Trashman => Main.player[Projectile.owner].GetModPlayer<DannyDevitoEffect>();
        public static float SickWeakness(NPC npc)
        {
            float baseVal = 1f;
            if (npc.Calamity().VulnerableToSickness.HasValue)
            {
                if (npc.Calamity().VulnerableToSickness.Value)
                    baseVal *= 2f;
                else
                    baseVal *= 0.5f;
            }
            return baseVal;
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.ThrowingKnife);
            Projectile.aiStyle = ProjAIStyleID.ThrownProjectile;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage *= SickWeakness(target);
        }
        public override void OnKill(int timeLeft)
        {
            foreach (NPC npc in Main.ActiveNPCs)
            {
                if (Projectile.Distance(npc.Center) < Trashman.radius)
                {
                    NpcPet.AddSlow(new NpcPet.PetSlow(Trashman.slow * SickWeakness(npc), Trashman.slowDuration, CalSlows.trashmanSignatureMove), npc);

                    if (Main.rand.NextBool(Math.Min((int)(Trashman.confusionChance * SickWeakness(npc)), 100), 100))
                    {
                        npc.AddBuff(BuffID.Confused, Trashman.confusionDuration);
                    }
                }
            }
            for (int i = 0; i < 60; i++)
            {
                Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(Trashman.radius, Trashman.radius), DustID.Poop, new Vector2(Main.rand.NextFloat(-0.2f, 0.2f), 2)).noGravity = true;
            }
            if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
            {
                SoundEngine.PlaySound(SoundID.Item178 with { PitchVariance = 1f }, Projectile.Center);
                SoundEngine.PlaySound(SoundID.Item37 with { PitchVariance = 0.4f, Pitch = -0.8f }, Projectile.Center);
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.active)
                target.AddBuff(ModContent.BuffType<BleedOut>(), Trashman.bleedDuration);
        }
    }
}
