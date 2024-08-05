using PetsOverhaulCalamityAddon.Systems;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon
{
    public class PetsOverhaulCalamityAddon : Mod
    {
        public override void Load()
        {
            Compatibility.AddPetItemNames();
        }
    }
}
