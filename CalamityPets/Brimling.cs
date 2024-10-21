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
                        Projectile proj = Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), Player.Center, 2f * Main.rand.NextVector2Circular(3f, 3f), ModContent.ProjectileType<BrimstoneFireballMinion>(), Main.DamageVar(dmg, Player.luck), 0f, Player.whoAmI);
                        proj.tileCollide = false;
                        proj.DamageType = DamageClass.Generic;
                        proj.netUpdate = true;
                    }
                    Pet.timer = Pet.timerMax;
                }

                if (info.DamageSource.TryGetCausingEntity(out Entity entity))
                {
                    NPC.HitInfo hit = new NPC.HitInfo() with { Crit = false, DamageType = DamageClass.Generic, HitDirection = info.HitDirection, Knockback = 0.5f };
                    int damageTaken = info.SourceDamage > Player.statLife ? Player.statLife : info.SourceDamage; //Caps the Reflect's base damage to Player's current HP.
                    int mult = (Player.statLife < Player.statLifeMax2 * drTreshold) ? 2 : 1;

                    if (entity is Projectile projectile && projectile.TryGetGlobalProjectile(out ProjectileSourceChecks proj))
                    {
                        NPC npc = Main.npc[proj.sourceNpcId];
                        npc.StrikeNPC(hit with { Damage = (int)(damageTaken * reflectAmount * mult) });
                        if (Main.netMode != NetmodeID.SinglePlayer)
                            NetMessage.SendStrikeNPC(npc, hit);
                    }
                    else if (entity is NPC npc && npc.active == true && npc.immortal == false)
                    {
                        npc.StrikeNPC(hit with { Damage = (int)(damageTaken * reflectAmount * mult) });
                        if (Main.netMode != NetmodeID.SinglePlayer)
                            NetMessage.SendStrikeNPC(npc, hit);
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
