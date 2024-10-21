using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaul.LightPets
{
    public sealed class EnchantedButterflyEffect : LightPetEffect
    {
        public override void PostUpdateMiscEffects()
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out EnchantedButterflyPet butter))
            {
                Pet.petHealMultiplier += butter.PetHealPower.CurrentStatInt;
                Pet.globalFortune += butter.GlobalFortune.CurrentStatInt;
                Player.aggro += butter.Aggro.CurrentStatInt;
                Player.statManaMax2 += butter.Mana.CurrentStatInt;
            }
        }
    }
    public sealed class EnchantedButterflyPet : GlobalItem
    {
        public LightPetStat PetHealPower = new(30, 0.006f, 0.07f);
        public LightPetStat GlobalFortune = new(16, 1, 6);
        public LightPetStat Aggro = new(20, -5, -40);
        public LightPetStat Mana = new(10, 4, 25);
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == CalamityLightPetIDs.Sparks;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            PetHealPower.SetRoll();
            GlobalFortune.SetRoll();
            Aggro.SetRoll();
            Mana.SetRoll();
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)PetHealPower.CurrentRoll);
            writer.Write((byte)GlobalFortune.CurrentRoll);
            writer.Write((byte)Aggro.CurrentRoll);
            writer.Write((byte)Mana.CurrentRoll);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            PetHealPower.CurrentRoll = reader.ReadByte();
            GlobalFortune.CurrentRoll = reader.ReadByte();
            Aggro.CurrentRoll = reader.ReadByte();
            Mana.CurrentRoll = reader.ReadByte();
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("Stat1", PetHealPower.CurrentRoll);
            tag.Add("Stat2", GlobalFortune.CurrentRoll);
            tag.Add("Stat3", Aggro.CurrentRoll);
            tag.Add("Stat4", Mana.CurrentRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.TryGet("Stat1", out int heal))
            {
                PetHealPower.CurrentRoll = heal;
            }

            if (tag.TryGet("Stat2", out int fortune))
            {
                GlobalFortune.CurrentRoll = fortune;
            }

            if (tag.TryGet("Stat3", out int aggro))
            {
                Aggro.CurrentRoll = aggro;
            }

            if (tag.TryGet("Stat4", out int mana))
            {
                Mana.CurrentRoll = mana;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.LightPetTooltips.EnchantedButterfly")

                        .Replace("<heal>", PetHealPower.BaseAndPerQuality())
                        .Replace("<fortune>", GlobalFortune.BaseAndPerQuality())
                        .Replace("<aggro>", Aggro.BaseAndPerQuality())
                        .Replace("<mana>", Mana.BaseAndPerQuality())

                        .Replace("<healLine>", PetHealPower.StatSummaryLine())
                        .Replace("<fortuneLine>", GlobalFortune.StatSummaryLine())
                        .Replace("<aggroLine>", Aggro.StatSummaryLine(Aggro.CurrentStatInt.ToString())) //Using overload so the + doesn't appear on tooltip
                        .Replace("<manaLine>", Mana.StatSummaryLine())
                        ));
            if (GlobalFortune.CurrentRoll <= 0)
            {
                tooltips.Add(new(Mod, "Tooltip0", PetTextsColors.RollMissingText()));
            }
        }
    }
}