using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaulCalamityAddon.CalamityLightPets
{
    public sealed class BurrowerBeaconEffect : LightPetEffect
    {
        public const int msDuration = 30;
        public int escaping = 0;
        public override int LightPetItemID => CalamityLightPetIDs.Burrower;
        public override void PostUpdateEquips()
        {
            if (escaping >= 0)
            {
                escaping--;
            }
            if (TryGetLightPet(out BurrowerBeaconPet burrow))
            {
                Pet.miningFortune += burrow.MiningFortune;
                Player.pickSpeed -= Player.pickSpeed * burrow.MiningSpeed.CurrentStatFloat;

                if (escaping > 0)
                {
                    Player.moveSpeed += burrow.MovementSpeedOnHurt;
                }
            }
        }
        public override void OnHurt(Player.HurtInfo info)
        {
            if (Player.CurrentLightPet() == LightPetItemID)
            {
                escaping = msDuration;
            }
        }
    }
    public sealed class BurrowerBeaconPet : LightPetItem
    {
        public LightPetStat MiningSpeed = new(8,0.0125f,"MiningSpeed",0.1f);
        public LightPetStat MiningFortune = new(12,1,"Fortune");
        public LightPetStat MovementSpeedOnHurt = new(15,0.02f,"MovementSpeed",0.5f);
        public override int LightPetItemID => CalamityLightPetIDs.Burrower;
        public override string BaseTooltip => Compatibility.LocVal("LightPetTooltips.BurrowerBeacon");
        public override void ModifyLightPetTooltip(ref string tooltip)
        {
            tooltip = tooltip.Replace("<msDuration>", PetUtils.Secondize(BurrowerBeaconEffect.msDuration));
        }
    }
}