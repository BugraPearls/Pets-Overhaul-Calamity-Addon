using CalamityMod;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.LightPets
{
    public sealed class LittleLightEffect : LightPetEffect
    {
        public override void PostUpdateMiscEffects()
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out LittleLightPet light))
            {
                Player.aggro += light.Aggro.CurrentStatInt;
                Pet.petShieldMultiplier += light.ShieldIncrease.CurrentStatFloat;
                Player.statLifeMax2 += light.Health.CurrentStatInt;
            }
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out LittleLightPet light))
            {
                modifiers.Knockback *= 1f - light.KnockbackResist.CurrentStatFloat;
            }
        }
    }
    public sealed class LittleLightPet : GlobalItem
    {
        public LightPetStat Aggro = new(20, 10, 50);
        public LightPetStat KnockbackResist = new(20, 0.02f, 0.2f);
        public LightPetStat ShieldIncrease = new(20, 0.008f, 0.06f);
        public LightPetStat Health = new(20, 2, 15);
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == CalamityLightPetIDs.LittleLight;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            Aggro.SetRoll();
            KnockbackResist.SetRoll();
            ShieldIncrease.SetRoll();
            Health.SetRoll();
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)Aggro.CurrentRoll);
            writer.Write((byte)KnockbackResist.CurrentRoll);
            writer.Write((byte)ShieldIncrease.CurrentRoll);
            writer.Write((byte)Health.CurrentRoll);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            Aggro.CurrentRoll = reader.ReadByte();
            KnockbackResist.CurrentRoll = reader.ReadByte();
            ShieldIncrease.CurrentRoll = reader.ReadByte();
            Health.CurrentRoll = reader.ReadByte();
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("Stat1", Aggro.CurrentRoll);
            tag.Add("Stat2", KnockbackResist.CurrentRoll);
            tag.Add("Stat3", ShieldIncrease.CurrentRoll);
            tag.Add("Stat4", Health.CurrentRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.TryGet("Stat1", out int aggro))
            {
                Aggro.CurrentRoll = aggro;
            }

            if (tag.TryGet("Stat2", out int kb))
            {
                KnockbackResist.CurrentRoll = kb;
            }

            if (tag.TryGet("Stat3", out int shield))
            {
                ShieldIncrease.CurrentRoll = shield;
            }

            if (tag.TryGet("Stat4", out int health))
            {
                Health.CurrentRoll = health;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.LightPetTooltips.LittleLight")

                        .Replace("<aggro>", Aggro.BaseAndPerQuality())
                        .Replace("<kbResist>", KnockbackResist.BaseAndPerQuality())
                        .Replace("<shield>", ShieldIncrease.BaseAndPerQuality())
                        .Replace("<health>", Health.BaseAndPerQuality())

                        .Replace("<aggroLine>", Aggro.StatSummaryLine())
                        .Replace("<kbResistLine>", KnockbackResist.StatSummaryLine())
                        .Replace("<shieldLine>", ShieldIncrease.StatSummaryLine())
                        .Replace("<healthLine>", Health.StatSummaryLine())
                        ));
            if (KnockbackResist.CurrentRoll <= 0)
            {
                tooltips.Add(new(Mod, "Tooltip0", PetTextsColors.RollMissingText()));
            }
        }
    }
}