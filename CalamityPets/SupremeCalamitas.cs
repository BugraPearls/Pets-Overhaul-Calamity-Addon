﻿using PetsOverhaulCalamityAddon.Systems;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using CalamityMod;
using Terraria.ID;
using PetsOverhaul.Items;
using PetsOverhaul.Projectiles;
using System;
using PetsOverhaul.Buffs;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class SupremeCalamitasEffect : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public float critDmgReduce = 0.5f;
        public float critChanceToDmg = 1.1f;
        public float magicExtraCost = 0.9f;
        public static bool ItemIsATool(Item item)
        {
            if (item == null || item.IsAir)
            {
                return false;
            }

            return item.axe > 0 || item.hammer > 0 || item.pick > 0 || item.fishingPole > 0;
        }
        public void SetCritDmgModifs(ref NPC.HitModifiers modifiers)
        {
            modifiers.SetCrit();
            modifiers.CritDamage -= critDmgReduce;
            modifiers.CritDamage += Player.GetTotalCritChance(modifiers.DamageType) * critChanceToDmg * 0.01f;
        }
        public override void PreUpdate()
        {
            if (Main.mouseItem.TryGetGlobalItem(out SupremeCalamitasManaCost scal)) //Done because Main.mouseItem does not count in UpdateInventory()
            {
                scal.SetManaCost(Main.mouseItem, Player);
            }
        }
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (Player.HasBuff(BuffID.ManaSickness) && Pet.PetInUseWithSwapCd(CalamityPetIDs.SupremeCalamitas) && ItemIsATool(item) == false && item.DamageType is not SummonDamageClass or MagicSummonHybridDamageClass or MagicDamageClass)
            {
                damage *= 1f - Player.manaSickReduction;
            }
        }
        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Pet.PetInUseWithSwapCd(CalamityPetIDs.SupremeCalamitas) && ItemIsATool(item) == false && modifiers.DamageType is not SummonDamageClass && item.channel == false)
            {
                SetCritDmgModifs(ref modifiers);
            }
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Pet.PetInUseWithSwapCd(CalamityPetIDs.SupremeCalamitas) && modifiers.DamageType is not SummonDamageClass && proj.TryGetGlobalProjectile(out ProjectileSourceChecks source) && source.itemProjIsFrom.channel == false && ItemIsATool(source.itemProjIsFrom) == false)
            {
                SetCritDmgModifs(ref modifiers);
            }
        }
    }
    public sealed class SupremeCalamitasManaCost : GlobalItem
    {
        public int originalManaCost = -1;
        public override bool InstancePerEntity => true;
        public void SetManaCost(Item item, Player player)
        {
            if (originalManaCost == -1)
                originalManaCost = item.mana;
            if (player.TryGetModPlayer(out SupremeCalamitasEffect sCal) && sCal.Pet.PetInUseWithSwapCd(CalamityPetIDs.SupremeCalamitas) && item.damage > 0 && SupremeCalamitasEffect.ItemIsATool(item) == false && item.channel == false)
            {
                if (item.DamageType is SummonDamageClass or MagicDamageClass or MagicSummonHybridDamageClass && item.mana > 0 && item.channel)
                {
                    return;
                }
                if (item.mana <= 0)
                {
                    item.mana = item.useTime * 2;
                }
            }
            else
            {
                item.mana = originalManaCost;
            }
        }
        public override void UpdateInventory(Item item, Player player)
        {
            SetManaCost(item, player);
        }
        public override void ModifyManaCost(Item item, Player player, ref float reduce, ref float mult)
        {
            if (player.TryGetModPlayer(out SupremeCalamitasEffect sCal) && sCal.Pet.PetInUseWithSwapCd(CalamityPetIDs.SupremeCalamitas) && SupremeCalamitasEffect.ItemIsATool(item) == false && item.DamageType is MagicDamageClass or MagicSummonHybridDamageClass && item.mana > 0)
            {
                mult += sCal.magicExtraCost;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            int index = tooltips.FindIndex(x => x.Name == "PrefixUseMana");
            if (index >= 0 && tooltips[index].Text.Contains('∞')) //This is bit of a bandaid fix, items that doesn't naturally have a mana value ends up having "Mana cost +∞" with red text if they have a prefix, no matter if prefix has no effects on mana cost.
            {
                tooltips[index].Hide();
            }
        }
    }
    public sealed class SupremeCalamitasManaSicknessTooltip : GlobalBuff
    {
        public override void ModifyBuffText(int type, ref string buffName, ref string tip, ref int rare)
        {
            if (Main.LocalPlayer.miscEquips[0].type == CalamityPetIDs.SupremeCalamitas && type == BuffID.ManaSickness && Main.LocalPlayer.HasBuff(ModContent.BuffType<ObliviousPet>()) == false)
            {
                tip += "\n" + Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.ScalManaSicknessExtraText");
            }
        }
    }
    public sealed class BrimstoneJewelTooltip : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == CalamityPetIDs.SupremeCalamitas;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }
            SupremeCalamitasEffect calamitas = Main.LocalPlayer.GetModPlayer<SupremeCalamitasEffect>();

            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.BrimstoneJewel")
                .Replace("<class>", PetTextsColors.ClassText(calamitas.PetClassPrimary, calamitas.PetClassSecondary))
                .Replace("<critDmgReduce>", Math.Round(calamitas.critDmgReduce * 100, 2).ToString())
                .Replace("<chanceToDmg>", Math.Round(calamitas.critChanceToDmg * 100, 2).ToString())
                .Replace("<magicCost>", Math.Round(calamitas.magicExtraCost * 100,2).ToString())
            ));
        }
    }
}
