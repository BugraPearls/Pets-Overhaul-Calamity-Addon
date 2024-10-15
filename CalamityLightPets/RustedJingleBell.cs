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
    public sealed class RustedJingleBellEffect : LightPetEffect
    {
        public int vanillaBreatheReset = 200; //vanilla doesn't reset the breathe max, so we do it ourselves. 
        public override void ResetEffects()
        {
            Player.breathMax = vanillaBreatheReset;
        }
        public override void PostUpdateMiscEffects()
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out RustedJingleBell bell))
            {
                Player.breathMax += bell.Breathe.CurrentStatInt / 7; //In vanilla how long Player can breathe by default is breathMax * 7 due to it ticking down every 7 frame.
                Pet.abilityHaste += bell.Haste.CurrentStatFloat;
            }
        }
    }
    public sealed class RustedJingleBell : GlobalItem
    {
        public LightPetStat Breathe = new(30, 14, 90);
        public LightPetStat Haste = new(40, 0.002f, 0.03f);
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == CalamityLightPetIDs.BabyGhostBell;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            Breathe.SetRoll();
            Haste.SetRoll();
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)Breathe.CurrentRoll);
            writer.Write((byte)Haste.CurrentRoll);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            Breathe.CurrentRoll = reader.ReadByte();
            Haste.CurrentRoll = reader.ReadByte();
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("BellBreathe", Breathe.CurrentRoll);
            tag.Add("BellHaste", Haste.CurrentRoll); 
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.TryGet("BellBreathe", out int breathe))
            {
                Breathe.CurrentRoll = breathe;
            }

            if (tag.TryGet("BellHaste", out int haste))
            {
                Haste.CurrentRoll = haste;
            }

        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.LightPetTooltips.RustedJingleBell")

                        .Replace("<breathe>", Breathe.BaseAndPerQuality(Math.Round(Breathe.StatPerRoll / 60f, 2).ToString(), Math.Round(Breathe.BaseStat / 60f, 2).ToString()))
                        .Replace("<haste>", Haste.BaseAndPerQuality())

                        .Replace("<breatheLine>", Breathe.StatSummaryLine(Math.Round(Breathe.CurrentStatInt / 60f, 2).ToString()))
                        .Replace("<hasteLine>", Haste.StatSummaryLine())
                        ));
            if (Haste.CurrentRoll <= 0)
            {
                tooltips.Add(new(Mod, "Tooltip0", PetTextsColors.RollMissingText()));
            }
        }
    }
}