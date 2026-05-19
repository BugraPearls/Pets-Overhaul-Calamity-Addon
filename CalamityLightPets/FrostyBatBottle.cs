using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using Terraria;
using Terraria.ID;

namespace PetsOverhaulCalamityAddon.CalamityLightPets
{
    public sealed class FrostyBatBottleEffect : LightPetEffect
    {
        public int escaping = 0;
        public const int slowDuration = 180;
        public override int LightPetItemID => CalamityLightPetIDs.FrostyBat;
        public override void PostUpdateEquips()
        {
            if (TryGetLightPet(out FrostyBatBottlePet frosty))
            {
                Pet.petSlowPotency += frosty.SlowPotency;
                Pet.knockbackResistance += frosty.KnockbackResist;

                if (Player.dashDelay == -1)
                {
                    PetUtils.CircularDustEffect(Player.Center, DustID.HallowSpray, 60, 6);
                    foreach (var npc in Main.ActiveNPCs)
                    {
                        if (Player.Distance(npc.Center) <= 60)
                        {
                            PetGlobalNPC.AddSlow(new(frosty.SlowOnDash.CurrentStatFloat, slowDuration, 0), npc, Player);
                        }
                    }
                }
            }
        }

    }
    public sealed class FrostyBatBottlePet : LightPetItem
    {
        public LightPetStat SlowPotency = new(30, 0.008f, "SlowPotency", 0.20f);
        public LightPetStat KnockbackResist = new(15, 0.005f, "KnockbackResist", 0.1f);
        public LightPetStat SlowOnDash = new(25, 0.0125f, "SlowOnDash", 0.1f);
        public override int LightPetItemID => CalamityLightPetIDs.FrostyBat;
        public override string BaseTooltip => Compatibility.LocVal("LightPetTooltips.FrostyBatBottle");
        public override void ModifyLightPetTooltip(ref string tooltip)
        {
            tooltip = tooltip.Replace("<slowDuration>", PetUtils.Secondize(FrostyBatBottleEffect.slowDuration));
        }
    }
}