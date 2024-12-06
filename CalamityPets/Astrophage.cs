using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using PetsOverhaul.NPCs;
using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class AstrophageEffect : PetEffect
    {
        public override int PetItemID => CalamityPetIDs.Astrophage;
        public override PetClasses PetClassPrimary => PetClasses.Utility;
        public override PetClasses PetClassSecondary => PetClasses.Offensive;
        public int infectRadius = 80;
        public int slowRadius = 600;
        public int infectDuration = 300;

        public float slowAmount = 0.4f;
        public float infectionHeavySlow = 2.2f;
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                GlobalPet.CircularDustEffect(Player.Center, DustID.CoralTorch, infectRadius, 8);
                GlobalPet.CircularDustEffect(Player.Center, DustID.Granite, slowRadius, 30, scale: 0.8f);
                foreach (var npc in Main.ActiveNPCs)
                {
                    if (Player.Distance(npc.Center) < infectRadius)
                    {
                        npc.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), infectDuration);
                    }

                    if (npc.TryGetGlobalNPC(out AstrophageInfection astro))
                    {
                        if (Player.Distance(npc.Center) < slowRadius && npc.Calamity().astralInfection > 0)
                        {
                            astro.infectedVal = infectionHeavySlow;
                            NpcPet.AddSlow(new NpcPet.PetSlow(slowAmount, 1, CalSlows.AstrophageSlow), npc);
                        }
                        else
                        {
                            astro.infectedVal = 0;
                        }
                    }
                }
            }
        }
    }
    public sealed class AstrophageInfection : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public float infectedVal = 0;
        public static int deathSpreadRange = 400;
        public static int deathSpreadSlowDur = 60;
        private int closestWhoAmI = -1;
        private float closestRange = 400;
        public override void OnKill(NPC npc)
        {
            if (infectedVal > 0 && npc.Calamity().astralInfection > 0)
            {
                GlobalPet.CircularDustEffect(npc.Center, DustID.DarkCelestial, deathSpreadRange, 40);
                foreach (var target in Main.ActiveNPCs)
                {
                    if (target == npc)
                        continue;
                    if (npc.Distance(target.Center) < deathSpreadRange && npc.Distance(target.Center) < closestRange)
                    {
                        closestRange = npc.Distance(target.Center);
                        closestWhoAmI = target.whoAmI;
                    }
                }
                if (closestWhoAmI != -1)
                {
                    Main.npc[closestWhoAmI].AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), npc.Calamity().astralInfection);
                    NpcPet.AddSlow(new NpcPet.PetSlow(infectedVal, deathSpreadSlowDur, CalSlows.AstrophageSlow), Main.npc[closestWhoAmI]);
                }
            }
        }
    }
    public sealed class AstrophageItemTooltip : PetTooltip
    {
        public override PetEffect PetsEffect => astro;
        public static AstrophageEffect astro
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out AstrophageEffect pet))
                    return pet;
                else
                    return ModContent.GetInstance<AstrophageEffect>();
            }
        }
        public override string PetsTooltip => Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.Astrophage")
                .Replace("<infectRadius>", Math.Round(astro.infectRadius / 16f, 2).ToString())
                .Replace("<infectDuration>", Math.Round(astro.infectDuration / 60f, 2).ToString())
                .Replace("<slowRadius>", Math.Round(astro.slowRadius / 16f, 2).ToString())
                .Replace("<slowAmount>", Math.Round(astro.slowAmount * 100, 2).ToString())
                .Replace("<spreadRadius>", Math.Round(AstrophageInfection.deathSpreadRange / 16f, 2).ToString())
                .Replace("<spreadSlow>", Math.Round(astro.infectionHeavySlow * 100, 2).ToString())
                .Replace("<spreadSlowDuration>", Math.Round(AstrophageInfection.deathSpreadSlowDur / 60f, 2).ToString());
    }
}
