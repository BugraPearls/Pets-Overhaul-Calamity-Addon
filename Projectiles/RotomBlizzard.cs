using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using PetsOverhaul.NPCs;
using PetsOverhaulCalamityAddon.CalamityPets;
using PetsOverhaulCalamityAddon.Systems;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.Projectiles
{
    public class RotomBlizzard : ModProjectile
    {
        public ElectricTroublemakerEffect Rotom => Main.player[Projectile.owner].GetModPlayer<ElectricTroublemakerEffect>();
        public override void SetDefaults()
        {
            Projectile.timeLeft = 5;
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.penetrate = -1;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
            Projectile.friendly = true;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.Resize((int)Projectile.ai[0], (int)Projectile.ai[0]);
            Projectile.timeLeft = (int)Projectile.ai[1];
        }
        public override void AI()
        {
            foreach (var npc in Main.ActiveNPCs)
            {
                if (Projectile.getRect().Intersects(npc.getRect()))
                {
                    NpcPet.AddSlow(new NpcPet.PetSlow(Rotom.coldSlow * Rotom.GetTypeEffectiveness(npc, ElectricTroublemakerEffect.blizzard), 1, CalSlows.rotomBlizzard), npc);
                    if (npc.TryGetGlobalNPC(out RotomBlizzardFreeze blizzard))
                    {
                        blizzard.cooldownToResetFreeze = 60;
                        blizzard.FreezeVal++;
                        if (blizzard.FreezeVal >= Rotom.freezeRequirement)
                        {
                            npc.AddBuff(ModContent.BuffType<GlacialState>(), Rotom.freezeDuration);
                        }
                    }
                }
            }
            for (int i = 0; i < 3; i++)
            {
                Dust.NewDustPerfect(new Vector2(Main.rand.NextFloat(Projectile.width) + Projectile.position.X, Projectile.position.Y - Main.rand.NextFloat(5f, 6f)), DustID.Snow, Vector2.Zero).noGravity = true;
            }
            if (Main.rand.NextBool(2))
            {
                Dust.NewDustPerfect(new Vector2(Main.rand.NextFloat(Projectile.width) + Projectile.position.X, Projectile.position.Y - 5), DustID.SnowflakeIce, new Vector2(0.4f, 11.5f)).noGravity = true;
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage *= Rotom.GetTypeEffectiveness(target, ElectricTroublemakerEffect.blizzard);
        }
    }
}
