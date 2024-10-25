using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Dusts;
using CalamityMod.Graphics.Renderers.CalamityRenderers;
using PetsOverhaul.Config;
using PetsOverhaul.NPCs;
using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class AstrophageEffect : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Utility;
        public override PetClasses PetClassSecondary => PetClasses.Offensive;
        public int infectRadius = 80;
        public int slowRadius = 600;
        public int infectDuration = 300;

        public float slowAmount = 0.4f;
        public float infectionHeavySlow = 2.2f;
        public override void PostUpdateMiscEffects()
        {
            if (Pet.PetInUseWithSwapCd(CalamityPetIDs.Astrophage))
            {
                GlobalPet.CircularDustEffect(Player.Center, DustID.CoralTorch, infectRadius, 8);
                GlobalPet.CircularDustEffect(Player.Center, DustID.Granite, slowRadius, 30, scale: 0.8f);
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && Player.Distance(npc.Center) < infectRadius)
                    {
                        npc.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), infectDuration);
                    }

                    if (npc.active && npc.TryGetGlobalNPC(out AstrophageInfection astro))
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
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (i == npc.whoAmI)
                        continue;
                    NPC target = Main.npc[i];
                    if (target.active && npc.Distance(target.Center) < deathSpreadRange && npc.Distance(target.Center) < closestRange)
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
    public sealed class AstrophageItemTooltip : GlobalItem
    {

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == CalamityPetIDs.Astrophage;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }

            AstrophageEffect astro = Main.LocalPlayer.GetModPlayer<AstrophageEffect>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.Astrophage")
                .Replace("<class>", PetTextsColors.ClassText(astro.PetClassPrimary, astro.PetClassSecondary))
                .Replace("<infectRadius>", Math.Round(astro.infectRadius / 16f, 2).ToString())
                .Replace("<infectDuration>", Math.Round(astro.infectDuration / 60f, 2).ToString())
                .Replace("<slowRadius>", Math.Round(astro.slowRadius / 16f, 2).ToString())
                .Replace("<slowAmount>", Math.Round(astro.slowAmount * 100, 2).ToString())
                .Replace("<spreadRadius>", Math.Round(AstrophageInfection.deathSpreadRange / 16f, 2).ToString())
                .Replace("<spreadSlow>", Math.Round(astro.infectionHeavySlow * 100, 2).ToString())
                .Replace("<spreadSlowDuration>", Math.Round(AstrophageInfection.deathSpreadSlowDur / 60f, 2).ToString())
            ));
        }
    }
}
