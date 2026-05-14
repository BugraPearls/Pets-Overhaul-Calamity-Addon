using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

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
                luck += sus.Luck.CurrentStatFloat;
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
        public LightPetStat Luck = new(69, 0.01f,"NoGamblingPls", LegacyKeysToInherit: ("Stat1", 69));
        public LightPetStat Damage = new(67, -0.00067f, "BrainDamage", LegacyKeysToInherit: ("Stat2", 31));
        public LightPetStat FallBlocks = new(420, 1, "TheHarderYouFallFurtherYouClimb", LegacyKeysToInherit: ("Stat3", 420));
        public LightPetStat Sus = new(100, 0.01f, "RedWasNotTheImpostor", 0.01f, LegacyKeysToInherit: ("Stat4", 100));
        public override int LightPetItemID => CalamityLightPetIDs.Lilorde;
        public override string BaseTooltip => Compatibility.LocVal("LightPetTooltips.SuspiciousLookingNOU");
    }
}