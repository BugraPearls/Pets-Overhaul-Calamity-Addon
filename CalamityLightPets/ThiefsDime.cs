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
    public sealed class ThiefsDimeEffect : LightPetEffect
    {
        public override void PostUpdateMiscEffects()
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out ThiefsDime dime))
            {
                Player.Calamity().rogueVelocity += dime.RogueVelocity.CurrentStatFloat;
                Player.Calamity().accStealthGenBoost += dime.StealthGain.CurrentStatFloat;
                Player.GetDamage<RogueDamageClass>() += dime.RogueDamage.CurrentStatFloat;
            }
        }
        public override void ModifyLuck(ref float luck)
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out ThiefsDime dime))
            {
                luck += dime.Luck.CurrentStatFloat;
            }
        }
    }
    public sealed class ThiefsDime : GlobalItem
    {
        public LightPetStat Luck = new(8, 1);
        public LightPetStat RogueDamage = new(25, 1);
        public LightPetStat RogueVelocity = new(25, 1);
        public LightPetStat StealthGain = new(30, 0.005f, 0.06f);
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == CalamityLightPetIDs.Goldie;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            Luck.SetRoll();
            RogueDamage.SetRoll();
            RogueVelocity.SetRoll();
            StealthGain.SetRoll();
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)Luck.CurrentRoll);
            writer.Write((byte)RogueDamage.CurrentRoll);
            writer.Write((byte)RogueVelocity.CurrentRoll);
            writer.Write((byte)StealthGain.CurrentRoll);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            Luck.CurrentRoll = reader.ReadByte();
            RogueDamage.CurrentRoll = reader.ReadByte();
            RogueVelocity.CurrentRoll = reader.ReadByte();
            StealthGain.CurrentRoll = reader.ReadByte();
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("Stat1", Luck.CurrentRoll);
            tag.Add("Stat2", RogueDamage.CurrentRoll);
            tag.Add("Stat3", RogueVelocity.CurrentRoll);
            tag.Add("Stat4", StealthGain.CurrentRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.TryGet("Stat1", out int luck))
            {
                Luck.CurrentRoll = luck;
            }

            if (tag.TryGet("Stat2", out int damage))
            {
                RogueDamage.CurrentRoll = damage;
            }

            if (tag.TryGet("Stat3", out int velocity))
            {
                RogueVelocity.CurrentRoll = velocity;
            }

            if (tag.TryGet("Stat4", out int stealth))
            {
                StealthGain.CurrentRoll = stealth;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.LightPetTooltips.RustedJingleBell")

                        .Replace("<luck>", Luck.BaseAndPerQuality())
                        .Replace("<rogue>", RogueDamage.BaseAndPerQuality())
                        .Replace("<velocity>", RogueVelocity.StatSummaryLine())
                        .Replace("<stealth>", StealthGain.BaseAndPerQuality())

                        .Replace("<luckLine>", Luck.StatSummaryLine())
                        .Replace("<rogueLine>", RogueDamage.StatSummaryLine())
                        .Replace("<velocityLine>", RogueVelocity.StatSummaryLine())
                        .Replace("<stealthLine>", StealthGain.StatSummaryLine())
                        ));
            if (RogueDamage.CurrentRoll <= 0)
            {
                tooltips.Add(new(Mod, "Tooltip0", PetTextsColors.RollMissingText()));
            }
        }
    }
}