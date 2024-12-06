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
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
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
            if (levi.PetIsEquipped())
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
        public override string PetsTooltip => Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.Levi")
                .Replace("<dmgPer>", Math.Round(levi.dmgPerFish * 100, 2).ToString())
                .Replace("<defPer>", levi.oneDefPerFishPower.ToString())
                .Replace("<helm>", levi.helm.ToString())
                .Replace("<chest>", levi.chest.ToString())
                .Replace("<leg>", levi.leg.ToString())
                .Replace("<critChance>", levi.crit.ToString());
    }
}
