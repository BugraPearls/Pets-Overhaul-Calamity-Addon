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
using CalamityMod;
using System;
using PetsOverhaul.NPCs;
using PetsOverhaulCalamityAddon.Buffs;
using System.Drawing;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class LadSharkEffect : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override PetClasses PetClassSecondary => PetClasses.Supportive;
        public float selfDmg = 0.85f;
        public float enemyDmg = 0.78f;
        public int grantRegen = 3;
        public int regenDuration = 180;
        public int regenCd = 600;

        public int radius = 320;

        public int loveRecoveryTimer = -1;
        public int currentRegen = 0;
        public override void PreUpdate()
        {
            if (Pet.PetInUse(CalamityPetIDs.LadShark))
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
            if (Pet.PetInUseWithSwapCd(CalamityPetIDs.LadShark))
            {
                damage *= selfDmg;
            }
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (Pet.PetInUseWithSwapCd(CalamityPetIDs.LadShark))
            {
                modifiers.FinalDamage *= enemyDmg;
            }
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.AbilityPressCheck() && Pet.PetInUseWithSwapCd(CalamityPetIDs.LadShark))
            {
                if (ModContent.GetInstance<Personalization>().AbilitySoundEnabled)
                    SoundEngine.PlaySound(SoundID.DD2_DarkMageCastHeal with { PitchVariance = 0.4f }, Player.Center);

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.Distance(Player.Center) < radius && npc.canGhostHeal && npc.type != NPCID.TargetDummy)
                    {
                        if (npc.TryGetGlobalNPC(out LoveRecoveryNPCs lad))
                        {
                            lad.recoveryValue = grantRegen;
                            lad.timer += regenDuration;
                        }
                    }
                }
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player player = Main.player[i];
                    if (player.active && ((player.whoAmI != 255 && player.whoAmI == Main.myPlayer) || (player.Distance(Player.Center) < radius)))
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
    public sealed class JoyfulHeartTooltip : GlobalItem
    {

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == CalamityPetIDs.LadShark;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().EnableTooltipToggle && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            LadSharkEffect shark = Main.LocalPlayer.GetModPlayer<LadSharkEffect>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.JoyfulHeart")
                .Replace("<class>", PetTextsColors.ClassText(shark.PetClassPrimary, shark.PetClassSecondary))
                .Replace("<selfDmg>", shark.selfDmg.ToString())
                .Replace("<takenDmg>", shark.enemyDmg.ToString())
                .Replace("<keybind>", PetTextsColors.KeybindText(Keybinds.UsePetAbility).ToString())
                .Replace("<radius>", Math.Round(shark.radius / 16f, 2).ToString())
                .Replace("<perSecRegen>", shark.grantRegen.ToString())
                .Replace("<duration>", Math.Round(shark.regenDuration / 60f, 2).ToString())
            ));
        }
    }
}
