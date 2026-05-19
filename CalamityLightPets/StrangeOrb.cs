using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using Terraria;

namespace PetsOverhaulCalamityAddon.CalamityLightPets
{
    public sealed class StrangeOrbEffect : LightPetEffect
    {
        public override int LightPetItemID => CalamityLightPetIDs.OceanSpirit;
        public override void PostUpdateEquips()
        {
            if (TryGetLightPet(out StrangeOrbPet orb))
            {
                Player.fishingSkill += orb.FishingPower;
                Pet.fishingFortune += orb.FishingFortune;
                if (Collision.WetCollision(Player.position, Player.width, Player.height))
                {
                    Pet.petHealMultiplier += orb.HealInWater;
                }
            }
        }
    }
    public sealed class StrangeOrbPet : LightPetItem
    {
        public LightPetStat FishingPower = new(8, 1, "Fishing", LegacyKeysToInherit: ("Stat1", 8));
        public LightPetStat FishingFortune = new(25, 1, "Fortune", LegacyKeysToInherit: ("Stat2", 25));
        public LightPetStat HealInWater = new(35, 0.006f, "Healing", 0.1f, LegacyKeysToInherit: ("Stat3", 35));
        public override int LightPetItemID => CalamityLightPetIDs.OceanSpirit;
        public override string BaseTooltip => Compatibility.LocVal("LightPetTooltips.StrangeOrb");
    }
}