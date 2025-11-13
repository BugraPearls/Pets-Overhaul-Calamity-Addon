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
                        Projectile proj = Projectile.NewProjectileDirect(PetUtils.GetSource_Pet(EntitySourcePetIDs.PetProjectile), Player.Center, Main.rand.NextVector2CircularEdge(4f, 4f), ModContent.ProjectileType<BrimstoneFireballMinion>(), Pet.PetDamage(dmg, DamageClass.Generic), 0f, Player.whoAmI);
                        proj.tileCollide = false;
                        proj.DamageType = DamageClass.Generic;
                        proj.CritChance = (int)Player.GetCritChance(DamageClass.Generic);
                        proj.netUpdate = true;
                    }
                    Pet.timer = Pet.timerMax;
                }

                if (info.DamageSource.TryGetCausingEntity(out Entity entity))
                {
                    int damageTaken = Math.Min(info.SourceDamage, Player.statLife);
                    damageTaken = Main.DamageVar(Pet.PetDamage(damageTaken * reflectAmount, DamageClass.Generic), Player.luck); //Caps the Reflect's base damage to Player's current HP.
                    if (entity is Projectile projectile && projectile.TryGetGlobalProjectile(out PetGlobalProjectile proj) && Main.npc[proj.sourceNpcId].active && Main.npc[proj.sourceNpcId].dontTakeDamage == false)
                    {
                        Main.npc[proj.sourceNpcId].SimpleStrikeNPC(damageTaken, info.HitDirection, Main.rand.NextBool((int)Math.Min(Player.GetTotalCritChance<GenericDamageClass>(), 100), 100), kbFromReflect, DamageClass.Generic);
                    }
                    else if (entity is NPC npc && npc.active == true && npc.dontTakeDamage == false)
                    {
                        npc.SimpleStrikeNPC(damageTaken, info.HitDirection, Main.rand.NextBool((int)Math.Min(Player.GetTotalCritChance<GenericDamageClass>(), 100), 100), kbFromReflect, DamageClass.Generic);
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
