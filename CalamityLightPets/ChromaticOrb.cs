using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaulCalamityAddon.CalamityLightPets
{
    public sealed class ChromaticOrbEffect : LightPetEffect
    {
        public override int LightPetItemID => CalamityLightPetIDs.Yuu;
        public override void PostUpdateEquips()
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out ChromaticOrbPet orb))
            {
                Pet.abilityHaste += orb.AbilityHaste.CurrentStatFloat;
                Player.GetCritChance<GenericDamageClass>() += orb.CritChance.CurrentStatFloat * 100;
                Player.jumpSpeedBoost += Player.jumpSpeed * orb.JumpSpeed.CurrentStatFloat;
            }
        }
    }
    public sealed class ChromaticOrbPet : LightPetItem
    {
        public LightPetStat AbilityHaste = new(40, 0.0045f, 0.07f);
        public LightPetStat CritChance = new(30, 0.002f, 0.03f);
        public LightPetStat JumpSpeed = new(10, 0.015f, 0.05f);
        //Thought about adding bonus Rage damage, but could be too much for 1 pet regarding damage output. Maybe in future if seems ok.
        public override int LightPetItemID => CalamityLightPetIDs.Yuu;
        public override void UpdateInventory(Item item, Player player)
        {
            AbilityHaste.SetRoll(player.luck);
            CritChance.SetRoll(player.luck);
            JumpSpeed.SetRoll(player.luck);
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)AbilityHaste.CurrentRoll);
            writer.Write((byte)CritChance.CurrentRoll);
            writer.Write((byte)JumpSpeed.CurrentRoll);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            AbilityHaste.CurrentRoll = reader.ReadByte();
            CritChance.CurrentRoll = reader.ReadByte();
            JumpSpeed.CurrentRoll = reader.ReadByte();
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("Stat1", AbilityHaste.CurrentRoll);
            tag.Add("Stat2", CritChance.CurrentRoll);
            tag.Add("Stat3", JumpSpeed.CurrentRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.TryGet("Stat1", out int haste))
            {
                AbilityHaste.CurrentRoll = haste;
            }

            if (tag.TryGet("Stat2", out int crit))
            {
                CritChance.CurrentRoll = crit;
            }

            if (tag.TryGet("Stat3", out int jump))
            {
                JumpSpeed.CurrentRoll = jump;
            }
        }
        public override int GetRoll() => AbilityHaste.CurrentRoll;
        public override string PetsTooltip => Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.LightPetTooltips.ChromaticOrb")

                        .Replace("<haste>", AbilityHaste.BaseAndPerQuality())
                        .Replace("<crit>", CritChance.BaseAndPerQuality())
                        .Replace("<jump>", JumpSpeed.BaseAndPerQuality())

                        .Replace("<hasteLine>", AbilityHaste.StatSummaryLine())
                        .Replace("<critLine>", CritChance.StatSummaryLine())
                        .Replace("<jumpLine>", JumpSpeed.StatSummaryLine());
    }
}