using CalamityMod.Buffs.Summon;
using PetsOverhaulCalamityAddon.Systems;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Projectiles.Summon;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class SonOfYharonEffect : PetEffect
    {
        public float dmgRebirth = 0.2f;
        public int defRebirth = 10;
        public float msRebirth = 0.25f;
        public int fireTime = 90;
        public int rebirthDuration = 600;
        public int rebirthCooldown = 7200;
        public override PetClasses PetClassPrimary => PetClasses.None;
        public override void PostUpdateEquips()
        {

        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.PetInUseWithSwapCd(CalamityPetIDs.SonOfYharon) && Keybinds.UsePetAbility.JustPressed)
            {
                Main.NewText("placeholder");
            }
        }
    }
    public sealed class McNuggetsTooltip : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == CalamityPetIDs.SonOfYharon;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().DisableTooltipToggle == false && !Keybinds.PetTooltipHide.Current)
            {
                return;
            }
            SonOfYharonEffect yharon = Main.LocalPlayer.GetModPlayer<SonOfYharonEffect>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.PetTooltips.McNuggets")
                .Replace("<class>", PetColors.ClassText(yharon.PetClassPrimary, yharon.PetClassSecondary))
                        .Replace("<keybind>", Keybinds.UsePetAbility.GetAssignedKeys(GlobalPet.PlayerInputMode).Count > 0 ? Keybinds.UsePetAbility.GetAssignedKeys(GlobalPet.PlayerInputMode)[0] : $"[c/{Colors.RarityTrash.Hex3()}:{Language.GetTextValue("Mods.PetsOverhaul.KeybindMissing")}]")
                        .Replace("<dmgRebirth>", Math.Round(yharon.dmgRebirth * 100, 2).ToString())
                        .Replace("<defRebirth>", yharon.defRebirth.ToString())
                        .Replace("<msRebirth>", Math.Round(yharon.msRebirth * 100, 2).ToString())
                        .Replace("<fireTime>", Math.Round(yharon.fireTime / 60f, 2).ToString())
                        .Replace("<rebornDuration>", Math.Round(yharon.rebirthDuration / 60f, 2).ToString())
                        .Replace("<cooldown>", Math.Round(yharon.rebirthCooldown / 3600f, 2).ToString())
            ));
        }
    }
}
