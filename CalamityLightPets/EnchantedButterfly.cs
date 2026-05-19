using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using Terraria;

namespace PetsOverhaulCalamityAddon.CalamityLightPets
{
    public sealed class EnchantedButterflyEffect : LightPetEffect
    {
        public override int LightPetItemID => CalamityLightPetIDs.Sparks;
        public override void PostUpdateEquips()
        {
            if (TryGetLightPet(out EnchantedButterflyPet butter))
            {
                Pet.petHealMultiplier += butter.PetHealPower;
                Pet.harvestingFortune += butter.HarvestingFortune;
                Player.aggro += butter.Aggro;
                Pet.petDirectDamageMultiplier += butter.PetDamage;
            }
        }
    }
    public sealed class EnchantedButterflyPet : LightPetItem
    {
        public LightPetStat PetHealPower = new(30, 0.006f, "Heal", 0.07f, LegacyKeysToInherit: ("Stat1", 30));
        public LightPetStat HarvestingFortune = new(10, 2, "Fortune", 7, LegacyKeysToInherit: ("Stat2", 16));
        public LightPetStat Aggro = new(20, -10, "Aggro", -50, true, LegacyKeysToInherit: ("Stat3", 20));
        public LightPetStat PetDamage = new(10, 0.018f, "Damage", 0.05f, LegacyKeysToInherit: ("Stat4", 10));
        public override int LightPetItemID => CalamityLightPetIDs.Sparks;
        public override string BaseTooltip => Compatibility.LocVal("LightPetTooltips.EnchantedButterfly");
        public override void ModifyLightPetTooltip(ref string tooltip)
        {
            tooltip = tooltip.Replace("<0Aggro>", Aggro.CurrentStatInt.ToString()).Replace("<1Aggro>", Aggro.BaseStat.ToString()).Replace("<2Aggro>", Aggro.StatPerRoll.ToString());
        }

    }
}