using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class ThirdSageEffect : PetEffect
    {
        public float damageMult = 0.9f;
        public int flatHealing = 23;
        public float percHealing = 0.08f;
        public int cooldown = 960;
        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override void PreUpdate()
        {
            if (Pet.PetInUse(CalamityPetIDs.ThirdSage))
            {
                Pet.SetPetAbilityTimer(cooldown);
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (Pet.PetInUse(CalamityPetIDs.ThirdSage) && Pet.timer > 0)
            {
                Player.GetDamage<GenericDamageClass>() *= damageMult;
            }
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.AbilityPressCheck() && Pet.PetInUseWithSwapCd(CalamityPetIDs.ThirdSage))
            {
                if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                    SoundEngine.PlaySound(SoundID.Item2 with { Pitch = -0.5f, PitchVariance = 0.4f }, Player.Center);
                Pet.PetRecovery(Player.statLifeMax2, percHealing, flatHealing, isLifesteal: false);
                Pet.timer = Pet.timerMax;
            }
        }
    }
    public sealed class HermitsBoxofOneHundredMedicinesTooltip : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == CalamityPetIDs.ThirdSage;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }
            ThirdSageEffect sage = Main.LocalPlayer.GetModPlayer<ThirdSageEffect>();

            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.HermitsBoxofOneHundredMedicines")
                    .Replace("<keybind>", PetTextsColors.KeybindText(PetKeybinds.UsePetAbility))
                    .Replace("<class>", PetTextsColors.ClassText(sage.PetClassPrimary, sage.PetClassSecondary))
                    .Replace("<flatHeal>", sage.flatHealing.ToString())
                    .Replace("<percHeal>", Math.Round(sage.percHealing * 100, 2).ToString())
                    .Replace("<cooldown>", Math.Round(sage.cooldown / 60f, 2).ToString())
                    .Replace("<damageMult>", Math.Round(sage.damageMult, 2).ToString())
                ));
        }
    }
}
