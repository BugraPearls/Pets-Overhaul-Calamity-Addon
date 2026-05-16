using CalamityMod;
using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader.IO;

namespace PetsOverhaulCalamityAddon.CalamityLightPets
{
    public sealed class ThiefsDimeEffect : LightPetEffect
    {
        public override int LightPetItemID => CalamityLightPetIDs.Goldie;
        public override void PostUpdateEquips()
        {
            if (TryGetLightPet(out ThiefsDimePet dime))
            {
                Player.Calamity().rogueVelocity += dime.RogueVelocity;
                Player.GetDamage<RogueDamageClass>() += dime.RogueDamage;
                Pet.globalFortune += dime.GlobalFortune;
            }
        }
        public override void ModifyLuck(ref float luck)
        {
            if (TryGetLightPet(out ThiefsDimePet dime))
            {
                luck += dime.Luck;
            }
        }
    }
    public sealed class ThiefsDimePet : LightPetItem
    {
        public LightPetStat Luck = new(16, 0.005f, "Luck", customStatDisplay: true, LegacyKeysToInherit: ("Stat1", 16));
        public LightPetStat RogueDamage = new(20, 0.002f, "Damage", 0.03f, LegacyKeysToInherit: ("Stat2", 20));
        public LightPetStat RogueVelocity = new(40, 0.003f, "Velocity", 0.043f, LegacyKeysToInherit: ("Stat3", 40));
        public LightPetStat GlobalFortune = new(15, 1, "Fortune", LegacyKeysToInherit: ("Stat4", 30));
        public override int LightPetItemID => CalamityLightPetIDs.Goldie;
        public override string BaseTooltip => Compatibility.LocVal("LightPetTooltips.ThiefsDime");
        public override void ModifyLightPetTooltip(ref string tooltip)
        {
            tooltip = tooltip.Replace("<0Luck>", Math.Round(Luck.CurrentStatFloat, 2).ToString()).Replace("<1Luck>", Luck.BaseStat.ToString()).Replace("<2Luck>", Luck.StatPerRoll.ToString());
        }
    }
}