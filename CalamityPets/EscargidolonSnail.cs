using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class EscargidolonSnailEffect : PetEffect
    {
        public override int PetItemID => CalamityPetIDs.EscargidolonSnail;
        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override PetClasses PetClassSecondary => PetClasses.Offensive;
        public override int PetStackCurrent => Player.aggro;
        public override int PetStackMax => 0;
        public override string PetStackText => Compatibility.LocVal("PetTooltips.AbyssShellFossilStack");
        public bool CurrentTooltip = true;
        public int CurrentDef => Player.aggro / aggroToDef;
        public int aggroToDef = 100;
        public int CurrentHp => Player.aggro / aggroToHp;
        public int aggroToHp = 15;
        public float CurrentDr => Player.aggro / aggroToDr;
        public float aggroToDr = 150;
        public float CurrentNegativeMs => Player.aggro / aggroToNegativeMs * -1;
        public float aggroToNegativeMs = 350;

        public float CurrentDmg => Player.aggro / aggroToDmg * -1;
        public float aggroToDmg = 100;
        public float CurrentCrit => Player.aggro / aggroToCrit * -1;
        public float aggroToCrit = 175;
        public float CurrentPen => Player.aggro / aggroToPen * -1;
        public float aggroToPen = 75;
        public float CurrentMs => Player.aggro / aggroToMs * -1;
        public float aggroToMs = 250;
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                if (Player.aggro >= 0)
                {
                    Player.statDefense += CurrentDef;
                    Player.statLifeMax2 += CurrentHp;
                    Player.endurance += CurrentDr / 100;
                    Player.moveSpeed += CurrentNegativeMs / 100;
                }
                else
                {
                    Player.GetDamage<GenericDamageClass>() += CurrentDmg / 100;
                    Player.GetCritChance<GenericDamageClass>() += CurrentCrit;
                    Player.GetArmorPenetration<GenericDamageClass>() += CurrentPen;
                    Player.moveSpeed += CurrentMs / 100;
                }
            }
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (PetKeybinds.PetAbilitySwitch.JustPressed)
            {
                CurrentTooltip = !CurrentTooltip;
            }
        }
    }
    public sealed class AbyssShellFossilTooltip : PetTooltip
    {
        public override PetEffect PetsEffect => snail;
        public static EscargidolonSnailEffect snail
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out EscargidolonSnailEffect pet))
                    return pet;
                else
                    return ModContent.GetInstance<EscargidolonSnailEffect>();
            }
        }
        public override string PetsTooltip
        {
            get
            {
                string Tooltip;
                switch (snail.CurrentTooltip)
                {
                    case true:
                        Tooltip = Compatibility.LocVal("PetTooltips.SnailPositiveAggro")
                            .Replace("<active>", snail.Player.aggro >= 0 ? Compatibility.LocVal("PetTooltips.SnailActive") : Compatibility.LocVal("PetTooltips.SnailInactive"))
                            .Replace("<aggroDef>", snail.aggroToDef.ToString())
                            .Replace("<aggroHealth>", snail.aggroToHp.ToString())
                            .Replace("<aggroDr>", snail.aggroToDr.ToString())
                            .Replace("<aggroMsLower>", snail.aggroToNegativeMs.ToString())
                            .Replace("<def>", snail.CurrentDef.ToString())
                            .Replace("<hp>", snail.CurrentHp.ToString())
                            .Replace("<dr>", Math.Round(snail.CurrentDr, 2).ToString())
                            .Replace("<msLower>", Math.Round(snail.CurrentNegativeMs, 2).ToString());
                        break;
                    case false:
                        Tooltip = Compatibility.LocVal("PetTooltips.SnailNegativeAggro")
                            .Replace("<active>", snail.Player.aggro < 0 ? Compatibility.LocVal("PetTooltips.SnailActive") : Compatibility.LocVal("PetTooltips.SnailInactive"))
                            .Replace("<aggroDamage>", snail.aggroToDmg.ToString())
                            .Replace("<aggroCrit>", snail.aggroToCrit.ToString())
                            .Replace("<aggroPen>", snail.aggroToPen.ToString())
                            .Replace("<aggroMs>", snail.aggroToMs.ToString())
                            .Replace("<dmg>", Math.Round(snail.CurrentDmg, 2).ToString())
                            .Replace("<crit>", Math.Round(snail.CurrentCrit, 2).ToString())
                            .Replace("<pen>", Math.Round(snail.CurrentPen, 2).ToString())
                            .Replace("<ms>", Math.Round(snail.CurrentMs, 2).ToString());
                        break;
                    default:
                }
                return Compatibility.LocVal("PetTooltips.AbyssShellFossil")
                    .Replace("<switchKeybind>", PetTextsColors.KeybindText(PetKeybinds.PetAbilitySwitch))
                    .Replace("<tooltip>", Tooltip);
            }
        }
        public override string SimpleTooltip => Compatibility.LocVal("SimpleTooltips.AbyssShellFossil");
    }
}
