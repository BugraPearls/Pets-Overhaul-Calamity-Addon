using PetsOverhaulCalamityAddon.Systems;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.GameInput;
using Microsoft.Xna.Framework;
using CalamityMod;
using Terraria.Audio;
using System;
using Terraria.ID;
using CalamityMod.Buffs.DamageOverTime;
using PetsOverhaul.NPCs;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class PlaguebringerBabEffect : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Rogue;
        public int timeToAdd = 420;
        public float mainTargetMult = 0.25f;
        public float surroundingMult = 0.15f;
        public int surroundRadius = 96;
        public int detonateRadius = 1600;
        public int cooldown = 510;
        public int plagueAndSlowDuration = 150;
        public float slowAmount = 0.25f;
        public override void PreUpdate()
        {
            if (Pet.PetInUse(CalamityPetIDs.PlagueBringerBab))
            {
                Pet.SetPetAbilityTimer(cooldown);
            }
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.AbilityPressCheck() && Pet.PetInUseWithSwapCd(CalamityPetIDs.PlagueBringerBab))
            {
                if (ModContent.GetInstance<Personalization>().AbilitySoundEnabled)
                {
                    if (Main.rand.NextBool())
                        SoundEngine.PlaySound(new SoundStyle("CalamityMod/Sounds/Custom/PlagueSounds/PBGAttackSwitch1") with { PitchVariance = 0.4f }, Player.Center);
                    else
                        SoundEngine.PlaySound(new SoundStyle("CalamityMod/Sounds/Custom/PlagueSounds/PBGAttackSwitch2") with { PitchVariance = 0.4f }, Player.Center);
                }
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && Player.Distance(npc.Center) < detonateRadius && npc.TryGetGlobalNPC(out PlaguebringerBabStacks boom) && boom.stacks > 0)
                    {
                        npc.AddBuff(ModContent.BuffType<Plague>(), plagueAndSlowDuration);
                        NpcPet.AddSlow(new NpcPet.PetSlow(slowAmount, plagueAndSlowDuration, CalSlows.PlagueSlow),npc);
                        NPC.HitInfo hit = new NPC.HitInfo() with { Damage = boom.stacks, DamageType = DamageClass.Generic };
                        npc.StrikeNPC(hit);

                        if (Main.netMode != NetmodeID.SinglePlayer)
                            NetMessage.SendStrikeNPC(npc, hit);

                        boom.stacks = 0;
                        boom.timer = 0;
                    }
                }
                Pet.timer = Pet.timerMax;
            }

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUseWithSwapCd(CalamityPetIDs.PlagueBringerBab) && hit.DamageType is RogueDamageClass && target.TryGetGlobalNPC(out PlaguebringerBabStacks victim))
            {
                if (target.active)
                {
                    victim.timer = timeToAdd;
                    victim.stacks += GlobalPet.Randomizer((int)(hit.Damage * mainTargetMult * 100));
                }
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (i == target.whoAmI)
                        continue;

                    NPC npc = Main.npc[i];
                    if (npc.active && target.Distance(npc.Center) < surroundRadius && npc.TryGetGlobalNPC(out PlaguebringerBabStacks surrounder))
                    {
                        surrounder.timer = timeToAdd;
                        surrounder.stacks += GlobalPet.Randomizer((int)(hit.Damage * surroundingMult * 100));
                    }
                }
            }
        }
    }
    public sealed class PlaguebringerBabStacks : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public int stacks = 0;
        public int timer = 0;
        public override bool PreAI(NPC npc)
        {
            timer--;
            if (timer < 0)
            {
                timer = 0;
                stacks = 0;
            }
            return base.PreAI(npc);
        }
        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (stacks > 0 && timer > 0 && Main.rand.NextBool(30))
            {
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.JungleSpore);
            }
        }
        public override void OnKill(NPC npc)
        {
            timer = 0;
            stacks = 0;
        }
    }
    public sealed class PlagueCallerTooltip : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == CalamityPetIDs.PlagueBringerBab;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().EnableTooltipToggle && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }

            PlaguebringerBabEffect plague = Main.LocalPlayer.GetModPlayer<PlaguebringerBabEffect>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.PlagueCaller")
                .Replace("<class>", PetTextsColors.ClassText(plague.PetClassPrimary, plague.PetClassSecondary))
                .Replace("<primaryPerc>", Math.Round(plague.mainTargetMult*100,2).ToString())
                .Replace("<hitAoE>", Math.Round(plague.surroundRadius / 16f, 2).ToString())
                .Replace("<surroundingPerc>", Math.Round(plague.surroundingMult * 100, 2).ToString())
                .Replace("<stackDuration>", Math.Round(plague.timeToAdd / 60f, 2).ToString())
                .Replace("<keybind>", PetTextsColors.KeybindText(Keybinds.UsePetAbility))
                .Replace("<detonateRadius>", Math.Round(plague.detonateRadius / 16f, 2).ToString())
                .Replace("<cooldown>", Math.Round(plague.cooldown / 60f, 2).ToString())
                .Replace("<slow>", Math.Round(plague.slowAmount * 100, 2).ToString())
                .Replace("<plagueDuration>", Math.Round(plague.cooldown / 60f, 2).ToString())
            ));
        }
    }
}
