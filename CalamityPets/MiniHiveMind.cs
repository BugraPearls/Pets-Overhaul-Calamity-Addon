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
    public sealed class MiniHiveMindEffect : PetEffect
    {
        public override int PetItemID => CalamityPetIDs.MiniHiveMind;
        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public int evilKills = 0;
        public int Level = 0;
        public List<int> expTresholds = [0, 10, 50, 150, 400, 900, 1700, 3000, 5000, 10000, 50000];
        public const int maxLvl = 10;
        public float damage = 0;
        public float crit = 0;
        public float luckVal = 0;
        public float pen = 0;
        public float critDmg = 0;
        public float evilMult = 1.35f;
        public float dmgIncrIfCorrupt = 0.12f;
        public override void ResetEffects()
        {
            damage = 0;
            crit = 0;
            luckVal = 0;
            pen = 0;
            critDmg = 0;
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
                damage += 0.01f;
                crit += 0.5f;
            }
            if (Level >= 1)
            {
                damage += 0.01f;
            }
            if (Level >= 2)
            {
                luckVal += 0.005f;
                damage += 0.01f;
            }
            if (Level >= 3)
            {
                damage += 0.01f;
                pen += 1f;
            }
            if (Level >= 4)
            {
                pen += 1f;
                damage += 0.01f;
                crit += 0.5f;
            }
            if (Level >= 5)
            {
                luckVal += 0.005f;
                crit += 1f;
                pen += 1f;
                damage += 0.02f;
            }
            if (Level >= 6)
            {
                damage += 0.02f;
                crit += 1f;
                pen += 2f;
            }
            if (Level >= 7)
            {
                damage += 0.01f;
                pen += 2f;
                luckVal += 0.01f;
                critDmg += 0.01f;
            }
            if (Level >= 8)
            {
                crit += 2f;
                damage += 0.02f;
                pen += 1f;
                luckVal += 0.02f;
            }
            if (Level >= 9)
            {
                crit += 1f;
                damage += 0.01f;
                pen += 2f;
                luckVal += 0.01f;
                critDmg += 0.01f;
            }
            if (Level >= 10)
            {
                luckVal += 0.05f;
                critDmg += 0.01f;
            }
            if (Player.ZoneCorrupt)
            {
                crit *= evilMult;
                damage *= evilMult;
                pen *= evilMult;
                luckVal *= evilMult;
                critDmg *= evilMult;
            }

            if (PetIsEquipped())
            {
                Player.GetCritChance<GenericDamageClass>() += crit;
                Player.GetDamage<GenericDamageClass>() += damage;
                Player.GetArmorPenetration<GenericDamageClass>() += pen;
            }
        }
        public override void ModifyLuck(ref float luck)
        {
            if (PetIsEquipped())
            {
                luck += luckVal;
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (PetIsEquipped())
            {
                modifiers.CritDamage += critDmg;
                if (GlobalPet.CorruptEnemies.Contains(target.type))
                {
                    modifiers.FinalDamage *= 1f + dmgIncrIfCorrupt;
                }
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
            if (player.TryGetModPlayer(out MiniHiveMindEffect hive) && hive.PetIsEquipped() && GlobalPet.CorruptEnemies.Contains(npc.type) && npc.SpawnedFromStatue == false)
            {
                hive.evilKills++;
            }
        }
        public override void SaveData(TagCompound tag)
        {
            tag.Add("HiveKills", evilKills);
        }
        public override void LoadData(TagCompound tag)
        {
            if (tag.TryGet("HiveKills", out int kills))
            {
                evilKills = kills;
            }
        }
    }
    public sealed class RottingEyeballTooltip : PetTooltip
    {
        public override PetEffect PetsEffect => hive;
        public static MiniHiveMindEffect hive
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out MiniHiveMindEffect pet))
                    return pet;
                else
                    return ModContent.GetInstance<MiniHiveMindEffect>();
            }
        }
        public override string PetsTooltip => Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.RottingEyeball")
                        .Replace("<incrToCorrupt>", Math.Round(hive.dmgIncrIfCorrupt * 100, 2).ToString())
                        .Replace("<killCount>", hive.evilKills.ToString())
                        .Replace("<dmg>", Math.Round(hive.damage * 100, 2).ToString())
                        .Replace("<crit>", hive.crit.ToString())
                        .Replace("<luck>", Math.Round(hive.luckVal,2).ToString())
                        .Replace("<pen>", hive.pen.ToString())
                        .Replace("<critDmg>", Math.Round(hive.critDmg * 100, 2).ToString())
                        .Replace("<evilMult>", hive.evilMult.ToString())
                        .Replace("<killReq>", hive.Level >= MiniHiveMindEffect.maxLvl ? Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.JunimoMaxed") : (hive.expTresholds[Math.Clamp(hive.Level + 1, 0, MiniHiveMindEffect.maxLvl)] - hive.evilKills).ToString());
    }
}
