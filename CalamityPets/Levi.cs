using PetsOverhaulCalamityAddon.Systems;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;
using System;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class LeviEffect : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public override PetClasses PetClassSecondary => PetClasses.Defensive;
        public float dmgPerFish = 0.005f;
        public float oneDefPerFishPower = 2.5f;
        public float crit = 5f;
        public int helm = 9;
        public int chest = 13;
        public int leg = 9;
        public override void PostUpdateMiscEffects()
        {
            if (Pet.PetInUseWithSwapCd(CalamityPetIDs.Levi))
            {
                Player.GetDamage<GenericDamageClass>() += Player.fishingSkill * dmgPerFish;
                Player.statDefense += (int)(Player.fishingSkill / oneDefPerFishPower);
            }
        }
    }
    public sealed class LeviAnglerArmor : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.AnglerHat || entity.type == ItemID.AnglerVest || entity.type == ItemID.AnglerPants;
        }
        public override void UpdateEquip(Item item, Player player)
        {
            LeviEffect levi = player.GetModPlayer<LeviEffect>();
            if (item.type == ItemID.AnglerHat)
            {
                player.statDefense += levi.helm;
            }
            if (item.type == ItemID.AnglerVest)
            {
                player.statDefense += levi.chest;
            }
            if (item.type == ItemID.AnglerPants)
            {
                player.statDefense += levi.leg;
            }
            player.GetCritChance<GenericDamageClass>() += levi.crit;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            LeviEffect levi = Main.LocalPlayer.GetModPlayer<LeviEffect>();
            if (levi.Pet.PetInUseWithSwapCd(CalamityPetIDs.Levi))
            {
                int def = item.defense;
                if (item.type == ItemID.AnglerHat)
                {
                    def = levi.helm;
                }
                if (item.type == ItemID.AnglerVest)
                {
                    def = levi.chest;
                }
                if (item.type == ItemID.AnglerPants)
                {
                    def = levi.leg;
                }
                if (tooltips.Find(x => x.Name == "Defense") != null)
                    tooltips.Find(x => x.Name == "Defense").Text = def.ToString() + Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.LeviDef");

                if (tooltips.Find(x => x.Name == "Tooltip0") != null)
                tooltips.Find(x => x.Name == "Tooltip0").Text = levi.crit.ToString() + Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.LeviCrit");
            }
        }
    }
    public sealed class LeviItemTooltip : GlobalItem
    {

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == CalamityPetIDs.Levi;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().EnableTooltipToggle && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            LeviEffect levi = Main.LocalPlayer.GetModPlayer<LeviEffect>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.Levi")
                .Replace("<class>", PetTextsColors.ClassText(levi.PetClassPrimary, levi.PetClassSecondary))
                .Replace("<dmgPer>", Math.Round(levi.dmgPerFish *100,2).ToString())
                .Replace("<defPer>", levi.oneDefPerFishPower.ToString())
                .Replace("<helm>", levi.helm.ToString())
                .Replace("<chest>", levi.chest.ToString())
                .Replace("<leg>", levi.leg.ToString())
                .Replace("<critChance>", levi.crit.ToString())
            ));
        }
    }
}
