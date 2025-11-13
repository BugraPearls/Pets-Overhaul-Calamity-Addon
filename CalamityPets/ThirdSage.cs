using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class ThirdSageEffect : PetEffect
    {
        public override int PetItemID => CalamityPetIDs.ThirdSage;
        public float damageMult = 0.9f;
        public int flatHealing = 23;
        public float percHealing = 0.08f;
        public int cooldown = 960;
        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override int PetAbilityCooldown => cooldown;
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped(false) && Pet.timer > 0)
            {
                Player.GetDamage<GenericDamageClass>() *= damageMult;
            }
        }
        public override void ExtraProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.AbilityPressCheck() && PetIsEquipped())
            {
                if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                    SoundEngine.PlaySound(SoundID.Item2 with { Pitch = -0.5f, PitchVariance = 0.4f }, Player.Center);
                Pet.PetRecovery(Player.statLifeMax2, percHealing, flatHealing, isLifesteal: false);
                Pet.timer = Pet.timerMax;
            }
        }
    }
    public sealed class HermitsBoxofOneHundredMedicinesTooltip : PetTooltip
    {
        public override PetEffect PetsEffect => sage;
        public static ThirdSageEffect sage
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out ThirdSageEffect pet))
                    return pet;
                else
                    return ModContent.GetInstance<ThirdSageEffect>();
            }
        }
        public override string PetsTooltip => Compatibility.LocVal("PetTooltips.HermitsBoxofOneHundredMedicines")
                    .Replace("<keybind>", PetUtils.KeybindText(PetKeybinds.UsePetAbility))
                    .Replace("<flatHeal>", sage.flatHealing.ToString())
                    .Replace("<percHeal>", Math.Round(sage.percHealing * 100, 2).ToString())
                    .Replace("<cooldown>", Math.Round(sage.cooldown / 60f, 2).ToString())
                    .Replace("<damageMult>", Math.Round(sage.damageMult, 2).ToString());
        public override string SimpleTooltip => Compatibility.LocVal("SimpleTooltips.HermitsBoxofOneHundredMedicines").Replace("<keybind>", PetUtils.KeybindText(PetKeybinds.UsePetAbility));
    }
}
