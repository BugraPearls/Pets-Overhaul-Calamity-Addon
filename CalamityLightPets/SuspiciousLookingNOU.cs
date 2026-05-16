using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static CalamityMod.Projectiles.Melee.BladecrestOathswordThrownBlade;

namespace PetsOverhaulCalamityAddon.CalamityLightPets
{
    public sealed class SuspiciousLookingNOUEffect : LightPetEffect
    {
        public override int LightPetItemID => CalamityLightPetIDs.Lilorde;
        public override void PostUpdateEquips()
        {
            if (TryGetLightPet(out SuspiciousLookingNOUPet sus))
            {
                Player.extraFall += sus.FallBlocks.CurrentStatInt;
                Player.GetDamage<GenericDamageClass>() += sus.Damage.CurrentStatFloat;
            }
        }
        public override void ModifyLuck(ref float luck)
        {
            if (TryGetLightPet(out SuspiciousLookingNOUPet sus))
            {
                luck += sus.Luck;
            }
        }
        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (TryGetLightPet(out SuspiciousLookingNOUPet sus))
            {
                g -= sus.Sus.CurrentStatFloat * 0.5f;
                b -= sus.Sus.CurrentStatFloat * 0.5f;
            }
        }
    }
    public sealed class SuspiciousLookingNOUPet : LightPetItem
    {
        public LightPetStat Luck = new(69, 0.01f,"NoGamblingPls", customStatDisplay: true,LegacyKeysToInherit: ("Stat1", 69));
        public LightPetStat Damage = new(67, -0.00067f, "BrainDamage", LegacyKeysToInherit: ("Stat2", 31));
        public LightPetStat FallBlocks = new(420, 1, "TheHarderYouFallFurtherYouClimb", LegacyKeysToInherit: ("Stat3", 420));
        public LightPetStat Sus = new(100, 0.01f, "RedWasNotTheImpostor", 0.01f, LegacyKeysToInherit: ("Stat4", 100));
        public override int LightPetItemID => CalamityLightPetIDs.Lilorde;
        public override string BaseTooltip => Compatibility.LocVal("LightPetTooltips.SuspiciousLookingNOU");
        public override void ExtraNetSend(Item item, BinaryWriter writer)
        {
            writer.Write((short)FallBlocks.CurrentRoll);
        }
        public override void ExtraNetReceive(Item item, BinaryReader reader)
        {
            FallBlocks.CurrentRoll = reader.ReadInt16();
        }
        public override void ModifyLightPetTooltip(ref string tooltip)
        {
            tooltip = tooltip.Replace("<0NoGamblingPls>", Math.Round(Luck.CurrentStatFloat, 2).ToString()).Replace("<1NoGamblingPls>", Luck.BaseStat.ToString()).Replace("<2NoGamblingPls>", Luck.StatPerRoll.ToString());
        }
    }
}