using CalamityMod.Projectiles.Summon;
using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System;
using Terraria;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class BrimlingEffect : PetEffect
    {
        public override int PetItemID => CalamityPetIDs.Brimling;
        public override PetClass PetClassPrimary => PetClassID.Defensive;
        public override PetClass PetClassSecondary => PetClassID.Offensive;
        public float drTreshold = 0.25f;
        public float dr = 0.15f;
        public int cooldown = 210;
        public float reflectAmount = 0.45f;
        public float kbFromReflect = 5f;
        public override int PetAbilityCooldown => cooldown;
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped() && Player.statLife < Player.statLifeMax2 * drTreshold)
            {
                Player.endurance += dr;
            }
        }
        public override void OnHurt(Player.HurtInfo info)
        {
            if (PetIsEquipped())
            {
                if (Pet.timer <= 0)
                {
                    float dmg = (info.SourceDamage / 2 + Player.statDefense) * (1f + Player.endurance);
                    for (int i = 0; i < 5; i++)
                    {
                        Projectile proj = Pet.NewPetSourcedProjectile(PetUtils.GetSource_Pet(EntitySourcePetIDs.PetProjectile), Player.Center, Main.rand.NextVector2CircularEdge(4f, 4f), ModContent.ProjectileType<BrimstoneFireballMinion>(), dmg, 0f, Player.whoAmI, damageClass: DamageClass.Generic);
                        proj.tileCollide = false;
                    }
                    Pet.timer = Pet.timerMax;
                }

                if (info.DamageSource.TryGetCausingEntity(out Entity entity))
                {
                    float damageTaken = Math.Min(info.SourceDamage, Player.statLife) * reflectAmount; //Caps the Reflect's base damage to Player's current HP.
                    if (entity is Projectile projectile && projectile.TryGetGlobalProjectile(out PetGlobalProjectile proj) && Main.npc[proj.sourceNpcId].active && Main.npc[proj.sourceNpcId].dontTakeDamage == false)
                    {
                        Pet.PetStrike(Main.npc[proj.sourceNpcId], damageTaken, info.HitDirection, Main.rand.NextBool((int)Math.Min(Player.GetTotalCritChance<GenericDamageClass>(), 100), 100), kbFromReflect, DamageClass.Generic);
                    }
                    else if (entity is NPC npc && npc.active == true && npc.dontTakeDamage == false)
                    {
                        Pet.PetStrike(npc, damageTaken, info.HitDirection, Main.rand.NextBool((int)Math.Min(Player.GetTotalCritChance<GenericDamageClass>(), 100), 100), kbFromReflect, DamageClass.Generic);

                    }
                }
            }
        }
    }
    public sealed class CharredRelicTooltip : PetTooltip
    {
        public override PetEffect PetsEffect => brimling;
        public static BrimlingEffect brimling
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out BrimlingEffect pet))
                    return pet;
                else
                    return ModContent.GetInstance<BrimlingEffect>();
            }
        }
        public override string PetsTooltip => Compatibility.LocVal("PetTooltips.CharredRelic")
                .Replace("<treshold>", Math.Round(brimling.drTreshold * 100, 2).ToString())
                .Replace("<dr>", Math.Round(brimling.dr * 100, 2).ToString())
                .Replace("<reflectAmount>", Math.Round(brimling.reflectAmount * 100, 2).ToString())
                .Replace("<cooldown>", Math.Round(brimling.cooldown / 60f, 2).ToString());
        public override string SimpleTooltip => Compatibility.LocVal("SimpleTooltips.CharredRelic");
    }
}
