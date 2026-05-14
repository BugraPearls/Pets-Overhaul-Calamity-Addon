using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader.IO;

namespace PetsOverhaulCalamityAddon.CalamityLightPets
{
    public sealed class RustedJingleBellEffect : LightPetEffect
    {
        public override int LightPetItemID => CalamityLightPetIDs.BabyGhostBell;
        public override void PostUpdateEquips()
        {
            if (TryGetLightPet(out RustedJingleBellPet bell))
            {
                Player.breathMax += bell.Breathe.CurrentStatInt / 7; //In vanilla how long Player can breathe by default is breathMax * 7 due to it ticking down every 7 frame.
                Pet.abilityHaste += bell.Haste.CurrentStatFloat;
                if (Collision.WetCollision(Player.position, Player.width, Player.height))
                {
                    Pet.miningFortune += bell.MiningFortuneInWater.CurrentStatInt;
                }
            }
        }
    }
    public sealed class RustedJingleBellPet : LightPetItem
    {
        public LightPetStat Breathe = new(30, 14,"Breathe", 90, LegacyKeysToInherit: ("Stat1", 30));
        public LightPetStat Haste = new(25, 0.002f, "Haste", 0.03f, LegacyKeysToInherit: ("Stat2", 25));
        public LightPetStat MiningFortuneInWater = new(10, 2, "Fortune", 10, LegacyKeysToInherit: ("Stat3", 10));
        public override int LightPetItemID => CalamityLightPetIDs.BabyGhostBell;
        public override string BaseTooltip => Compatibility.LocVal("LightPetTooltips.RustedJingleBell");
    }
}