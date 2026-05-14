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
        public override int LightPetItemID => CalamityLightPetIDs.Radiator;
        public override void PostUpdateEquips()
        {
            if (TryGetLightPet(out RadiatingCrystalPet crystal))
            {
                PetUtils.CircularDustEffect(Player.Center, DustID.CursedTorch, crystal.PoisonRadius.CurrentStatInt, crystal.PoisonRadius.CurrentStatInt / 20);
                Player.GetKnockback<GenericDamageClass>() += crystal.Knockback.CurrentStatFloat;
                for (int i = 0; i < Player.MaxBuffs; i++)
                {
                    if (Main.debuff[Player.buffType[i]] && BuffID.Sets.NurseCannotRemoveDebuff[Player.buffType[i]] == false)
                    {
                        Player.statDefense += crystal.DebuffedDefense.CurrentStatInt;
                    }
                }
                foreach (NPC npc in Main.ActiveNPCs)
                {
                    if (PetUtils.ValidTargetCheck(npc) && Player.Distance(npc.Center) < crystal.PoisonRadius.CurrentStatInt)
                    {
                        npc.AddBuff(BuffID.Poisoned, 60);
                    }
                }
            }
        }
    }
    public sealed class RadiatingCrystalPet : LightPetItem
    {
        public LightPetStat Knockback = new(25, 0.01f,"Knockback", 0.05f, LegacyKeysToInherit: ("Stat1", 25));
        public LightPetStat DebuffedDefense = new(7, 1, "Defense", LegacyKeysToInherit: ("Stat2", 7));
        public LightPetStat PoisonRadius = new(15, 12, "Poison", 60, LegacyKeysToInherit: ("Stat3", 15));
        public override int LightPetItemID => CalamityLightPetIDs.Radiator;
        public override string BaseTooltip => Compatibility.LocVal("LightPetTooltips.RadiatingCrystal");
    }
}