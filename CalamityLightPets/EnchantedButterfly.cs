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
        public override void PostUpdateEquips()
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out EnchantedButterflyPet butter))
            {
                Pet.petHealMultiplier += butter.PetHealPower.CurrentStatInt;
                Pet.globalFortune += butter.GlobalFortune.CurrentStatInt;
                Player.aggro += butter.Aggro.CurrentStatInt;
                Pet.petDirectDamageMultiplier += butter.PetDamage.CurrentStatFloat;
            }
        }
    }
    public sealed class EnchantedButterflyPet : LightPetItem
    {
        public LightPetStat PetHealPower = new(30, 0.006f, 0.07f);
        public LightPetStat GlobalFortune = new(16, 1, 6);
        public LightPetStat Aggro = new(20, -5, -40);
        public LightPetStat PetDamage = new(10, 0.01f, 0.05f);
        public override int LightPetItemID => CalamityLightPetIDs.Sparks;
        public override void UpdateInventory(Item item, Player player)
        {
            PetHealPower.SetRoll(player.luck);
            GlobalFortune.SetRoll(player.luck);
            Aggro.SetRoll(player.luck);
            PetDamage.SetRoll(player.luck);
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)PetHealPower.CurrentRoll);
            writer.Write((byte)GlobalFortune.CurrentRoll);
            writer.Write((byte)Aggro.CurrentRoll);
            writer.Write((byte)PetDamage.CurrentRoll);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            PetHealPower.CurrentRoll = reader.ReadByte();
            GlobalFortune.CurrentRoll = reader.ReadByte();
            Aggro.CurrentRoll = reader.ReadByte();
            PetDamage.CurrentRoll = reader.ReadByte();
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("Stat1", PetHealPower.CurrentRoll);
            tag.Add("Stat2", GlobalFortune.CurrentRoll);
            tag.Add("Stat3", Aggro.CurrentRoll);
            tag.Add("Stat4", PetDamage.CurrentRoll);
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
                PetDamage.CurrentRoll = mana;
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
                        .Replace("<damage>", PetDamage.BaseAndPerQuality())

                        .Replace("<healLine>", PetHealPower.StatSummaryLine())
                        .Replace("<fortuneLine>", GlobalFortune.StatSummaryLine())
                        .Replace("<aggroLine>", Aggro.StatSummaryLine(Aggro.CurrentStatInt.ToString())) //Using overload so the + doesn't appear on tooltip
                        .Replace("<damageLine>", PetDamage.StatSummaryLine())
                        ));
            if (GlobalFortune.CurrentRoll <= 0)
            {
                tooltips.Add(new(Mod, "Tooltip0", PetTextsColors.RollMissingText()));
            }
        }
    }
}