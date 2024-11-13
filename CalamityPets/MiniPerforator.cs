using PetsOverhaul.Config;
using PetsOverhaul.Projectiles;
using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class MiniPerforatorEffect : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public int evilKills = 0;
        public int Level = 0;
        public List<int> expTresholds = [0, 10, 50, 150, 400, 900, 1700, 3000, 5000, 10000];
        public const int maxLvl = 9;
        public int defense = 0;
        public int health = 0;
        public float luckVal = 0;
        public float dr = 0;
        public int regen = 0;
        public float evilMult = 1.5f;
        public float drIfHurtByCrimson = 0.06f;
        public override void ResetEffects()
        {
            defense = 0;
            health = 0;
            luckVal = 0;
            dr = 0;
            regen = 0;
        }
        public override void PostUpdateMiscEffects()
        {
            if (evilKills < 0)
                evilKills = 0;

            if (evilKills >= expTresholds[maxLvl])
                Level = maxLvl;
            else
                Level = expTresholds.FindIndex(x => x > Math.Clamp(x, 0, evilKills)) - 1;

            if (Level < 0)
                Level = 0;

            if (Level >= 0)
            {
                health += 5;
                defense += 2;
            }
            if (Level >= 1)
            {
                health += 5;
            }
            if (Level >= 2)
            {
                luckVal += 0.005f;
                health += 5;
            }
            if (Level >= 3)
            {
                defense += 1;
                health += 5;
                dr += 0.01f;
            }
            if (Level >= 4)
            {
                regen += 1;
                health += 5;
            }
            if (Level >= 5)
            {
                luckVal += 0.005f;
                health += 5;
                dr += 0.01f;
                defense += 1;
            }
            if (Level >= 6)
            {
                defense += 2;
                health += 5;
                dr += 0.01f;
            }
            if (Level >= 7)
            {
                health += 5;
                dr += 0.01f;
                luckVal += 0.01f;
                regen += 1;
            }
            if (Level >= 8)
            {
                health += 5;
                defense += 2;
                dr += 0.02f;
                luckVal += 0.02f;
            }
            if (Level >= 9)
            {
                health += 5;
                defense += 1;
                dr += 0.01f;
                luckVal += 0.05f;
                regen += 1;
            }
            if (Player.ZoneCrimson)
            {
                health = (int)(health * evilMult);
                defense = (int)(defense * evilMult);
                dr *= evilMult;
                luckVal *= evilMult;
                regen = (int)(regen * evilMult);
            }

            if (Pet.PetInUseWithSwapCd(CalamityPetIDs.MiniPerforator))
            {
                Player.statLifeMax2 += health;
                Player.statDefense += defense;
                Player.endurance += dr;
            }
        }
        public override void UpdateLifeRegen()
        {
            if (Pet.PetInUseWithSwapCd(CalamityPetIDs.MiniPerforator))
            {
                Player.lifeRegen += regen;
            }
        }
        public override void ModifyLuck(ref float luck)
        {
            if (Pet.PetInUseWithSwapCd(CalamityPetIDs.MiniPerforator))
            {
                luck += luckVal;
            }
        }
        public override void Load()
        {
            GlobalPet.OnEnemyDeath += EnemyKillEffect;
        }
        public override void Unload()
        {
            GlobalPet.OnEnemyDeath -= EnemyKillEffect;
        }
        public static void EnemyKillEffect(NPC npc, Player player)
        {
            if (player.TryGetModPlayer(out MiniPerforatorEffect perf) && perf.Pet.PetInUseWithSwapCd(CalamityPetIDs.MiniPerforator) && GlobalPet.CrimsonEnemies.Contains(npc.type) && npc.SpawnedFromStatue == false)
            {
                perf.evilKills++;
            }
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (Pet.PetInUseWithSwapCd(CalamityPetIDs.MiniPerforator) && modifiers.DamageSource.TryGetCausingEntity(out Entity entity) && ((entity is Projectile proj && proj.TryGetGlobalProjectile(out ProjectileSourceChecks source) && GlobalPet.CrimsonEnemies.Contains(Main.npc[source.sourceNpcId].type)) || (entity is NPC npc && GlobalPet.CrimsonEnemies.Contains(npc.type))))
            {
                modifiers.FinalDamage *= 1f - drIfHurtByCrimson;
            }
        }
        public override void SaveData(TagCompound tag)
        {
            tag.Add("PerforatorKills", evilKills);
        }
        public override void LoadData(TagCompound tag)
        {
            if (tag.TryGet("PerforatorKills", out int kills))
            {
                evilKills = kills;
            }
        }
    }
    public sealed class BloodyVeinTooltip : PetTooltip
    {
        public override PetEffect PetsEffect => perforator;
        public static MiniPerforatorEffect perforator
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out MiniPerforatorEffect pet))
                    return pet;
                else
                    return ModContent.GetInstance<MiniPerforatorEffect>();
            }
        }
        public override string PetsTooltip => Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.BloodyVein")
                        .Replace("<drFromCrimson>", Math.Round(perforator.drIfHurtByCrimson * 100, 2).ToString())
                        .Replace("<killCount>", perforator.evilKills.ToString())
                        .Replace("<def>", perforator.defense.ToString())
                        .Replace("<hp>", perforator.health.ToString())
                        .Replace("<luck>", perforator.luckVal.ToString())
                        .Replace("<dr>", Math.Round(perforator.dr * 100, 2).ToString())
                        .Replace("<regen>", perforator.regen.ToString())
                        .Replace("<evilMult>", perforator.evilMult.ToString())
                        .Replace("<killReq>", perforator.Level >= MiniPerforatorEffect.maxLvl ? Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.JunimoMaxed") : (perforator.expTresholds[Math.Clamp(perforator.Level + 1, 0, MiniPerforatorEffect.maxLvl)] - perforator.evilKills).ToString());
    }
}
