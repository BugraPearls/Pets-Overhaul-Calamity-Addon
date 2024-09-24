using PetsOverhaulCalamityAddon.Systems;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class FoxEffect : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Utility;
        public int baseTime = 120;
        public int perTime = 30;
        public int cooldown = 7200;
        private int cleansePenalty = 0;
        public override void PreUpdate()
        {
            if (Pet.PetInUse(CalamityPetIDs.Fox))
            {
                Pet.SetPetAbilityTimer(cooldown);
            }
            if (cleansePenalty > 0)
            {
                cleansePenalty--;
                Player.buffImmune[BuffID.Cursed] = false;
                Player.AddBuff(BuffID.Cursed, 1);
            }
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.AbilityPressCheck() && Pet.PetInUseWithSwapCd(CalamityPetIDs.Fox))
            {
                int amountOfDebuffs = 0;
                for (int i = 0; i < Player.MaxBuffs; i++)
                {
                    if (Main.debuff[Player.buffType[i]])
                    {
                        Player.DelBuff(i);
                        amountOfDebuffs++;
                    }
                }
                if (amountOfDebuffs > 0)
                {
                    if (ModContent.GetInstance<Personalization>().AbilitySoundDisabled == false)
                        SoundEngine.PlaySound(SoundID.Item35 with { Pitch = 0.75f, PitchVariance = 0.1f }, Player.position);
                    cleansePenalty = baseTime + perTime * (amountOfDebuffs - 1);
                    CombatText.NewText(Player.getRect(), Color.Orange, Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.FoxCleansedText") + amountOfDebuffs.ToString(),dramatic: true);
                    Pet.timer = Pet.timerMax;
                }
            }
        }
    }
    public sealed class FoxDriveTooltip : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == CalamityPetIDs.Fox;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().DisableTooltipToggle == false && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            FoxEffect fox = Main.LocalPlayer.GetModPlayer<FoxEffect>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.FoxDrive")
                .Replace("<class>", PetTextsColors.ClassText(fox.PetClassPrimary, fox.PetClassSecondary))
                .Replace("<keybind>", PetTextsColors.KeybindText(Keybinds.UsePetAbility))
                .Replace("<baseCurse>", Math.Round(fox.baseTime / 60f, 2).ToString())
                .Replace("<perCurse>", Math.Round(fox.perTime / 60f, 2).ToString())
                .Replace("<cooldown>", Math.Round(fox.cooldown / 60f, 2).ToString())
            ));
        }
    }
}
