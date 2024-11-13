using CalamityMod;
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
    public sealed class KendraEffect : PetEffect
    {
        public override int PetItemID => CalamityPetIDs.Kendra;
        public override PetClasses PetClassPrimary => PetClasses.Rogue;
        public float absorbPercent = 1.70f;
        public float stealthMult = 1.3f;
        public int currentNextDamage = 0;
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (currentNextDamage > 0 && Pet.PetInUseWithSwapCd(CalamityPetIDs.Kendra) && GlobalPet.LifestealCheck(target) && modifiers.DamageType is RogueDamageClass)
            {
                modifiers.FlatBonusDamage += currentNextDamage * (proj.Calamity().stealthStrike ? stealthMult : 1f);
                currentNextDamage = 0;
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
            if (player.TryGetModPlayer(out KendraEffect kendra) && kendra.Pet.PetInUseWithSwapCd(CalamityPetIDs.Kendra))
            {
                if ((npc.damage * kendra.absorbPercent) > kendra.currentNextDamage)
                    kendra.currentNextDamage = (int)(npc.damage * kendra.absorbPercent);
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
        public override string PetsTooltip => Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.RomajedaOrchid")
                .Replace("<percAbsorb>", Math.Round(kendra.absorbPercent * 100, 2).ToString())
                .Replace("<stealthMult>", kendra.stealthMult.ToString())
                .Replace("<storedDmg>", kendra.currentNextDamage.ToString());
    }
}
