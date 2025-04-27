using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaulCalamityAddon.CalamityLightPets
{
    public sealed class RadiatingCrystalEffect : LightPetEffect
    {
        public override int LightPetItemID => CalamityLightPetIDs.Radiator;
        public override void PostUpdateEquips()
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out RadiatingCrystalPet crystal))
            {
                GlobalPet.CircularDustEffect(Player.Center, DustID.CursedTorch, crystal.PoisonRadius.CurrentStatInt, crystal.PoisonRadius.CurrentStatInt / 20);
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
    public sealed class RadiatingCrystalPet : LightPetItem
    {
        public LightPetStat Knockback = new(25, 0.01f, 0.05f);
        public LightPetStat DebuffedDefense = new(7, 1);
        public LightPetStat PoisonRadius = new(15, 12, 60);
        public override int LightPetItemID => CalamityLightPetIDs.Radiator;
        public override void UpdateInventory(Item item, Player player)
        {
            Knockback.SetRoll(player.luck);
            DebuffedDefense.SetRoll(player.luck);
            PoisonRadius.SetRoll(player.luck);
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
        public override int GetRoll() => Knockback.CurrentRoll;
        public override string PetsTooltip => Compatibility.LocVal("LightPetTooltips.RadiatingCrystal")

                        .Replace("<knockback>", Knockback.BaseAndPerQuality())
                        .Replace("<def>", DebuffedDefense.BaseAndPerQuality())
                        .Replace("<radius>", PoisonRadius.BaseAndPerQuality(Math.Round(PoisonRadius.StatPerRoll / 16f, 2).ToString(), Math.Round(PoisonRadius.BaseStat / 16f, 2).ToString()))

                        .Replace("<knockbackLine>", Knockback.StatSummaryLine())
                        .Replace("<defLine>", DebuffedDefense.StatSummaryLine())
                        .Replace("<radiusLine>", PoisonRadius.StatSummaryLine(Math.Round(PoisonRadius.CurrentStatInt / 16f, 2).ToString()));
    }
}