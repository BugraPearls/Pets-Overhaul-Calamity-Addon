using PetsOverhaulCalamityAddon.Systems;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon
{
    public class PetsOverhaulCalamityAddon : Mod
    {
        public override void Load()
        {
            Compatibility.AddPetItemNames();
            Compatibility.AddCalamityItemsToGatheringLists();
            Compatibility.AddCalamityNonBossTrueBosses();
            Compatibility.AddCalamityCorruptEnemies();
            Compatibility.AddCalamityCrimsonEnemies();
            Compatibility.AddCalamityHallowEnemies();
            Compatibility.AddCalamitySoundEffects();
            Compatibility.AddCalamityPetSlows();
            Compatibility.AddCalamityItemLists();
        }
    }
}
