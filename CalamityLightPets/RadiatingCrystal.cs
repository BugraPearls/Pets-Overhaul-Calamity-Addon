using CalamityMod.CalPlayer;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.LightPets
{
    public sealed class RadiatingCrystalEffect : LightPetEffect
    {
        public override void PostUpdateMiscEffects()
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out RadiatingCrystalPet crystal))
            {
                Player.GetKnockback<GenericDamageClass>() += crystal.Knockback.CurrentStatFloat;
                for (int i = 0; i < Player.MaxBuffs; i++)
                {
                    if (Main.debuff[Player.buffType[i]] && BuffID.Sets.NurseCannotRemoveDebuff[Player.buffType[i]] == false)
                    {
                        Player.statDefense += crystal.DebuffedDefense.CurrentStatInt;
                    }
                }
                foreach (NPC npc in Main.ActiveNPCs)
                {
                    if (Player.Distance(npc.Center) < crystal.PoisonRadius.CurrentStatInt)
                    {
                        npc.AddBuff(BuffID.Poisoned, 60);
                    }
                }
            }
        }
    }
    public sealed class RadiatingCrystalPet : GlobalItem
    {
        public LightPetStat Knockback = new(25, 0.01f, 0.05f);
        public LightPetStat DebuffedDefense = new(7, 1);
        public LightPetStat PoisonRadius = new(15, 12, 60);
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == CalamityLightPetIDs.Radiator;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            Knockback.SetRoll();
            DebuffedDefense.SetRoll();
            PoisonRadius.SetRoll();
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)Knockback.CurrentRoll);
            writer.Write((byte)DebuffedDefense.CurrentRoll);
            writer.Write((byte)PoisonRadius.CurrentRoll);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            Knockback.CurrentRoll = reader.ReadByte();
            DebuffedDefense.CurrentRoll = reader.ReadByte();
            PoisonRadius.CurrentRoll = reader.ReadByte();
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("Stat1", Knockback.CurrentRoll);
            tag.Add("Stat2", DebuffedDefense.CurrentRoll);
            tag.Add("Stat3", PoisonRadius.CurrentRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.TryGet("Stat1", out int poisonTolerance))
            {
                Knockback.CurrentRoll = poisonTolerance;
            }

            if (tag.TryGet("Stat2", out int def))
            {
                DebuffedDefense.CurrentRoll = def;
            }

            if (tag.TryGet("Stat3", out int radius))
            {
                PoisonRadius.CurrentRoll = radius;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.LightPetTooltips.RadiatingCrystal")

                        .Replace("<knockback>", Knockback.BaseAndPerQuality())
                        .Replace("<def>", DebuffedDefense.BaseAndPerQuality())
                        .Replace("<radius>", PoisonRadius.BaseAndPerQuality(Math.Round(PoisonRadius.StatPerRoll / 16f, 2).ToString()))

                        .Replace("<knockbackLine>", Knockback.StatSummaryLine())
                        .Replace("<defLine>", DebuffedDefense.StatSummaryLine())
                        .Replace("<radiusLine>", PoisonRadius.StatSummaryLine(Math.Round(PoisonRadius.CurrentStatInt / 16f, 2).ToString()))
                        ));
            if (DebuffedDefense.CurrentRoll <= 0)
            {
                tooltips.Add(new(Mod, "Tooltip0", PetTextsColors.RollMissingText()));
            }
        }
    }
}