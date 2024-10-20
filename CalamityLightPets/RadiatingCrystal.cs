using CalamityMod.CalPlayer;
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
    public sealed class RadiatingCrystalEffect : LightPetEffect
    {
        public override void PostUpdateMiscEffects()
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out RadiatingCrystalPet crystal))
            {
                Player.GetCritChance<GenericDamageClass>() += crystal.CritChance.CurrentStatFloat;
            }
        }
    }
    public sealed class RadiatingCrystalPet : GlobalItem
    {
        public LightPetStat PoisonTolerance = new(25, 14, 90);
        public LightPetStat CritChance = new(40, 0.15f, 2.5f);
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == CalamityLightPetIDs.Radiator;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            PoisonTolerance.SetRoll();
            CritChance.SetRoll();
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)PoisonTolerance.CurrentRoll);
            writer.Write((byte)CritChance.CurrentRoll);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            PoisonTolerance.CurrentRoll = reader.ReadByte();
            CritChance.CurrentRoll = reader.ReadByte();
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("RadiatorTolerance", PoisonTolerance.CurrentRoll);
            tag.Add("RadiatorCrit", CritChance.CurrentRoll); 
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.TryGet("RadiatorTolerance", out int poisonTolerance))
            {
                PoisonTolerance.CurrentRoll = poisonTolerance;
            }

            if (tag.TryGet("RadiatorCrit", out int crit))
            {
                CritChance.CurrentRoll = crit;
            }

        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.LightPetTooltips.RustedJingleBell")

                        .Replace("<tolerance>", PoisonTolerance.BaseAndPerQuality(Math.Round(PoisonTolerance.StatPerRoll / 60f, 2).ToString(), Math.Round(PoisonTolerance.BaseStat / 60f, 2).ToString()))
                        .Replace("<crit>", CritChance.BaseAndPerQuality())

                        .Replace("<toleranceLine>", PoisonTolerance.StatSummaryLine(Math.Round(PoisonTolerance.CurrentStatInt / 60f, 2).ToString()))
                        .Replace("<critLine>", CritChance.StatSummaryLine())
                        ));
            if (CritChance.CurrentRoll <= 0)
            {
                tooltips.Add(new(Mod, "Tooltip0", PetTextsColors.RollMissingText()));
            }
        }
    }
}