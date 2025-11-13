using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using PetsOverhaul.NPCs;
using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System;
using Terraria;
using Terraria.ID;
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
        public float dmgBoostPerInfected = 0.03f;
        public int dmgInfectCap = 10;
        public int infectCount = 0;
        public float slowAmount = 0.4f;
        public float infectionHeavySlow = 2.2f;
        public override int PetStackMax => dmgInfectCap;
        public override int PetStackCurrent => infectCount;
        public override string PetStackText => Compatibility.LocVal("PetTooltips.AstrophageStack");
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                PetModPlayer.CircularDustEffect(Player.Center, DustID.CoralTorch, infectRadius, 8);
                PetModPlayer.CircularDustEffect(Player.Center, DustID.Granite, slowRadius, 30, scale: 0.8f);
                infectCount = 0;
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
                            PetGlobalNPC.AddSlow(new PetGlobalNPC.PetSlow(slowAmount, 1, CalSlows.AstrophageSlow), npc);
                            infectCount++;
                        }
                        else
                        {
                            astro.infectedVal = 0;
                        }
                    }
                }
                Player.GetDamage<GenericDamageClass>() += Math.Min(dmgInfectCap, infectCount) * dmgBoostPerInfected;
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
                PetModPlayer.CircularDustEffect(npc.Center, DustID.DarkCelestial, deathSpreadRange, 40);
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
                    PetGlobalNPC.AddSlow(new PetGlobalNPC.PetSlow(infectedVal, deathSpreadSlowDur, CalSlows.AstrophageSlow), Main.npc[closestWhoAmI]);
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
        public override string PetsTooltip => Compatibility.LocVal("PetTooltips.Astrophage")
                .Replace("<infectRadius>", Math.Round(astro.infectRadius / 16f, 2).ToString())
                .Replace("<infectDuration>", Math.Round(astro.infectDuration / 60f, 2).ToString())
                .Replace("<slowRadius>", Math.Round(astro.slowRadius / 16f, 2).ToString())
                .Replace("<slowAmount>", Math.Round(astro.slowAmount * 100, 2).ToString())
                .Replace("<dmg>", Math.Round(astro.dmgBoostPerInfected * 100, 2).ToString())
                .Replace("<cap>", astro.dmgInfectCap.ToString())
                .Replace("<spreadRadius>", Math.Round(AstrophageInfection.deathSpreadRange / 16f, 2).ToString())
                .Replace("<spreadSlow>", Math.Round(astro.infectionHeavySlow * 100, 2).ToString())
                .Replace("<spreadSlowDuration>", Math.Round(AstrophageInfection.deathSpreadSlowDur / 60f, 2).ToString());
        public override string SimpleTooltip => Compatibility.LocVal("SimpleTooltips.Astrophage");
    }
}
