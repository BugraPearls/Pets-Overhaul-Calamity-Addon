using CalamityMod;
using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System;
using Terraria;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class KendraEffect : PetEffect
    {
        public override int PetItemID => CalamityPetIDs.Kendra;
        public override PetClasses PetClassPrimary => PetClasses.Rogue;
        public float absorbPercent = 1.1f;
        public float stealthMult = 1.3f;
        public int currentNextDamage = 0;
        public override int PetStackCurrent => currentNextDamage;
        public override int PetStackMax => 0;
        public override string PetStackText => Compatibility.LocVal("PetTooltips.RomajedaOrchidStack");
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (currentNextDamage > 0 && PetIsEquipped() && PetUtils.LifestealCheck(target) && modifiers.DamageType is RogueDamageClass)
            {
                modifiers.FlatBonusDamage += currentNextDamage * (proj.Calamity().stealthStrike ? stealthMult : 1f);
                currentNextDamage = 0;
            }
        }
        public override void Load()
        {
            PetModPlayer.OnEnemyDeath += OnKillEffect;
        }
        public override void Unload()
        {
            PetModPlayer.OnEnemyDeath -= OnKillEffect;
        }
        public static void OnKillEffect(NPC npc, Player player)
        {
            if (player.TryGetModPlayer(out KendraEffect kendra) && kendra.PetIsEquipped())
            {
                if ((npc.defDamage * kendra.absorbPercent) > kendra.currentNextDamage) //Using defDamage instead of damage, because damage can get changed while defDamage is stored.
                {
                    kendra.currentNextDamage = (int)(npc.defDamage * kendra.absorbPercent);
                }
            }
        }
    }
    public sealed class RomajedaOrchidTooltip : PetTooltip
    {
        public override PetEffect PetsEffect => kendra;
        public static KendraEffect kendra
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out KendraEffect pet))
                    return pet;
                else
                    return ModContent.GetInstance<KendraEffect>();
            }
        }
        public override string PetsTooltip => Compatibility.LocVal("PetTooltips.RomajedaOrchid")
                .Replace("<percAbsorb>", Math.Round(kendra.absorbPercent * 100, 2).ToString())
                .Replace("<stealthMult>", kendra.stealthMult.ToString());
        public override string SimpleTooltip => Compatibility.LocVal("SimpleTooltips.RomajedaOrchid");
    }
}
