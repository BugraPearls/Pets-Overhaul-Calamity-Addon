﻿using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader.IO;

namespace PetsOverhaulCalamityAddon.CalamityLightPets
{
    public sealed class RustedJingleBellEffect : LightPetEffect
    {
        public override int LightPetItemID => CalamityLightPetIDs.BabyGhostBell;
        public override void PostUpdateEquips()
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
    public sealed class RustedJingleBellPet : LightPetItem
    {
        public LightPetStat Breathe = new(30, 14, 90);
        public LightPetStat Haste = new(25, 0.002f, 0.03f);
        public LightPetStat MiningFortuneInWater = new(10, 2, 10);
        public override int LightPetItemID => CalamityLightPetIDs.BabyGhostBell;
        public override void UpdateInventory(Item item, Player player)
        {
            Breathe.SetRoll(player.luck);
            Haste.SetRoll(player.luck);
            MiningFortuneInWater.SetRoll(player.luck);
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
        public override int GetRoll() => Breathe.CurrentRoll;
        public override string PetsTooltip => Compatibility.LocVal("LightPetTooltips.RustedJingleBell")

                        .Replace("<breathe>", Breathe.BaseAndPerQuality(Math.Round(Breathe.StatPerRoll / 60f, 2).ToString(), Math.Round(Breathe.BaseStat / 60f, 2).ToString()))
                        .Replace("<haste>", Haste.BaseAndPerQuality())
                        .Replace("<fortune>", MiningFortuneInWater.BaseAndPerQuality())

                        .Replace("<breatheLine>", Breathe.StatSummaryLine(Math.Round(Breathe.CurrentStatInt / 60f, 2).ToString()))
                        .Replace("<hasteLine>", Haste.StatSummaryLine())
                        .Replace("<fortuneLine>", MiningFortuneInWater.StatSummaryLine());
    }
}