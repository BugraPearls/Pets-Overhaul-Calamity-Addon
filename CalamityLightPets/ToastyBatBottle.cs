using CalamityMod;
using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaulCalamityAddon.CalamityLightPets
{
    public sealed class ToastyBatBottleEffect : LightPetEffect
    {
        public int escaping = 0;
        public override int LightPetItemID => CalamityLightPetIDs.ToastyBat;
        public override void PostUpdateEquips()
        {
            if (TryGetLightPet(out ToastyBatBottlePet toasty))
            {
                Pet.petDirectDamageMultiplier += toasty.PetDamage;
                Player.Calamity().RageDuration += toasty.RageDuration;

                if (Player.dashDelay == -1)
                {
                    PetUtils.CircularDustEffect(Player.Center, DustID.LavaMoss, 60, 6);
                    foreach (var npc in Main.ActiveNPCs)
                    {
                        if (PetUtils.ValidTargetCheck(npc) && Player.Distance(npc.Center) <= 60)
                        {
                            npc.AddBuff(BuffID.OnFire, toasty.BurnOnDash.CurrentStatInt);
                        }
                    }
                }
            }
        }

    }
    public sealed class ToastyBatBottlePet : LightPetItem
    {
        public LightPetStat RageDuration = new(20, 3, "Rage",30, true);
        public LightPetStat PetDamage = new(10, 0.015f, "PetDamage", 0.07f);
        public LightPetStat BurnOnDash = new(15, 18, "BurnOnDash", 120, true);
        public override int LightPetItemID => CalamityLightPetIDs.ToastyBat;
        public override string BaseTooltip => Compatibility.LocVal("LightPetTooltips.ToastyBatBottle");
        public override void ModifyLightPetTooltip(ref string tooltip)
        {
            tooltip = tooltip.Replace("<0BurnOnDash>", PetUtils.Secondize(BurnOnDash.CurrentStatInt)).Replace("<1BurnOnDash>", PetUtils.Secondize((int)BurnOnDash.BaseStat)).Replace("<2BurnOnDash>", PetUtils.Secondize((int)BurnOnDash.StatPerRoll))
                .Replace("<0Rage>", PetUtils.Secondize(RageDuration.CurrentStatInt)).Replace("<1Rage>", PetUtils.Secondize((int)RageDuration.BaseStat)).Replace("<2Rage>", PetUtils.Secondize((int)RageDuration.StatPerRoll));
        }
    }
}