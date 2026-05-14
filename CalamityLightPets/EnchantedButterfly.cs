using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System.IO;
using Terraria;
using Terraria.ModLoader.IO;

namespace PetsOverhaulCalamityAddon.CalamityLightPets
{
    public sealed class EnchantedButterflyEffect : LightPetEffect
    {
        public override int LightPetItemID => CalamityLightPetIDs.Sparks;
        public override void PostUpdateEquips()
        {
            if (TryGetLightPet(out EnchantedButterflyPet butter))
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
        public LightPetStat PetHealPower = new(30, 0.006f,"Heal", 0.07f, LegacyKeysToInherit: ("Stat1", 30));
        public LightPetStat GlobalFortune = new(16, 1, "Fortune", 6, LegacyKeysToInherit: ("Stat2", 16));
        public LightPetStat Aggro = new(20, -5, "Aggro", -40, LegacyKeysToInherit: ("Stat3", 20));
        public LightPetStat PetDamage = new(10, 0.01f, "Damage", 0.05f, LegacyKeysToInherit: ("Stat4", 10));
        public override int LightPetItemID => CalamityLightPetIDs.Sparks;
        public override string BaseTooltip => Compatibility.LocVal("LightPetTooltips.EnchantedButterfly");

    }
}