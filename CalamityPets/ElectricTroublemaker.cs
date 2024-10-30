using CalamityMod;
using CalamityMod.CalPlayer;
using Microsoft.Xna.Framework;
using PetsOverhaul.Config;
using PetsOverhaul.NPCs;
using PetsOverhaul.Projectiles;
using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Projectiles;
using PetsOverhaulCalamityAddon.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class ElectricTroublemakerEffect : PetEffect
    {
        public int currentMove = 0;
        public const float dexMult = 1.2f;
        public const float STABMult = 1.5f;
        public const float superEffective = 2f;
        public const float notVeryEffective = 0.5f;
        public bool chargeNextAttack = false;
        public int baseCooldown = 6000;
        private int internalCooldownToInitiateAttack = 0; //This to remove accidental initiations/cancelings.

        public const int thunderWave = 0;
        public float tWaveSlow = 0.5f;
        public int tWaveDuration = 360;
        public int tWaveRadius = 56;
        public float tWaveCooldown = 0.25f;

        public const int hydroPump = 1;
        public int hydroPumpDmg = 50;
        public int wetDuration = 1200;
        public float hydroPumpCooldown = 0.2f;

        public const int overheat = 2;
        public int overheatDmg = 25;
        public int burnDuration = 270;
        public int overheatRadius = 100;
        public float overheatCooldown = 0.35f;

        public const int blizzard = 3;
        public int blizzardDmg = 9;
        public int blizzardRadius = 160;
        public int blizzardDuration = 210;
        public float coldSlow = 0.1f;
        public int freezeDuration = 20;
        public int freezeRequirement = 180;
        public float blizzardCooldown = 0.8f;

        public const int leafStorm = 4;
        public int leafStormDmg = 15;
        public int minimumLeaf = 10;
        public int maxLeaf = 15;
        public float leafStormCooldown = 0.45f;

        public const int airSlash = 5;
        public int airSlashDmg = 15;
        public float airSlashKb = 12f;
        public int airSlashRadius = 80;
        public float airSlashCooldown = 0.3f;
        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public override PetClasses PetClassSecondary => PetClasses.Utility;
        public float GetTypeEffectiveness(NPC npc, int moveId)
        {
            float baseVal = 1f;
            bool electric = false;
            bool water = false;
            bool fire = false;
            bool ice = false;
            bool grass = false;
            bool flying = false;
            if (Player.ZoneBeach || Player.InSunkenSea() || Player.InSulphur() || Player.InAbyss())
                water = true;
            else if (Player.ZoneTowerSolar || Player.ZoneDesert || Player.ZoneUndergroundDesert || Player.ZoneUnderworldHeight || Player.InCalamity())
                fire = true;
            else if (Player.ZoneSnow || Main.snowMoon)
                ice = true;
            else if (Player.ZoneJungle)
                grass = true;
            else if (Player.ZoneSkyHeight || Player.ZoneMeteor || Player.InAstral())
                flying = true;
            else
                electric = true;
            switch (moveId)
            {
                case thunderWave:
                    if (electric)
                        baseVal *= STABMult;
                    if (npc.Calamity().VulnerableToElectricity.HasValue)
                    {
                        if (npc.Calamity().VulnerableToElectricity.Value)
                            baseVal *= superEffective;
                        else
                            baseVal *= notVeryEffective;
                    }
                    break;
                case hydroPump:
                    if (water)
                        baseVal *= STABMult;
                    if (npc.Calamity().VulnerableToWater.HasValue)
                    {
                        if (npc.Calamity().VulnerableToWater.Value)
                            baseVal *= superEffective;
                        else
                            baseVal *= notVeryEffective;
                    }
                    break;
                case overheat:
                    if (fire)
                        baseVal *= STABMult;
                    if (npc.Calamity().VulnerableToHeat.HasValue)
                    {
                        if (npc.Calamity().VulnerableToHeat.Value)
                            baseVal *= superEffective;
                        else
                            baseVal *= notVeryEffective;
                    }
                    break;
                case blizzard:
                    if (ice)
                        baseVal *= STABMult;
                    if (npc.Calamity().VulnerableToCold.HasValue)
                    {
                        if (npc.Calamity().VulnerableToCold.Value)
                            baseVal *= superEffective;
                        else
                            baseVal *= notVeryEffective;
                    }
                    break;
                case leafStorm:
                    if (grass)
                        baseVal *= STABMult;
                    if (npc.Calamity().VulnerableToSickness.HasValue)
                    {
                        if (npc.Calamity().VulnerableToSickness.Value)
                            baseVal *= superEffective;
                        else
                            baseVal *= notVeryEffective;
                    }
                    break;
                case airSlash:
                    if (flying)
                        baseVal *= STABMult;
                    if (npc.noGravity) //If enemy flies
                    {
                        baseVal *= notVeryEffective;
                    }
                    else if (npc.velocity.Y == 0) //If enemy doesn't fly, and also grounded
                    {
                        baseVal *= superEffective;
                    }
                    break;
                default:
                    break;
            }
            if (CalamityPlayer.areThereAnyDamnBosses)
            {
                baseVal *= dexMult;
            }
            return baseVal;
        }
        public override void SaveData(TagCompound tag)
        {
            tag.Add("CurrentMove", currentMove);
        }
        public override void LoadData(TagCompound tag)
        {
            if (tag.TryGet("CurrentMove", out int move))
            {
                currentMove = move;
            }
        }
        public override void PreUpdate()
        {
            if (Pet.PetInUse(CalamityPetIDs.ElectricTroublemaker))
            {
                Pet.SetPetAbilityTimer(baseCooldown);
            }

            internalCooldownToInitiateAttack--;
            if (internalCooldownToInitiateAttack < 0)
                internalCooldownToInitiateAttack = 0;
        }
        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (chargeNextAttack && Pet.PetInUse(CalamityPetIDs.ElectricTroublemaker) && Main.rand.NextBool(15) && drawInfo.shadow == 0f)
            {
                switch (currentMove)
                {
                    case thunderWave:
                        if (Pet.timer + Pet.timerMax * tWaveCooldown < Pet.timerMax)
                        {
                            int dust1 = Dust.NewDust(Player.position, Player.width, Player.height, DustID.Electric);
                            Main.dust[dust1].noGravity = true;
                            drawInfo.DustCache.Add(dust1);
                        }
                        break;
                    case hydroPump:
                        if (Pet.timer + Pet.timerMax * hydroPumpCooldown < Pet.timerMax)
                        {
                            int dust2 = Dust.NewDust(Player.position, Player.width, Player.height, DustID.Water, Scale: 1.5f);
                            Main.dust[dust2].noGravity = true;
                            drawInfo.DustCache.Add(dust2);
                        }
                        break;
                    case overheat:
                        if (Pet.timer + Pet.timerMax * overheatCooldown < Pet.timerMax)
                        {
                            int dust3 = Dust.NewDust(Player.position, Player.width, Player.height, DustID.CrimsonTorch, Scale: 2f);
                            Main.dust[dust3].noGravity = true;
                            drawInfo.DustCache.Add(dust3);
                        }
                        break;
                    case blizzard:
                        if (Pet.timer + Pet.timerMax * blizzardCooldown < Pet.timerMax)
                        {
                            int dust4 = Dust.NewDustPerfect(new Vector2(Main.rand.NextFloat(Player.width) + Player.position.X, Player.position.Y - 5), DustID.Snow, new Vector2(0.4f, 4.5f)).dustIndex;
                            Main.dust[dust4].noGravity = true;
                            drawInfo.DustCache.Add(dust4);
                        }
                        break;
                    case leafStorm:
                        if (Pet.timer + Pet.timerMax * leafStormCooldown < Pet.timerMax)
                        {
                            int dust5 = Dust.NewDust(Player.position, Player.width, Player.height, DustID.Grass);
                            Main.dust[dust5].noGravity = true;
                            drawInfo.DustCache.Add(dust5);
                        }
                        break;
                    case airSlash:
                        if (Pet.timer + Pet.timerMax * airSlashCooldown < Pet.timerMax)
                        {
                            int dust6 = Dust.NewDust(Player.position, Player.width, Player.height, DustID.Cloud, SpeedY: -6, newColor: Color.White);
                            Main.dust[dust6].noGravity = true;
                            drawInfo.DustCache.Add(dust6);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (PetKeybinds.PetAbilitySwitch.JustPressed && Pet.PetInUse(CalamityPetIDs.ElectricTroublemaker))
            {
                currentMove++;
                if (currentMove >= 6)
                {
                    currentMove = 0;
                }
            }
            if (Player.dead == false && PetKeybinds.UsePetAbility.JustPressed && Pet.PetInUseWithSwapCd(CalamityPetIDs.ElectricTroublemaker) && internalCooldownToInitiateAttack <= 0)
            {
                chargeNextAttack = !chargeNextAttack;
                internalCooldownToInitiateAttack = 10;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Pet.PetInUseWithSwapCd(CalamityPetIDs.ElectricTroublemaker) && chargeNextAttack)
            {
                switch (currentMove)
                {
                    case thunderWave:
                        if (Pet.timer + Pet.timerMax * tWaveCooldown < Pet.timerMax)
                        {
                            foreach (var npc in Main.ActiveNPCs)
                            {
                                if (target.Distance(npc.Center) < tWaveRadius)
                                {
                                    NpcPet.AddSlow(new NpcPet.PetSlow(tWaveSlow * GetTypeEffectiveness(npc, thunderWave), tWaveDuration, CalSlows.rotomThunderWave), npc);
                                }
                            }
                            GlobalPet.CircularDustEffect(target.Center, DustID.Electric, tWaveRadius, 10);
                            if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                                SoundEngine.PlaySound(new SoundStyle("PetsOverhaulCalamityAddon/Sounds/ElectricTroublemaker/ThunderWave") with { PitchVariance = 0.6f }, target.Center);
                            Pet.timer += (int)(Pet.timerMax * tWaveCooldown);
                        }
                        break;
                    case hydroPump:
                        if (target.active && Pet.timer + Pet.timerMax * hydroPumpCooldown < Pet.timerMax)
                        {
                            target.SimpleStrikeNPC((int)(hydroPumpDmg * GetTypeEffectiveness(target, hydroPump)), hit.HitDirection, Main.rand.NextBool((int)Math.Min(Player.GetTotalCritChance(hit.DamageType), 100), 100),0,hit.DamageType,true,Player.luck);
                            for (int i = 0; i < 10; i++)
                            {
                                Dust.NewDustDirect(target.position, target.width, target.height, DustID.Water, hit.HitDirection * Main.rand.NextFloat(7f, 12f), Main.rand.NextFloat(0, 1.5f), 0, Scale: 3f).noGravity = true;
                            }
                            target.AddBuff(BuffID.Wet, (int)(wetDuration * GetTypeEffectiveness(target, hydroPump)));
                            if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                                SoundEngine.PlaySound(new SoundStyle("PetsOverhaulCalamityAddon/Sounds/ElectricTroublemaker/HydroPump") with { PitchVariance = 0.8f }, target.Center);
                            Pet.timer += (int)(Pet.timerMax * hydroPumpCooldown);
                        }
                        break;
                    case overheat:
                        if (Pet.timer + Pet.timerMax * overheatCooldown < Pet.timerMax)
                        {
                            Projectile petProjectile = Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), target.Center, Vector2.Zero, ModContent.ProjectileType<PetExplosion>(), (int)(overheatDmg * GetTypeEffectiveness(target, overheat)), 0, Player.whoAmI, overheatRadius);
                            petProjectile.DamageType = hit.DamageType;
                            petProjectile.CritChance = (int)Player.GetTotalCritChance(hit.DamageType);
                            target.AddBuff(BuffID.OnFire, (int)(burnDuration * GetTypeEffectiveness(target, overheat)));
                            for (int i = 0; i < 15; i++)
                            {
                                Dust.NewDustPerfect(target.Center + Main.rand.NextVector2Circular(overheatRadius, overheatRadius), DustID.SolarFlare);
                            }

                            Projectile petProj = Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), Player.Center, Vector2.Zero, ModContent.ProjectileType<PetExplosion>(), (int)(overheatDmg * GetTypeEffectiveness(target, overheat)), 0, Player.whoAmI, overheatRadius);
                            petProj.DamageType = hit.DamageType;
                            petProj.CritChance = (int)Player.GetTotalCritChance(hit.DamageType);
                            for (int i = 0; i < 10; i++)
                            {
                                Dust.NewDustPerfect(Player.Center + Main.rand.NextVector2Circular(overheatRadius, overheatRadius), DustID.SolarFlare);
                            }

                            if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                                SoundEngine.PlaySound(new SoundStyle("PetsOverhaulCalamityAddon/Sounds/ElectricTroublemaker/Overheat") with { PitchVariance = 0.2f }, target.Center);
                            Pet.timer += (int)(Pet.timerMax * overheatCooldown);
                        }
                        break;
                    case blizzard:
                        if (Pet.timer + Pet.timerMax * leafStormCooldown < Pet.timerMax)
                        {
                            Projectile petProjectile = Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), target.Center, Vector2.Zero, ModContent.ProjectileType<RotomBlizzard>(), blizzardDmg, 0, Player.whoAmI, blizzardRadius, blizzardDuration); //does its type effectiveness in Projectile code
                            petProjectile.DamageType = hit.DamageType;
                            petProjectile.CritChance = (int)Player.GetTotalCritChance(hit.DamageType);
                            if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                                SoundEngine.PlaySound(new SoundStyle("PetsOverhaulCalamityAddon/Sounds/ElectricTroublemaker/Blizzard") with { PitchVariance = 0.8f }, target.Center);
                            Pet.timer += (int)(Pet.timerMax * leafStormCooldown);
                        }
                        break;
                    case leafStorm:
                        if (Pet.timer + Pet.timerMax * leafStormCooldown < Pet.timerMax)
                        {
                            for (int i = 0; i < Main.rand.Next(minimumLeaf, maxLeaf); i++)
                            {
                                Projectile petProjectile = Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), target.Center + Main.rand.NextVector2CircularEdge(target.width, target.height), Main.rand.NextVector2CircularEdge(10, 10), ProjectileID.Leaf, (int)(leafStormDmg * GetTypeEffectiveness(target, leafStorm)), 0, Player.whoAmI);
                                petProjectile.DamageType = hit.DamageType;
                                petProjectile.CritChance = (int)Player.GetTotalCritChance(hit.DamageType);
                            }

                            if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                                SoundEngine.PlaySound(new SoundStyle("PetsOverhaulCalamityAddon/Sounds/ElectricTroublemaker/LeafStorm") with { Pitch = -0.4f, PitchVariance = 0.5f }, target.Center);
                            Pet.timer += (int)(Pet.timerMax * leafStormCooldown);
                        }
                        break;
                    case airSlash:
                        if (Pet.timer + Pet.timerMax * airSlashCooldown < Pet.timerMax)
                        {
                            foreach (var npc in Main.ActiveNPCs)
                            {
                                if (target.Distance(npc.Center) < airSlashRadius)
                                {
                                    npc.SimpleStrikeNPC((int)(airSlashDmg * GetTypeEffectiveness(npc, airSlash)), hit.HitDirection, Main.rand.NextBool((int)Math.Min(Player.GetTotalCritChance(hit.DamageType), 100), 100), airSlashKb * GetTypeEffectiveness(npc, airSlash),hit.DamageType,true,Player.luck);
                                }
                            }
                            for (int i = 0; i < 15; i++)
                            {
                                Dust.NewDustPerfect(target.Center + Main.rand.NextVector2Circular(airSlashRadius / 2.5f, airSlashRadius / 2.5f), DustID.Cloud, new Vector2(hit.HitDirection * 6, -3)).noGravity = true;
                            }
                            GlobalPet.CircularDustEffect(target.Center, DustID.Snow, airSlashRadius, 10);
                            if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                                SoundEngine.PlaySound(new SoundStyle("PetsOverhaulCalamityAddon/Sounds/ElectricTroublemaker/AirSlash") with { PitchVariance = 0.5f }, target.Center);
                            Pet.timer += (int)(Pet.timerMax * airSlashCooldown);
                        }
                        break;
                    default:
                        break;
                }
                chargeNextAttack = false;
            }
        }
    }
    public sealed class RotomBlizzardFreeze : GlobalNPC
    {
        public int FreezeVal = 0;
        public int cooldownToResetFreeze = 0;
        public override bool InstancePerEntity => true;
        public override void PostAI(NPC npc)
        {
            cooldownToResetFreeze--;
            if (cooldownToResetFreeze <= 0)
            {
                FreezeVal = 0;
                cooldownToResetFreeze = 0;
            }
        }
    }
    public sealed class TheEtomerTooltip : GlobalItem
    {

        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == CalamityPetIDs.ElectricTroublemaker;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }
            ElectricTroublemakerEffect rotom = Main.LocalPlayer.GetModPlayer<ElectricTroublemakerEffect>();
            string MoveTooltip = "Invalid Ability";
            switch (rotom.currentMove)
            {
                case ElectricTroublemakerEffect.thunderWave:
                    MoveTooltip = Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.RotomThunderWave")
                        .Replace("<radius>", Math.Round(rotom.tWaveRadius / 16f, 2).ToString())
                        .Replace("<slow>", Math.Round(rotom.tWaveSlow * 100, 2).ToString())
                        .Replace("<slowDuration>", Math.Round(rotom.tWaveDuration / 60f, 2).ToString())
                        .Replace("<percentOfCd>", Math.Round(rotom.tWaveCooldown * 100, 2).ToString())
                        .Replace("<percentToSeconds>", Math.Round(rotom.tWaveCooldown * rotom.Pet.timerMax / 60, 2).ToString());
                    break;
                case ElectricTroublemakerEffect.hydroPump:
                    MoveTooltip = Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.RotomHydroPump")
                        .Replace("<dmg>", rotom.hydroPumpDmg.ToString())
                        .Replace("<wetDuration>", Math.Round(rotom.wetDuration / 60f, 2).ToString())
                        .Replace("<percentOfCd>", Math.Round(rotom.hydroPumpCooldown * 100, 2).ToString())
                        .Replace("<percentToSeconds>", Math.Round(rotom.hydroPumpCooldown * rotom.Pet.timerMax / 60, 2).ToString());
                    break;
                case ElectricTroublemakerEffect.overheat:
                    MoveTooltip = Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.RotomOverheat")
                        .Replace("<radius>", Math.Round(rotom.overheatRadius / 16f, 2).ToString())
                        .Replace("<dmg>", rotom.overheatDmg.ToString())
                        .Replace("<burnDuration>", Math.Round(rotom.burnDuration / 60f, 2).ToString())
                        .Replace("<percentOfCd>", Math.Round(rotom.overheatCooldown * 100, 2).ToString())
                        .Replace("<percentToSeconds>", Math.Round(rotom.overheatCooldown * rotom.Pet.timerMax / 60, 2).ToString());
                    break;
                case ElectricTroublemakerEffect.blizzard:
                    MoveTooltip = Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.RotomBlizzard")
                        .Replace("<dmg>", rotom.blizzardDmg.ToString())
                        .Replace("<blizzardDuration>", Math.Round(rotom.blizzardDuration / 60f, 2).ToString())
                        .Replace("<slow>", Math.Round(rotom.coldSlow * 100, 2).ToString())
                        .Replace("<freezeRequirement>", Math.Round(rotom.freezeRequirement / 60f, 2).ToString())
                        .Replace("<freezeDuration>", Math.Round(rotom.freezeDuration / 60f, 2).ToString())
                        .Replace("<percentOfCd>", Math.Round(rotom.blizzardCooldown * 100, 2).ToString())
                        .Replace("<percentToSeconds>", Math.Round(rotom.blizzardCooldown * rotom.Pet.timerMax / 60, 2).ToString());
                    break;
                case ElectricTroublemakerEffect.leafStorm:
                    MoveTooltip = Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.RotomLeafStorm")
                        .Replace("<dmg>", rotom.leafStormDmg.ToString())
                        .Replace("<lowLeaf>", rotom.minimumLeaf.ToString())
                        .Replace("<highLeaf>", rotom.maxLeaf.ToString())
                        .Replace("<percentOfCd>", Math.Round(rotom.leafStormCooldown * 100, 2).ToString())
                        .Replace("<percentToSeconds>", Math.Round(rotom.leafStormCooldown * rotom.Pet.timerMax / 60, 2).ToString());
                    break;
                case ElectricTroublemakerEffect.airSlash:
                    MoveTooltip = Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.RotomAirSlash")
                        .Replace("<radius>", Math.Round(rotom.airSlashRadius / 16f, 2).ToString())
                        .Replace("<dmg>", rotom.airSlashDmg.ToString())
                        .Replace("<knockback>", rotom.airSlashKb.ToString())
                        .Replace("<percentOfCd>", Math.Round(rotom.airSlashCooldown * 100, 2).ToString())
                        .Replace("<percentToSeconds>", Math.Round(rotom.airSlashCooldown * rotom.Pet.timerMax / 60, 2).ToString());
                    break;
                default:
                    break;
            };
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.TheEtomer")
                    .Replace("<class>", PetTextsColors.ClassText(rotom.PetClassPrimary, rotom.PetClassSecondary))
                    .Replace("<abilitySwitch>", PetTextsColors.KeybindText(PetKeybinds.PetAbilitySwitch))
                    .Replace("<abilityUse>", PetTextsColors.KeybindText(PetKeybinds.UsePetAbility))
                    .Replace("<dexMult>", ElectricTroublemakerEffect.dexMult.ToString())
                    .Replace("<STABMult>", ElectricTroublemakerEffect.STABMult.ToString())
                    .Replace("<baseCooldown>", Math.Round(rotom.baseCooldown / 60f, 2).ToString())
                    .Replace("<abilityTooltip>", MoveTooltip)
                ));
        }
    }
}
