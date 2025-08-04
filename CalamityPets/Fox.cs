using CalamityMod;
using Microsoft.Xna.Framework;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class FoxEffect : PetEffect
    {
        public override int PetItemID => CalamityPetIDs.Fox;
        public override PetClasses PetClassPrimary => PetClasses.Utility;
        public int baseTime = 30;
        public int perTime = 12;
        public int cooldown = 900;
        private int cleansePenalty = 0;
        public override int PetAbilityCooldown => cooldown;
        public override void ExtraPreUpdateNoCheck()
        {
            if (cleansePenalty > 0)
            {
                cleansePenalty--;
                Player.buffImmune[BuffID.Cursed] = false;
                Player.AddBuff(BuffID.Cursed, 1);
            }
        }
        public override void ExtraProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.AbilityPressCheck() && PetIsEquipped())
            {
                List<int> idsToRemove = new(Player.MaxBuffs);
                for (int i = 0; i < Player.MaxBuffs; i++)
                {
                    int buffId = Player.buffType[i];
                    if (Main.debuff[buffId] && BuffID.Sets.NurseCannotRemoveDebuff[buffId] == false)
                    {
                        idsToRemove.Add(buffId);
                    }
                }
                if (idsToRemove.Count > 0)
                {
                    if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                        SoundEngine.PlaySound(SoundID.Item35 with { Pitch = 0.75f, PitchVariance = 0.1f }, Player.Center);
                    cleansePenalty = baseTime + perTime * (idsToRemove.Count - 1);
                    CombatText.NewText(Player.getRect(), Color.Orange, Compatibility.LocVal("PetTooltips.FoxCleansedText") + idsToRemove.Count.ToString(), dramatic: true);
                    foreach (var item in idsToRemove)
                    {
                        Player.ClearBuff(item);
                    }
                    Pet.timer = Pet.timerMax;
                }
            }
        }
    }
    public sealed class FoxDriveTooltip : PetTooltip
    {
        public override PetEffect PetsEffect => fox;
        public static FoxEffect fox
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out FoxEffect pet))
                    return pet;
                else
                    return ModContent.GetInstance<FoxEffect>();
            }
        }
        public override string PetsTooltip => Compatibility.LocVal("PetTooltips.FoxDrive")
                .Replace("<keybind>", PetTextsColors.KeybindText(PetKeybinds.UsePetAbility))
                .Replace("<baseCurse>", Math.Round(fox.baseTime / 60f, 2).ToString())
                .Replace("<perCurse>", Math.Round(fox.perTime / 60f, 2).ToString())
                .Replace("<cooldown>", Math.Round(fox.cooldown / 60f, 2).ToString());
        public override string SimpleTooltip => Compatibility.LocVal("SimpleTooltips.FoxDrive").Replace("<keybind>", PetTextsColors.KeybindText(PetKeybinds.UsePetAbility));
    }
}
