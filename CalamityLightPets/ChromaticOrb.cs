using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaulCalamityAddon.CalamityLightPets
{
    public sealed class ChromaticOrbEffect : LightPetEffect
    {
        public override int LightPetItemID => CalamityLightPetIDs.Yuu;
        public override void PostUpdateEquips()
        {
            if (TryGetLightPet(out ChromaticOrbPet orb))
            {
                Pet.abilityHaste += orb.AbilityHaste;
                Player.GetCritChance<GenericDamageClass>() += orb.CritChance.CurrentStatFloat * 100;
                Player.jumpSpeedBoost += Player.jumpSpeed * orb.JumpSpeed.CurrentStatFloat;
            }
        }
    }
    public sealed class ChromaticOrbPet : LightPetItem
    {
        public LightPetStat AbilityHaste = new(40, 0.004f, "Haste", 0.08f, LegacyKeysToInherit: ("Stat1", 40));
        public LightPetStat CritChance = new(30, 0.0015f, "Crit", 0.025f, LegacyKeysToInherit: ("Stat2", 30));
        public LightPetStat JumpSpeed = new(10, 0.015f, "Jump", 0.05f, LegacyKeysToInherit: ("Stat3", 10));
        public override int LightPetItemID => CalamityLightPetIDs.Yuu;
        public override string BaseTooltip => Compatibility.LocVal("LightPetTooltips.ChromaticOrb");
    }
}