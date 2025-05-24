using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class LeviEffect : PetEffect
    {
        public override int PetItemID => CalamityPetIDs.Levi;
        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public override PetClasses PetClassSecondary => PetClasses.Defensive;
        public float dmgPerFish = 0.005f;
        public float oneDefPerFishPower = 2.5f;
        public float crit = 5f;
        public int helm = 9;
        public int chest = 13;
        public int leg = 9;
        public int actFishPow => Player.GetFishingConditions().FinalFishingLevel;
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                Player.GetDamage<GenericDamageClass>() += actFishPow * dmgPerFish;
                Player.statDefense += (int)(actFishPow / oneDefPerFishPower);
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
            if (levi.PetIsEquipped())
            {
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
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            LeviEffect levi = Main.LocalPlayer.GetModPlayer<LeviEffect>();
            if (levi.PetIsEquipped())
            {
                int def = item.defense;
                if (item.type == ItemID.AnglerHat)
                {
                    def = levi.helm + item.defense;
                }
                if (item.type == ItemID.AnglerVest)
                {
                    def = levi.chest + item.defense;
                }
                if (item.type == ItemID.AnglerPants)
                {
                    def = levi.leg + item.defense;
                }

                int indx = tooltips.FindLastIndex(x => x.Name == "Defense");

                if (indx < 0) //SOMEHOW if Defense line doesn't exist.
                {
                    indx = tooltips.FindLastIndex(x => x.Name == "ItemName") + 1;
                    tooltips.Insert(indx, new(Mod, "PetTooltip0", def.ToString() + Language.GetTextValue("LegacyTooltip.25")));
                    tooltips.Insert(indx + 1, new(Mod, "PetTooltip1", levi.crit.ToString() + Compatibility.LocVal("PetTooltips.LeviCrit")));
                }
                else
                {
                    tooltips[indx].Text = def.ToString() + Language.GetTextValue("LegacyTooltip.25");
                    tooltips.Insert(indx + 1, new(Mod, "PetTooltip0", levi.crit.ToString() + Compatibility.LocVal("PetTooltips.LeviCrit")));
                }
            }
        }
    }
    public sealed class LeviItemTooltip : PetTooltip
    {
        public override PetEffect PetsEffect => levi;
        public static LeviEffect levi
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out LeviEffect pet))
                    return pet;
                else
                    return ModContent.GetInstance<LeviEffect>();
            }
        }
        public override string PetsTooltip => Compatibility.LocVal("PetTooltips.Levi")
                .Replace("<dmgPer>", Math.Round(levi.dmgPerFish * 100, 2).ToString())
                .Replace("<currentDmg>", Math.Round(levi.dmgPerFish * levi.actFishPow * 100, 2).ToString())
                .Replace("<defPer>", levi.oneDefPerFishPower.ToString())
                .Replace("<currentDef>", Math.Round(levi.actFishPow / levi.oneDefPerFishPower, 2).ToString())
                .Replace("<helm>", levi.helm.ToString())
                .Replace("<chest>", levi.chest.ToString())
                .Replace("<leg>", levi.leg.ToString())
                .Replace("<critChance>", levi.crit.ToString());
        public override string SimpleTooltip => Compatibility.LocVal("SimpleTooltips.Levi");
    }
}
