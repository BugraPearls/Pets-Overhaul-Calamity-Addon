using CalamityMod.Projectiles.Summon;
using PetsOverhaul.Config;
using PetsOverhaul.Projectiles;
using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class BrimlingEffect : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override PetClasses PetClassSecondary => PetClasses.Offensive;
        public float drTreshold = 0.25f;
        public float dr = 0.15f;
        public int baseDmg = 30;
        public int cooldown = 210;
        public float reflectAmount = 0.45f;
        public float kbFromReflect = 5f;
        public override void PreUpdate()
        {
            if (Pet.PetInUse(CalamityPetIDs.Brimling))
            {
                Pet.SetPetAbilityTimer(cooldown);
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (Pet.PetInUseWithSwapCd(CalamityPetIDs.Brimling) && Player.statLife < Player.statLifeMax2 * drTreshold)
            {
                Player.endurance += dr;
            }
        }
        public override void OnHurt(Player.HurtInfo info)
        {
            if (Pet.PetInUseWithSwapCd(CalamityPetIDs.Brimling))
            {
                if (Pet.timer <= 0)
                {
                    float dmg = (info.SourceDamage / 2 + Player.statDefense) * (1f + Player.endurance);
                    for (int i = 0; i < 5; i++)
                    {
                        Projectile proj = Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), Player.Center, Main.rand.NextVector2CircularEdge(4f, 4f), ModContent.ProjectileType<BrimstoneFireballMinion>(), Main.DamageVar(dmg, Player.luck), 0f, Player.whoAmI);
                        proj.tileCollide = false;
                        proj.DamageType = DamageClass.Generic;
                        proj.CritChance = (int)Player.GetCritChance(DamageClass.Generic);
                        proj.netUpdate = true;
                    }
                    Pet.timer = Pet.timerMax;
                }

                if (info.DamageSource.TryGetCausingEntity(out Entity entity))
                {
                    int damageTaken = Main.DamageVar(info.SourceDamage, Player.luck);
                    damageTaken = Math.Min(damageTaken, Player.statLife); //Caps the Reflect's base damage to Player's current HP.
                    if (entity is Projectile projectile && projectile.TryGetGlobalProjectile(out ProjectileSourceChecks proj) && Main.npc[proj.sourceNpcId].active)
                    {
                        Main.npc[proj.sourceNpcId].SimpleStrikeNPC((int)(damageTaken * reflectAmount), info.HitDirection, Main.rand.NextBool((int)Math.Min(Player.GetTotalCritChance<GenericDamageClass>(), 100), 100), kbFromReflect, DamageClass.Generic);
                    }
                    else if (entity is NPC npc && npc.active == true)
                    {
                        npc.SimpleStrikeNPC((int)(damageTaken * reflectAmount), info.HitDirection, Main.rand.NextBool((int)Math.Min(Player.GetTotalCritChance<GenericDamageClass>(), 100), 100), kbFromReflect, DamageClass.Generic);
                    }
                }
            }
        }
    }
    public sealed class CharredRelicTooltip : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == CalamityPetIDs.Brimling;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }

            BrimlingEffect brimling = Main.LocalPlayer.GetModPlayer<BrimlingEffect>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.CharredRelic")
                .Replace("<class>", PetTextsColors.ClassText(brimling.PetClassPrimary, brimling.PetClassSecondary))
                .Replace("<treshold>", Math.Round(brimling.drTreshold * 100, 2).ToString())
                .Replace("<dr>", Math.Round(brimling.dr * 100, 2).ToString())
                .Replace("<reflectAmount>", Math.Round(brimling.reflectAmount * 100, 2).ToString())
                .Replace("<cooldown>", Math.Round(brimling.cooldown / 60f, 2).ToString())
            ));
        }
    }
}
