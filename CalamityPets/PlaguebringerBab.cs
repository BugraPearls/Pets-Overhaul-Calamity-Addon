using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using PetsOverhaul.Config;
using PetsOverhaul.NPCs;
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
    public sealed class PlaguebringerBabEffect : PetEffect
    {
        public override int PetItemID => CalamityPetIDs.PlagueBringerBab;
        public override PetClasses PetClassPrimary => PetClasses.Rogue;
        public int timeToAdd = 420;
        public float mainTargetMult = 0.25f;
        public float surroundingMult = 0.15f;
        public int surroundRadius = 96;
        public int detonateRadius = 1200;
        public int cooldown = 510;
        public int plagueAndSlowDuration = 150;
        public float slowAmount = 0.25f;
        private bool hitThisFrame = false;
        public int CurrentCanDetonate { get 
            {
                int current = 0;
                foreach (var npc in Main.ActiveNPCs)
                {
                    if (npc.dontTakeDamage == false && Player.Distance(npc.Center) < detonateRadius && npc.TryGetGlobalNPC(out PlaguebringerBabStacks boom) && boom.stacks > 0)
                    {
                        current += boom.stacks;
                    }
                }
                return current;
            }
        }
        public override int PetAbilityCooldown => cooldown;
        public override string PetStackText => Compatibility.LocVal("PetTooltips.PlagueCallerStack");
        public override int PetStackMax => 0;
        public override int PetStackCurrent => CurrentCanDetonate;
        public override void ExtraProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.AbilityPressCheck() && PetIsEquipped())
            {
                if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                {
                    if (Main.rand.NextBool())
                        SoundEngine.PlaySound(new SoundStyle("CalamityMod/Sounds/Custom/PlagueSounds/PBGAttackSwitch1") with { PitchVariance = 0.4f }, Player.Center);
                    else
                        SoundEngine.PlaySound(new SoundStyle("CalamityMod/Sounds/Custom/PlagueSounds/PBGAttackSwitch2") with { PitchVariance = 0.4f }, Player.Center);
                }
                foreach (var npc in Main.ActiveNPCs)
                {
                    if (npc.dontTakeDamage == false && Player.Distance(npc.Center) < detonateRadius && npc.TryGetGlobalNPC(out PlaguebringerBabStacks boom) && boom.stacks > 0)
                    {
                        npc.AddBuff(ModContent.BuffType<Plague>(), plagueAndSlowDuration);
                        PetGlobalNPC.AddSlow(new PetSlow(slowAmount, plagueAndSlowDuration, CalSlows.PlagueSlow), npc);
                        npc.SimpleStrikeNPC(Pet.PetDamage(boom.stacks, DamageClass.Throwing), npc.direction, Main.rand.NextBool((int)Math.Min(Player.GetTotalCritChance<RogueDamageClass>(), 100), 100), 0, DamageClass.Throwing, true, Player.luck);
                        boom.stacks = 0;
                        boom.timer = 0;
                    }
                }
                PetUtils.CircularDustEffect(Player.Center, DustID.JungleTorch, detonateRadius, 200, scale: 2f);
                Pet.timer = Pet.timerMax;
            }

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PetIsEquipped() && hit.DamageType is RogueDamageClass && target.TryGetGlobalNPC(out PlaguebringerBabStacks victim) && hitThisFrame == false)
            {
                hitThisFrame = true;
                PetUtils.CircularDustEffect(target.Center, DustID.JungleTorch, surroundRadius, 12);
                if (target.active)
                {
                    victim.timer = timeToAdd;
                    victim.stacks += Math.Max(PetUtils.Randomizer((int)(damageDone * mainTargetMult * 100)), 1);
                }
                foreach (var npc in Main.ActiveNPCs)
                {
                    if (npc == target && npc.dontTakeDamage == true)
                        continue;

                    if (target.Distance(npc.Center) < surroundRadius && npc.TryGetGlobalNPC(out PlaguebringerBabStacks surrounder))
                    {
                        surrounder.timer = timeToAdd;
                        surrounder.stacks += Math.Max(PetUtils.Randomizer((int)(damageDone * surroundingMult * 100)), 1);
                    }
                }
            }
        }
        public override void ExtraPreUpdate()
        {
            hitThisFrame = false;
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
    public sealed class PlagueCallerTooltip : PetTooltip
    {
        public override PetEffect PetsEffect => plague;
        public static PlaguebringerBabEffect plague
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out PlaguebringerBabEffect pet))
                    return pet;
                else
                    return ModContent.GetInstance<PlaguebringerBabEffect>();
            }
        }
        public override string PetsTooltip => Compatibility.LocVal("PetTooltips.PlagueCaller")
                .Replace("<primaryPerc>", Math.Round(plague.mainTargetMult * 100, 2).ToString())
                .Replace("<hitAoE>", Math.Round(plague.surroundRadius / 16f, 2).ToString())
                .Replace("<surroundingPerc>", Math.Round(plague.surroundingMult * 100, 2).ToString())
                .Replace("<stackDuration>", Math.Round(plague.timeToAdd / 60f, 2).ToString())
                .Replace("<keybind>", PetUtils.KeybindText(PetKeybinds.UsePetAbility))
                .Replace("<detonateRadius>", Math.Round(plague.detonateRadius / 16f, 2).ToString())
                .Replace("<cooldown>", Math.Round(plague.cooldown / 60f, 2).ToString())
                .Replace("<slow>", Math.Round(plague.slowAmount * 100, 2).ToString())
                .Replace("<plagueDuration>", Math.Round(plague.plagueAndSlowDuration / 60f, 2).ToString());
        public override string SimpleTooltip => Compatibility.LocVal("SimpleTooltips.PlagueCaller").Replace("<keybind>", PetUtils.KeybindText(PetKeybinds.UsePetAbility));
    }
}
