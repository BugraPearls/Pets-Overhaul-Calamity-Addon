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
                Player.aggro += light.Aggro.CurrentStatInt;
                Pet.petShieldMultiplier += light.ShieldIncrease.CurrentStatFloat;
                Player.statLifeMax2 += light.Health.CurrentStatInt;
            }
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (TryGetLightPet(out LittleLightPet light))
            {
                modifiers.Knockback *= 1f - light.KnockbackResist.CurrentStatFloat;
            }
        }
    }
    public sealed class LittleLightPet : LightPetItem
    {
        public LightPetStat Aggro = new(20, 10,"Aggro", 50, LegacyKeysToInherit: ("Stat1", 20));
        public LightPetStat KnockbackResist = new(20, 0.02f, "KbResist", 0.2f, LegacyKeysToInherit: ("Stat2", 20));
        public LightPetStat ShieldIncrease = new(20, 0.008f, "Shield", 0.06f, LegacyKeysToInherit: ("Stat3", 20));
        public LightPetStat Health = new(20, 2, "Health", 25, LegacyKeysToInherit: ("Stat4", 20));
        public override int LightPetItemID => CalamityLightPetIDs.LittleLight;
        public override string BaseTooltip => Compatibility.LocVal("LightPetTooltips.LittleLight");

    }
}