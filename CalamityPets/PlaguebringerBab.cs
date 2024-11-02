using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using PetsOverhaul.Config;
using PetsOverhaul.NPCs;
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
    public sealed class PlaguebringerBabEffect : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Rogue;
        public int timeToAdd = 420;
        public float mainTargetMult = 0.25f;
        public float surroundingMult = 0.15f;
        public int surroundRadius = 96;
        public int detonateRadius = 1200;
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
                if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
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
                        NpcPet.AddSlow(new NpcPet.PetSlow(slowAmount, plagueAndSlowDuration, CalSlows.PlagueSlow), npc);
                        npc.SimpleStrikeNPC(Pet.PetDamage(boom.stacks), npc.direction, Main.rand.NextBool((int)Math.Min(Player.GetTotalCritChance<RogueDamageClass>(), 100), 100), 0, DamageClass.Throwing, true, Player.luck);
                        boom.stacks = 0;
                        boom.timer = 0;
                    }
                }
                GlobalPet.CircularDustEffect(Player.Center, DustID.JungleTorch, detonateRadius, 200, scale: 2f);
                Pet.timer = Pet.timerMax;
            }

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUseWithSwapCd(CalamityPetIDs.PlagueBringerBab) && hit.DamageType is RogueDamageClass && target.TryGetGlobalNPC(out PlaguebringerBabStacks victim))
            {
                GlobalPet.CircularDustEffect(target.Center, DustID.JungleTorch, surroundRadius, 12);
                if (target.active)
                {
                    victim.timer = timeToAdd;
                    victim.stacks += Math.Max(GlobalPet.Randomizer((int)(hit.SourceDamage * mainTargetMult * 100)), 1);
                }
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (i == target.whoAmI)
                        continue;

                    NPC npc = Main.npc[i];
                    if (npc.active && target.Distance(npc.Center) < surroundRadius && npc.TryGetGlobalNPC(out PlaguebringerBabStacks surrounder))
                    {
                        surrounder.timer = timeToAdd;
                        surrounder.stacks += Math.Max(GlobalPet.Randomizer((int)(hit.SourceDamage * surroundingMult * 100)), 1);
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
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.JungleTorch);
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
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }

            PlaguebringerBabEffect plague = Main.LocalPlayer.GetModPlayer<PlaguebringerBabEffect>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.PlagueCaller")
                .Replace("<class>", PetTextsColors.ClassText(plague.PetClassPrimary, plague.PetClassSecondary))
                .Replace("<primaryPerc>", Math.Round(plague.mainTargetMult * 100, 2).ToString())
                .Replace("<hitAoE>", Math.Round(plague.surroundRadius / 16f, 2).ToString())
                .Replace("<surroundingPerc>", Math.Round(plague.surroundingMult * 100, 2).ToString())
                .Replace("<stackDuration>", Math.Round(plague.timeToAdd / 60f, 2).ToString())
                .Replace("<keybind>", PetTextsColors.KeybindText(PetKeybinds.UsePetAbility))
                .Replace("<detonateRadius>", Math.Round(plague.detonateRadius / 16f, 2).ToString())
                .Replace("<cooldown>", Math.Round(plague.cooldown / 60f, 2).ToString())
                .Replace("<slow>", Math.Round(plague.slowAmount * 100, 2).ToString())
                .Replace("<plagueDuration>", Math.Round(plague.plagueAndSlowDuration / 60f, 2).ToString())
            ));
        }
    }
}
