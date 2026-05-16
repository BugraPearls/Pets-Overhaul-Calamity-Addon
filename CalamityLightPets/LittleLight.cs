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
            if (TryGetLightPet(out LittleLightPet light))
            {
                Player.aggro += light.Aggro;
                Pet.petShieldMultiplier += light.ShieldIncrease;
                Player.statLifeMax2 += light.Health;
                Pet.knockbackResistance += light.KnockbackResist;
            }
        }
    }
    public sealed class LittleLightPet : LightPetItem
    {
        public LightPetStat Aggro = new(20, 10,"Aggro", 150, LegacyKeysToInherit: ("Stat1", 20));
        public LightPetStat KnockbackResist = new(10, 0.02f, "KbResist", 0.8f, LegacyKeysToInherit: ("Stat2", 20));
        public LightPetStat ShieldIncrease = new(20, 0.0115f, "Shield", 0.1f, LegacyKeysToInherit: ("Stat3", 20));
        public LightPetStat Health = new(20, 2, "Health", 20, LegacyKeysToInherit: ("Stat4", 20));
        public override int LightPetItemID => CalamityLightPetIDs.LittleLight;
        public override string BaseTooltip => Compatibility.LocVal("LightPetTooltips.LittleLight");

    }
}