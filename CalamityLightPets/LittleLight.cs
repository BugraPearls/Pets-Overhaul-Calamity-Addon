using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System.IO;
using Terraria;
using Terraria.ModLoader.IO;

namespace PetsOverhaulCalamityAddon.CalamityLightPets
{
    public sealed class LittleLightEffect : LightPetEffect
    {
        public override int LightPetItemID => CalamityLightPetIDs.LittleLight;
        public override void PostUpdateEquips()
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out LittleLightPet light))
            {
                Player.aggro += light.Aggro.CurrentStatInt;
                Pet.petShieldMultiplier += light.ShieldIncrease.CurrentStatFloat;
                Player.statLifeMax2 += light.Health.CurrentStatInt;
            }
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (Player.miscEquips[1].TryGetGlobalItem(out LittleLightPet light))
            {
                modifiers.Knockback *= 1f - light.KnockbackResist.CurrentStatFloat;
            }
        }
    }
    public sealed class LittleLightPet : LightPetItem
    {
        public LightPetStat Aggro = new(20, 10, 50);
        public LightPetStat KnockbackResist = new(20, 0.02f, 0.2f);
        public LightPetStat ShieldIncrease = new(20, 0.008f, 0.06f);
        public LightPetStat Health = new(20, 2, 25);
        public override int LightPetItemID => CalamityLightPetIDs.LittleLight;
        public override void UpdateInventory(Item item, Player player)
        {
            Aggro.SetRoll(player.luck);
            KnockbackResist.SetRoll(player.luck);
            ShieldIncrease.SetRoll(player.luck);
            Health.SetRoll(player.luck);
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)Aggro.CurrentRoll);
            writer.Write((byte)KnockbackResist.CurrentRoll);
            writer.Write((byte)ShieldIncrease.CurrentRoll);
            writer.Write((byte)Health.CurrentRoll);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            Aggro.CurrentRoll = reader.ReadByte();
            KnockbackResist.CurrentRoll = reader.ReadByte();
            ShieldIncrease.CurrentRoll = reader.ReadByte();
            Health.CurrentRoll = reader.ReadByte();
        }
        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("Stat1", Aggro.CurrentRoll);
            tag.Add("Stat2", KnockbackResist.CurrentRoll);
            tag.Add("Stat3", ShieldIncrease.CurrentRoll);
            tag.Add("Stat4", Health.CurrentRoll);
        }
        public override void LoadData(Item item, TagCompound tag)
        {
            if (tag.TryGet("Stat1", out int aggro))
            {
                Aggro.CurrentRoll = aggro;
            }

            if (tag.TryGet("Stat2", out int kb))
            {
                KnockbackResist.CurrentRoll = kb;
            }

            if (tag.TryGet("Stat3", out int shield))
            {
                ShieldIncrease.CurrentRoll = shield;
            }

            if (tag.TryGet("Stat4", out int health))
            {
                Health.CurrentRoll = health;
            }
        }
        public override int GetRoll() => Health.CurrentRoll;
        public override string PetsTooltip => Compatibility.LocVal("LightPetTooltips.LittleLight")

                        .Replace("<aggro>", Aggro.BaseAndPerQuality())
                        .Replace("<kbResist>", KnockbackResist.BaseAndPerQuality())
                        .Replace("<shield>", ShieldIncrease.BaseAndPerQuality())
                        .Replace("<health>", Health.BaseAndPerQuality())

                        .Replace("<aggroLine>", Aggro.StatSummaryLine())
                        .Replace("<kbResistLine>", KnockbackResist.StatSummaryLine())
                        .Replace("<shieldLine>", ShieldIncrease.StatSummaryLine())
                        .Replace("<healthLine>", Health.StatSummaryLine());

    }
}