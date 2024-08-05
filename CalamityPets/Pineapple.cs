using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Buffs;
using PetsOverhaulCalamityAddon.Systems;
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
        public override PetClasses PetClassPrimary => PetClasses.None;
        public override void PostUpdateEquips()
        {
            if (Player.HasBuff(ModContent.BuffType<TheGrandNourishment>()))
            {
                Player.statDefense += defense;
                Player.GetCritChance<GenericDamageClass>() += crit;
                Player.GetDamage<GenericDamageClass>() += damage;
                Player.GetKnockback<SummonDamageClass>() += summonerKb;
                Player.moveSpeed += moveSpd;
                Player.pickSpeed += miningSpeed;
            }
            if (Pet.PetInUseWithSwapCd(CalamityIDs.Pineapple))
            {
                
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
            if (pineapple.Pet.PetInUseWithSwapCd(CalamityIDs.Pineapple) && BuffID.Sets.IsWellFed[type])
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
            orig(self,buffType,buffTime,quiet, foodHack);
        }
    }
    public sealed class PineappleItemTooltip : GlobalItem
    {

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == CalamityIDs.Pineapple;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().DisableTooltipToggle == false && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            PineappleEffect pineapple = Main.LocalPlayer.GetModPlayer<PineappleEffect>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.PineapplePet")
                .Replace("<class>", PetColors.ClassText(pineapple.PetClassPrimary, pineapple.PetClassSecondary))
            ));
        }
    }
}
