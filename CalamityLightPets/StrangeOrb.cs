using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System.IO;
using Terraria;
using Terraria.ModLoader.IO;

namespace PetsOverhaulCalamityAddon.CalamityLightPets
{
    public sealed class StrangeOrbEffect : LightPetEffect
    {
        public override int LightPetItemID => CalamityLightPetIDs.OceanSpirit;
        public override void PostUpdateEquips()
        {
            if (TryGetLightPet(out StrangeOrbPet orb))
            {
                Player.fishingSkill += orb.FishingPower.CurrentStatInt;
                Pet.fishingFortune += orb.FishingFortune.CurrentStatInt;
                if (Collision.WetCollision(Player.position, Player.width, Player.height))
                {
                    Pet.petHealMultiplier += orb.HealInWater.CurrentStatFloat;
                }
            }
        }
    }
    public sealed class StrangeOrbPet : LightPetItem
    {
        public LightPetStat FishingPower = new(8, 1,"Fishing", LegacyKeysToInherit: ("Stat1", 8));
        public LightPetStat FishingFortune = new(25, 1, "Fortune", LegacyKeysToInherit: ("Stat2", 25));
        public LightPetStat HealInWater = new(35, 0.005f, "Healing", 0.07f, LegacyKeysToInherit: ("Stat3", 35));
        public override int LightPetItemID => CalamityLightPetIDs.OceanSpirit;
        public override string BaseTooltip => Compatibility.LocVal("LightPetTooltips.StrangeOrb");
    }
}