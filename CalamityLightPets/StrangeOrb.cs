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
    public sealed class StrangeOrbEffect : LightPetEffect
    {
        public override void PostUpdateMiscEffects()
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out StrangeOrb orb))
            {
                Player.fishingSkill += orb.FishingPower.CurrentStatInt;
                Pet.fishingFortune += orb.FishingFortune.CurrentStatInt;
                if (Collision.WetCollision(Player.position,Player.width,Player.height))
                {
                    Pet.petHealMultiplier += orb.HealInWater.CurrentStatFloat;
                }
            }
        }
    }
    public sealed class StrangeOrb : GlobalItem
    {
        public LightPetStat FishingPower = new(8, 1);
        public LightPetStat FishingFortune = new(25, 1);
        public LightPetStat HealInWater = new(35, 0.005f, 0.07f);
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == CalamityLightPetIDs.OceanSpirit;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            FishingPower.SetRoll();
            FishingFortune.SetRoll();
            HealInWater.SetRoll();
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)FishingPower.CurrentRoll);
            writer.Write((byte)FishingFortune.CurrentRoll);
            writer.Write((byte)HealInWater.CurrentRoll);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            FishingPower.CurrentRoll = reader.ReadByte();
            FishingFortune.CurrentRoll = reader.ReadByte();
            HealInWater.CurrentRoll = reader.ReadByte();
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("Stat1", FishingPower.CurrentRoll);
            tag.Add("Stat2", FishingFortune.CurrentRoll);
            tag.Add("Stat3", HealInWater.CurrentRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.TryGet("Stat1", out int power))
            {
                FishingPower.CurrentRoll = power;
            }

            if (tag.TryGet("Stat2", out int fortune))
            {
                FishingFortune.CurrentRoll = fortune;
            }

            if (tag.TryGet("Stat3", out int heal))
            {
                HealInWater.CurrentRoll = heal;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.LightPetTooltips.RustedJingleBell")

                        .Replace("<power>", FishingPower.BaseAndPerQuality())
                        .Replace("<fortune>", FishingFortune.BaseAndPerQuality())
                        .Replace("<heal>", HealInWater.BaseAndPerQuality())

                        .Replace("<powerLine>", FishingPower.StatSummaryLine())
                        .Replace("<fortuneLine>", FishingFortune.StatSummaryLine())
                        .Replace("<healLine>", HealInWater.StatSummaryLine())
                        ));
            if (FishingFortune.CurrentRoll <= 0)
            {
                tooltips.Add(new(Mod, "Tooltip0", PetTextsColors.RollMissingText()));
            }
        }
    }
}