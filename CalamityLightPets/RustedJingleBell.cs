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
            if (Player.miscEquips[1].TryGetGlobalItem(out RustedJingleBellPet bell))
            {
                Player.breathMax += bell.Breathe.CurrentStatInt / 7; //In vanilla how long Player can breathe by default is breathMax * 7 due to it ticking down every 7 frame.
                Pet.abilityHaste += bell.Haste.CurrentStatFloat;
                if (Collision.WetCollision(Player.position, Player.width, Player.height))
                {
                    Pet.miningFortune += bell.MiningFortuneInWater.CurrentStatInt;
                }
            }
        }
    }
    public sealed class RustedJingleBellPet : GlobalItem
    {
        public LightPetStat Breathe = new(30, 14, 90);
        public LightPetStat Haste = new(25, 0.002f, 0.03f);
        public LightPetStat MiningFortuneInWater = new(10, 2, 5);
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == CalamityLightPetIDs.BabyGhostBell;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            Breathe.SetRoll();
            Haste.SetRoll();
            MiningFortuneInWater.SetRoll();
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)Breathe.CurrentRoll);
            writer.Write((byte)Haste.CurrentRoll);
            writer.Write((byte)MiningFortuneInWater.CurrentRoll);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            Breathe.CurrentRoll = reader.ReadByte();
            Haste.CurrentRoll = reader.ReadByte();
            MiningFortuneInWater.CurrentRoll = reader.ReadByte();
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("Stat1", Breathe.CurrentRoll);
            tag.Add("Stat2", Haste.CurrentRoll);
            tag.Add("Stat3", MiningFortuneInWater.CurrentRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.TryGet("Stat1", out int breathe))
            {
                Breathe.CurrentRoll = breathe;
            }

            if (tag.TryGet("Stat2", out int haste))
            {
                Haste.CurrentRoll = haste;
            }

            if (tag.TryGet("Stat3", out int fortune))
            {
                MiningFortuneInWater.CurrentRoll = fortune;
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
                        .Replace("<fortune>", MiningFortuneInWater.BaseAndPerQuality())

                        .Replace("<breatheLine>", Breathe.StatSummaryLine(Math.Round(Breathe.CurrentStatInt / 60f, 2).ToString()))
                        .Replace("<hasteLine>", Haste.StatSummaryLine())
                        .Replace("<fortuneLine>", MiningFortuneInWater.StatSummaryLine())
                        ));
            if (Haste.CurrentRoll <= 0)
            {
                tooltips.Add(new(Mod, "Tooltip0", PetTextsColors.RollMissingText()));
            }
        }
    }
}