using CalamityMod;
using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaulCalamityAddon.CalamityLightPets
{
    public sealed class RadiatingCrystalEffect : LightPetEffect
    {
        public float lastSulphPoisoning = 0;
        public override int LightPetItemID => CalamityLightPetIDs.Radiator;
        public override void PostUpdateEquips()
        {
            if (TryGetLightPet(out RadiatingCrystalPet crystal))
            {
                PetUtils.CircularDustEffect(Player.Center, DustID.CursedTorch, crystal.PoisonRadius.CurrentStatInt, crystal.PoisonRadius.CurrentStatInt / 20);
                Player.GetKnockback<GenericDamageClass>() += crystal.Knockback.CurrentStatFloat;
                Pet.petSlowPotency += crystal.SlowPotency;
                foreach (NPC npc in Main.ActiveNPCs)
                {
                    if (PetUtils.ValidTargetCheck(npc) && Player.Distance(npc.Center) < crystal.PoisonRadius.CurrentStatInt)
                    {
                        npc.AddBuff(BuffID.Poisoned, 60);
                    }
                }

                if (Player.Calamity().SulphWaterPoisoningLevel > lastSulphPoisoning)
                {
                    Player.Calamity().SulphWaterPoisoningLevel -= (Player.Calamity().SulphWaterPoisoningLevel - lastSulphPoisoning) * (1-(1 / (1 + crystal.SulphurPoisonResistance.CurrentStatFloat / 1)));
                }
            }
            lastSulphPoisoning = Player.Calamity().SulphWaterPoisoningLevel; //Needs to be tracked outside of Light Pet, otherwise can be exploited
        }
    }
    public sealed class RadiatingCrystalPet : LightPetItem
    {
        public LightPetStat Knockback = new(25, 0.006f,"Knockback", 0.05f, LegacyKeysToInherit: ("Stat1", 25));
        public LightPetStat SlowPotency = new(5, 0.03f, "Slow", 0.05f, LegacyKeysToInherit: ("Stat2", 7));
        public LightPetStat PoisonRadius = new(15, 12, "Poison", 60, true, LegacyKeysToInherit: ("Stat3", 15));
        public LightPetStat SulphurPoisonResistance = new(10, 0.015f, "SulphurResist", 0.2f);
        public override int LightPetItemID => CalamityLightPetIDs.Radiator;
        public override string BaseTooltip => Compatibility.LocVal("LightPetTooltips.RadiatingCrystal");
        public override void ModifyLightPetTooltip(ref string tooltip)
        {
            tooltip = tooltip.Replace("<0Poison>", (PoisonRadius.CurrentStatInt / 16).ToString()).Replace("<1Poison>", (PoisonRadius.BaseStat / 16).ToString()).Replace("<2Poison>", (PoisonRadius.StatPerRoll/16).ToString());
        }
    }
}