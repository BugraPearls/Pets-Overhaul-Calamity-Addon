using CalamityMod.NPCs.Yharon;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Buffs;
using PetsOverhaulCalamityAddon.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class PineappleEffect : PetEffect
    {
        public int defense = 5;
        public float crit = 5;
        public float damage = 0.125f;
        public float summonerKb = 1.25f;
        public float moveSpd = 0.125f;
        public float miningSpeed = 0.2f;
        public override PetClasses PetClassPrimary => PetClasses.Utility;
        public override void PostUpdateMiscEffects()
        {
            void WellFedAmp(int Defense, float Crit, float Damage, float SummonerKb, float MoveSpd, float MiningSpeed)
            {
                Player.wellFed = true;
                Player.statDefense += Defense;
                Player.GetCritChance<GenericDamageClass>() += Crit;
                Player.GetDamage<GenericDamageClass>() += Damage;
                Player.GetKnockback<SummonDamageClass>() += SummonerKb;
                Player.moveSpeed += MoveSpd;
                Player.pickSpeed -= MiningSpeed; //Mining speed needs to be negative, so it reduces mining time.
            }

            if (Player.HasBuff(ModContent.BuffType<TheGrandNourishment>()))
            {
                WellFedAmp(defense, crit, damage, summonerKb, moveSpd, miningSpeed);
            }
            if (Pet.PetInUseWithSwapCd(CalamityPetIDs.Pineapple)) //Half of all Well Fed buffs (Calamity Modified values)
            {
                if (Player.HasBuff(ModContent.BuffType<TheGrandNourishment>()))
                {
                    WellFedAmp((int)Math.Ceiling(defense / 2f), crit / 2, damage / 2, summonerKb / 2, moveSpd / 2, miningSpeed / 2);
                }
                else if (Player.HasBuff(BuffID.WellFed3))
                {
                    WellFedAmp(2, 2f, 0.05f, 0.5f, 0.05f, 0.075f);
                }
                else if (Player.HasBuff(BuffID.WellFed2))
                {
                    WellFedAmp(2, 1.5f, 0.0375f, 0.375f, 0.0375f, 0.05f);
                }
                else if (Player.HasBuff(BuffID.WellFed))
                {
                    WellFedAmp(1, 1f, 0.025f, 0.25f, 0.025f, 0.025f);
                }
            }
        }
        public override void Load()
        {
            On_Player.AddBuff += ChangeWellFed;
        }

        static private void ChangeWellFed(On_Player.orig_AddBuff orig, Player self, int type, int timeToAdd, bool quiet, bool foodHack)
        {
            int buffType = type;
            int buffTime = timeToAdd;
            PineappleEffect pineapple = self.GetModPlayer<PineappleEffect>();
            if (pineapple.Pet.PetInUseWithSwapCd(CalamityPetIDs.Pineapple) && BuffID.Sets.IsWellFed[type])
            {
                switch (type)
                {
                    case BuffID.WellFed:
                        buffType = BuffID.WellFed2;
                        buffTime /= 2;
                        break;
                    case BuffID.WellFed2:
                        buffType = BuffID.WellFed3;
                        buffTime /= 2;
                        break;
                    case BuffID.WellFed3:
                        buffType = ModContent.BuffType<TheGrandNourishment>();
                        buffTime /= 2;
                        break;
                    default:
                        break;
                }
            }
            if (buffType == ModContent.BuffType<TheGrandNourishment>())
            {
                self.ClearBuff(BuffID.WellFed);
                self.ClearBuff(BuffID.WellFed2);
                self.ClearBuff(BuffID.WellFed3);
            }
            else if ((buffType == BuffID.WellFed || buffType == BuffID.WellFed2 || buffType == BuffID.WellFed3) && self.HasBuff(ModContent.BuffType<TheGrandNourishment>())) 
            {
                return;
            }
            orig(self, buffType, buffTime, quiet, foodHack);
        }
    }
    public sealed class PineappleItemTooltip : GlobalItem
    {

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == CalamityPetIDs.Pineapple;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().EnableTooltipToggle && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }
            PineappleEffect pineapple = Main.LocalPlayer.GetModPlayer<PineappleEffect>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.PineapplePet")
                .Replace("<class>", PetTextsColors.ClassText(pineapple.PetClassPrimary, pineapple.PetClassSecondary))
                .Replace("<def>", pineapple.defense.ToString())
                .Replace("<crit>", Math.Round(pineapple.crit, 2).ToString())
                .Replace("<dmg>", Math.Round(pineapple.damage* 100, 2).ToString())
                .Replace("<sumKb>", Math.Round(pineapple.summonerKb, 2).ToString())
                .Replace("<moveSpd>", Math.Round(pineapple.moveSpd * 100, 2).ToString())
                .Replace("<miningSpd>", Math.Round(pineapple.miningSpeed * 100, 2).ToString())
            ));
        }
    }
}
