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
                Player.Calamity().rogueVelocity += dime.RogueVelocity.CurrentStatFloat;
                Player.Calamity().stealthGenStandstill += dime.StealthGain.CurrentStatFloat;
                Player.Calamity().stealthGenMoving += dime.StealthGain.CurrentStatFloat;
                Player.GetDamage<RogueDamageClass>() += dime.RogueDamage.CurrentStatFloat;
            }
        }
        public override void ModifyLuck(ref float luck)
        {
            if (TryGetLightPet(out ThiefsDimePet dime))
            {
                luck += dime.Luck.CurrentStatFloat;
            }
        }
    }
    public sealed class ThiefsDimePet : LightPetItem
    {
        public LightPetStat Luck = new(16, 0.005f, "Luck", LegacyKeysToInherit: ("Stat1", 16));
        public LightPetStat RogueDamage = new(20, 0.0025f, "Damage", 0.05f, LegacyKeysToInherit: ("Stat2", 20));
        public LightPetStat RogueVelocity = new(40, 0.004f, "Velocity", 0.04f, LegacyKeysToInherit: ("Stat3", 40));
        public LightPetStat StealthGain = new(30, 0.002f, "Stealth", 0.03f, LegacyKeysToInherit: ("Stat4", 30));
        public override int LightPetItemID => CalamityLightPetIDs.Goldie;
        public override string BaseTooltip => Compatibility.LocVal("LightPetTooltips.ThiefsDime");
    }
}