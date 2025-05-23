﻿using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaulCalamityAddon.CalamityLightPets
{
    public sealed class SuspiciousLookingNOUEffect : LightPetEffect
    {
        public override int LightPetItemID => CalamityLightPetIDs.Lilorde;
        public override void PostUpdateEquips()
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out SuspiciousLookingNOUPet sus))
            {
                Player.extraFall += sus.FallBlocks.CurrentStatInt;
                Player.GetDamage<GenericDamageClass>() += sus.Damage.CurrentStatFloat;
            }
        }
        public override void ModifyLuck(ref float luck)
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out SuspiciousLookingNOUPet sus))
            {
                luck += sus.Luck.CurrentStatFloat;
            }
        }
        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out SuspiciousLookingNOUPet sus))
            {
                g -= sus.Sus.CurrentStatFloat * 0.5f;
                b -= sus.Sus.CurrentStatFloat * 0.5f;
            }
        }
    }
    public sealed class SuspiciousLookingNOUPet : LightPetItem
    {
        public LightPetStat Luck = new(69, 0.01f);
        public LightPetStat Damage = new(31, -0.003f);
        public LightPetStat FallBlocks = new(420, 1);
        public LightPetStat Sus = new(100, 0.01f, 0.01f);
        public override int LightPetItemID => CalamityLightPetIDs.Lilorde;
        public override void UpdateInventory(Item item, Player player)
        {
            Luck.SetRoll(player.luck);
            Damage.SetRoll(player.luck);
            FallBlocks.SetRoll(player.luck);
            Sus.SetRoll(player.luck);
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)Luck.CurrentRoll);
            writer.Write((byte)Damage.CurrentRoll);
            writer.Write((short)FallBlocks.CurrentRoll);
            writer.Write((byte)Sus.CurrentRoll);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            Luck.CurrentRoll = reader.ReadByte();
            Damage.CurrentRoll = reader.ReadByte();
            FallBlocks.CurrentRoll = reader.ReadInt16();
            Sus.CurrentRoll = reader.ReadByte();
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("Stat1", Luck.CurrentRoll);
            tag.Add("Stat2", Damage.CurrentRoll);
            tag.Add("Stat3", FallBlocks.CurrentRoll);
            tag.Add("Stat4", Sus.CurrentRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.TryGet("Stat1", out int luck))
            {
                Luck.CurrentRoll = luck;
            }

            if (tag.TryGet("Stat2", out int damage))
            {
                Damage.CurrentRoll = damage;
            }

            if (tag.TryGet("Stat3", out int fall))
            {
                FallBlocks.CurrentRoll = fall;
            }

            if (tag.TryGet("Stat4", out int sus))
            {
                Sus.CurrentRoll = sus;
            }
        }
        public override int GetRoll() => Sus.CurrentRoll;
        public override string PetsTooltip => Compatibility.LocVal("LightPetTooltips.SuspiciousLookingNOU")

                        .Replace("<luck>", Luck.BaseAndPerQuality(Luck.StatPerRoll.ToString()))
                        .Replace("<damage>", Damage.BaseAndPerQuality())
                        .Replace("<fall>", FallBlocks.BaseAndPerQuality())
                        .Replace("<sus>", Sus.BaseAndPerQuality())

                        .Replace("<luckLine>", Luck.StatSummaryLine(Math.Round(Luck.CurrentStatFloat, 2).ToString()))
                        .Replace("<damageLine>", Damage.StatSummaryLine())
                        .Replace("<fallLine>", FallBlocks.StatSummaryLine())
                        .Replace("<susLine>", Sus.StatSummaryLine());
    }
}