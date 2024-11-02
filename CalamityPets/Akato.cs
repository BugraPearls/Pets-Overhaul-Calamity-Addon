using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using PetsOverhaul.Config;
using PetsOverhaul.NPCs;
using PetsOverhaul.Projectiles;
using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Items;
using PetsOverhaulCalamityAddon.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class AkatoEffect : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public long dragonPracticeStacks = 0;
        public const long maxStacks = 1000000000000; //1 trillion, 1.000.000.000.000
        public const int bossMult = 10;
        public const int stackForKill = 3;
        public const int stackPerHit = 1;
        public int cooldown = 600;

        private int beginOnHit = 0;
        public int onHitExpiration = 180;

        public int burnAmount = 300; //This burn is multiplied by 2 inside the SuperScorchingFlame code, so this is per second damage.
        public int burnDuration = 180;
        public float explosionMult = 0.7f;
        public float executeTreshold = 0.065f;
        public int explosionSize = 240;

        public int stackBurnBonusMult = 40;
        public int StackBurnBonus
        {
            get
            {
                if (dragonPracticeStacks == 1)
                {
                    return (int)(stackBurnBonusMult * 0.1f);
                }
                if (dragonPracticeStacks < 1)
                {
                    return 0;
                }
                return (int)(Math.Log10(dragonPracticeStacks) * stackBurnBonusMult);
            }
        }

        public int stackBurnTimeMult = 15;
        public int StackBurnTime
        {
            get
            {
                if (dragonPracticeStacks == 1)
                {
                    return (int)(stackBurnTimeMult * 0.1f);
                }
                if (dragonPracticeStacks < 1)
                {
                    return 0;
                }
                return (int)(Math.Log10(dragonPracticeStacks) * stackBurnTimeMult);
            }
        }

        public float stackExplosionBonusMult = 0.1f;
        public float StackExplosionBonus
        {
            get
            {
                if (dragonPracticeStacks == 1)
                {
                    return stackExplosionBonusMult * 0.1f;
                }
                if (dragonPracticeStacks < 1)
                {
                    return 0;
                }
                return (float)Math.Log10(dragonPracticeStacks) * stackExplosionBonusMult;
            }
        }

        public float stackExecuteBonusMult = 0.01f;
        public float StackExecuteBonus
        {
            get
            {
                if (dragonPracticeStacks == 1)
                {
                    return stackExecuteBonusMult * 0.1f;
                }
                if (dragonPracticeStacks < 1)
                {
                    return 0;
                }
                return (float)Math.Log10(dragonPracticeStacks) * stackExecuteBonusMult;
            }
        }

        public int stackExplosionSizeMult = 16;
        public int StackExplosionSize
        {
            get
            {
                if (dragonPracticeStacks == 1)
                {
                    return (int)(stackExplosionSizeMult * 0.1f);
                }
                if (dragonPracticeStacks < 1)
                {
                    return 0;
                }
                return (int)(Math.Log10(dragonPracticeStacks) * stackExplosionSizeMult);
            }
        }

        public void AddStack(NPC npc)
        {
            long stack;
            if (npc.active == false)
            {
                stack = stackForKill;
                if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                {
                    SoundEngine.PlaySound(new SoundStyle("PetsOverhaulCalamityAddon/Sounds/Akato/AkatoExecute") with { PitchVariance = 1f, Identifier = "akatoExecute", MaxInstances = 4, SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest, Type = SoundType.Sound, Volume = 0.7f }, npc.Center); //See disclaimer.txt in Sounds/Akato folder for further info regarding sounds origin
                }
            }
            else
            {
                stack = stackPerHit;
            }
            if (npc.boss || NpcPet.NonBossTrueBosses.Contains(npc.type))
            {
                stack *= bossMult;
            }
            else if (npc.rarity > 0)
            {
                stack *= 1 + npc.rarity;
            }
            dragonPracticeStacks += stack;
        }
        public override void SaveData(TagCompound tag)
        {
            tag.Add("dragonPractice", dragonPracticeStacks);
        }
        public override void LoadData(TagCompound tag)
        {
            if (tag.TryGet("dragonPractice", out long stack))
            {
                dragonPracticeStacks = stack;
            }
        }
        public override void PreUpdate()
        {
            if (dragonPracticeStacks > maxStacks)
            {
                dragonPracticeStacks = maxStacks;
            }
            if (Pet.PetInUse(CalamityPetIDs.Akato))
            {
                Pet.SetPetAbilityTimer(cooldown);
                beginOnHit--;
                if (beginOnHit < 0)
                    beginOnHit = 0;

            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUseWithSwapCd(CalamityPetIDs.Akato) && beginOnHit > 0)
            {
                Projectile petProjectile = Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), target.Center, Vector2.Zero, ModContent.ProjectileType<PetExplosion>(), Pet.PetDamage(damageDone * (explosionMult + StackExplosionBonus)), 0, Player.whoAmI, explosionSize + StackExplosionSize);
                petProjectile.DamageType = hit.DamageType;
                petProjectile.CritChance = (int)Player.GetTotalCritChance(hit.DamageType);
                if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                {
                    string path = Main.rand.Next(3) switch
                    {
                        0 => "PetsOverhaulCalamityAddon/Sounds/Akato/AkatoExplosion0",
                        1 => "PetsOverhaulCalamityAddon/Sounds/Akato/AkatoExplosion1",
                        2 => "PetsOverhaulCalamityAddon/Sounds/Akato/AkatoExplosion2",
                        _ => "PetsOverhaulCalamityAddon/Sounds/Akato/AkatoExplosion0",
                    };
                    SoundEngine.PlaySound(new SoundStyle(path) with { PitchVariance = 0.6f, Identifier = "akatoExplosion", MaxInstances = 2, SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest, Type = SoundType.Sound, Volume = 0.8f }, target.Center); //See disclaimer.txt in Sounds/Akato folder for further info regarding sounds origin
                }
                beginOnHit = 0;
                if (GlobalPet.LifestealCheck(target))
                {
                    AddStack(target);
                }
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUseWithSwapCd(CalamityPetIDs.Akato) && GlobalPet.LifestealCheck(target) && proj.TryGetGlobalProjectile(out ProjectileSourceChecks petProj) && petProj.petProj)
            {
                AddStack(target);
                if (target.active && target.TryGetGlobalNPC(out SuperScorcherBreath scorch))
                {
                    scorch.Burns.Add(new SuperScorcherBreath.Smoldering(Player.whoAmI, burnDuration + StackBurnTime));
                }
            }
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.AbilityPressCheck() && Pet.PetInUseWithSwapCd(CalamityPetIDs.Akato))
            {
                Pet.timer = Pet.timerMax;
                beginOnHit = onHitExpiration;
            }
        }
    }
    public class SuperScorcherBreath : GlobalNPC
    {
        public struct Smoldering(int playerWhoAmI, int duration)
        {
            public int BurnDuration = duration;
            public readonly AkatoEffect AkatoOfBurner => Main.player[playerWhoAmI].GetModPlayer<AkatoEffect>();
            public readonly int BurnAmount => AkatoOfBurner.burnAmount + AkatoOfBurner.StackBurnBonus; //Both burn damage and execute treshold SHOULD update like this while already in effect.
            public readonly float ExecuteTreshold => AkatoOfBurner.executeTreshold + AkatoOfBurner.StackExecuteBonus;
        }
        public List<Smoldering> Burns = new();
        public override bool InstancePerEntity => true;
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (Burns.Count > 0)
            {
                foreach (Smoldering smolder in Burns)
                {
                    npc.lifeRegen -= smolder.BurnAmount * 2;
                    damage = npc.lifeMax / 100;
                }
                Smoldering highestExecute = Burns.MaxBy(x => x.ExecuteTreshold);
                if (npc.life <= npc.lifeMax * highestExecute.ExecuteTreshold)
                {
                    GlobalPet.RemoveOldestCombatText();

                    CombatText.NewText(npc.getRect(), Color.GhostWhite, (int)(npc.lifeMax * highestExecute.ExecuteTreshold), true);

                    npc.StrikeInstantKill();
                    highestExecute.AkatoOfBurner.AddStack(npc);
                }
            }
        }
        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (Burns.Count > 0 && Main.rand.NextBool(5))
            {
                Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<HolyFireDust>());
            }
        }
        public override bool PreAI(NPC npc)
        {
            if (Burns.Count > 0)
            {
                for (int i = 0; i < Burns.Count; i++)
                {
                    Smoldering value = Burns[i];
                    value.BurnDuration--;
                    Burns[i] = value;
                }

                int num = Burns.FindIndex(x => x.BurnDuration <= 0);
                if (num != -1)
                {
                    Burns.RemoveAt(num);
                }
            }
            return base.PreAI(npc);
        }
    }
    public sealed class ForgottenDragonEggTooltip : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == CalamityPetIDs.Akato;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }

            AkatoEffect akato = Main.LocalPlayer.GetModPlayer<AkatoEffect>();

            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.ForgottenDragonEgg")
                        .Replace("<class>", PetTextsColors.ClassText(akato.PetClassPrimary, akato.PetClassSecondary))
                        .Replace("<stackIcon>", ModContent.ItemType<DragonPracticeIcon>().ToString())
                        .Replace("<keybind>", PetTextsColors.KeybindText(PetKeybinds.UsePetAbility))
                        .Replace("<cooldown>", Math.Round(akato.cooldown / 60f, 2).ToString())
                        .Replace("<expire>", Math.Round(akato.onHitExpiration / 60f, 2).ToString())
                        .Replace("<baseSize>", Math.Round(akato.explosionSize / 16f, 2).ToString())
                        .Replace("<stackSize>", Math.Round(akato.StackExplosionSize / 16f, 2).ToString())
                        .Replace("<baseMult>", Math.Round(akato.explosionMult * 100, 2).ToString())
                        .Replace("<stackMult>", Math.Round(akato.StackExplosionBonus * 100, 4).ToString())
                        .Replace("<burnDuration>", Math.Round(akato.burnDuration / 60f, 2).ToString())
                        .Replace("<stackBurnDuration>", Math.Round(akato.StackBurnTime / 60f, 2).ToString())
                        .Replace("<baseBurn>", akato.burnAmount.ToString())
                        .Replace("<stackBurn>", akato.StackBurnBonus.ToString())
                        .Replace("<baseExecute>", Math.Round(akato.executeTreshold * 100, 2).ToString())
                        .Replace("<stackExecute>", Math.Round(akato.StackExecuteBonus * 100, 4).ToString())
                        .Replace("<dragonPractice>", akato.dragonPracticeStacks.ToString())
                        .Replace("<hitStack>", AkatoEffect.stackPerHit.ToString())
                        .Replace("<killStack>", AkatoEffect.stackForKill.ToString())
                        .Replace("<bossMultiply>", AkatoEffect.bossMult.ToString())
                    ));
        }
    }
}
