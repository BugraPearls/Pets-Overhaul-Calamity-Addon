using Microsoft.Xna.Framework;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Projectiles;
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
    public sealed class DannyDevitoEffect : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Utility;
        public override PetClasses PetClassSecondary => PetClasses.Offensive;
        public int radius = 150;
        public int damage = 35;
        public int bleedDuration = 780;
        public float slow = 0.35f;
        public int slowDuration = 330;
        public int confusionChance = 40; //out of 100
        public int confusionDuration = 150;
        public int cooldown = 420;
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.AbilityPressCheck() && Pet.PetInUseWithSwapCd(CalamityPetIDs.DannyDevito))
            {
                SoundEngine.PlaySound(SoundID.Item1 with { PitchVariance = 0.2f }, Player.Center);
                Projectile petProjectile = Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), Player.Center, new Vector2(Main.MouseWorld.X - Player.Center.X, Main.MouseWorld.Y - Player.Center.Y) * 0.03f, ModContent.ProjectileType<Trashcan>(), Pet.PetDamage(damage), 4f, Player.whoAmI);
                petProjectile.DamageType = DamageClass.Generic;
                petProjectile.CritChance = (int)Player.GetTotalCritChance(DamageClass.Generic);
                Pet.timer = Pet.timerMax;
            }
        }
        public override void PreUpdate()
        {
            if (Pet.PetInUseWithSwapCd(CalamityPetIDs.DannyDevito))
            {
                Pet.SetPetAbilityTimer(cooldown);
            }
        }
    }
    public sealed class TrashmanTrashcanTooltip : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == CalamityPetIDs.DannyDevito;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }

            DannyDevitoEffect dannyDevito = Main.LocalPlayer.GetModPlayer<DannyDevitoEffect>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.TrashmanTrashcan")
                .Replace("<class>", PetTextsColors.ClassText(dannyDevito.PetClassPrimary, dannyDevito.PetClassSecondary))
                .Replace("<keybind>", PetTextsColors.KeybindText(PetKeybinds.UsePetAbility))
                .Replace("<damage>", dannyDevito.damage.ToString())
                .Replace("<bleedDuration>", Math.Round(dannyDevito.bleedDuration / 60f, 2).ToString())
                .Replace("<slow>", Math.Round(dannyDevito.slow * 100, 2).ToString())
                .Replace("<slowDuration>", Math.Round(dannyDevito.slowDuration / 60f, 2).ToString())
                .Replace("<confuseChance>", dannyDevito.confusionChance.ToString())
                .Replace("<confuseDuration>", Math.Round(dannyDevito.confusionDuration / 60f, 2).ToString())
            ));
        }
    }
}
