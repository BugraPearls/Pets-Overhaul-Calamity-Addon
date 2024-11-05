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
    public sealed class RomajedaOrchidTooltip : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == CalamityPetIDs.Kendra;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }

            KendraEffect kendra = Main.LocalPlayer.GetModPlayer<KendraEffect>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.RomajedaOrchid")
                .Replace("<class>", PetTextsColors.ClassText(kendra.PetClassPrimary, kendra.PetClassSecondary))
                .Replace("<percAbsorb>", Math.Round(kendra.absorbPercent * 100, 2).ToString())
                .Replace("<stealthMult>", kendra.stealthMult.ToString())
                .Replace("<storedDmg>", kendra.currentNextDamage.ToString())
            ));
        }
    }
}
