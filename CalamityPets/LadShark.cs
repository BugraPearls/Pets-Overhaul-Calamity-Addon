using CalamityMod;
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
    public sealed class LadSharkEffect : PetEffect
    {
        public override int PetItemID => CalamityPetIDs.LadShark;
        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override PetClasses PetClassSecondary => PetClasses.Supportive;
        public float selfDmg = 0.85f;
        public float enemyDmg = 0.8f;
        public int grantRegen = 3;
        public int regenDuration = 180;
        public int regenCd = 1200;

        public int radius = 320;

        public int loveRecoveryTimer = -1;
        public int currentRegen = 0;
        public override void PreUpdate()
        {
            if (PetIsEquipped(false))
            {
                Pet.SetPetAbilityTimer(regenCd);
            }

            if (loveRecoveryTimer >= 0)
            {
                if (loveRecoveryTimer % 30 == 0)
                {
                    Pet.PetRecovery(0, 0, flatIncrease: currentRegen, isLifesteal: false);
                }
                loveRecoveryTimer--;
            }
        }
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (PetIsEquipped())
            {
                damage *= selfDmg;
            }
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (PetIsEquipped())
            {
                modifiers.FinalDamage *= enemyDmg;
            }
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.AbilityPressCheck() && PetIsEquipped())
            {
                if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                    SoundEngine.PlaySound(SoundID.DD2_DarkMageCastHeal with { PitchVariance = 0.4f }, Player.Center);

                GlobalPet.CircularDustEffect(Player.Center, DustID.HealingPlus, radius, 30, scale: 2f);

                foreach (var npc in Main.ActiveNPCs)
                {
                    if (npc.Distance(Player.Center) < radius && npc.canGhostHeal && npc.immortal == false && npc.TryGetGlobalNPC(out LoveRecoveryNPCs lad))
                    {
                        lad.recoveryValue = grantRegen;
                        lad.timer += regenDuration;
                    }
                }
                foreach (var player in Main.ActivePlayers)
                {
                    if ((player.whoAmI == Main.myPlayer) || (player.Distance(Player.Center) < radius))
                    {
                        player.GetModPlayer<LadSharkEffect>().loveRecoveryTimer += regenDuration;
                        player.GetModPlayer<LadSharkEffect>().currentRegen = grantRegen * 2;
                    }
                }

                Pet.timer = Pet.timerMax;
            }
        }
    }
    public sealed class LoveRecoveryNPCs : GlobalNPC
    {
        public int timer = -1;
        public int recoveryValue = 0;
        public override bool InstancePerEntity => true;
        public override bool PreAI(NPC npc)
        {
            if (timer >= 0)
            {
                if (timer % 30 == 0)
                {
                    int recoveryVal = recoveryValue * (npc.IsAnEnemy() ? 1 : 2);
                    if (npc.life < npc.lifeMax && npc.life + recoveryVal > npc.lifeMax)
                    {
                        recoveryVal = npc.lifeMax - npc.life;
                    }
                    if (npc.life == npc.lifeMax)
                    {
                        return base.PreAI(npc);
                    }
                    npc.life += recoveryVal;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        npc.HealEffect(recoveryVal);
                    }
                    npc.netUpdate = true;
                }
                timer--;
            }
            return base.PreAI(npc);
        }
    }
    public sealed class JoyfulHeartTooltip : PetTooltip
    {
        public override PetEffect PetsEffect => shark;
        public static LadSharkEffect shark
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out LadSharkEffect pet))
                    return pet;
                else
                    return ModContent.GetInstance<LadSharkEffect>();
            }
        }
        public override string PetsTooltip => Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.JoyfulHeart")
                .Replace("<selfDmg>", shark.selfDmg.ToString())
                .Replace("<takenDmg>", shark.enemyDmg.ToString())
                .Replace("<keybind>", PetTextsColors.KeybindText(PetKeybinds.UsePetAbility).ToString())
                .Replace("<radius>", Math.Round(shark.radius / 16f, 2).ToString())
                .Replace("<perSecRegen>", shark.grantRegen.ToString())
                .Replace("<duration>", Math.Round(shark.regenDuration / 60f, 2).ToString());
    }
}
