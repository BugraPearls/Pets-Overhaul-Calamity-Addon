using CalamityMod;
using CalamityMod.NPCs.PlagueEnemies;
using PetsOverhaul.PetEffects;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.Systems
{
    public class CalPetModPlayer : ModPlayer
    {
        public float rogueVelo = 0.35f;
        public float rogueDmg = 0.2f;
        public float stealthGain = 0.15f;
        public float stealthMax = 0.2f;
        public string RogueClassText => Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.MoonlingRogueTooltip")
                                            .Replace("<rogueVelocity>", Math.Round(rogueVelo * 100, 2).ToString())
                                            .Replace("<rogueDmg>", Math.Round(rogueDmg * 100, 2).ToString())
                                            .Replace("<rogueStealthGain>", Math.Round(stealthGain * 100, 2).ToString())
                                            .Replace("<rogueStealthMax>", Math.Round(stealthMax * 100, 2).ToString());
        public StatModifier CalDamageClasses => Player.GetTotalDamage<RogueDamageClass>();

        internal bool TileBeforeSymbiote = false;
        internal int X => (int)Player.Center.X / 16;
        internal int Y => (int)(Player.Bottom.Y - 1f) / 16;
        public override void PreUpdate()
        {
            TileBeforeSymbiote = CalamityUtils.ParanoidTileRetrieval(X, Y).HasTile;
        }
        public override void UpdateEquips()
        {
            if (Player.Calamity().fungalSymbiote && TileBeforeSymbiote == false && CalamityUtils.ParanoidTileRetrieval(X, Y).HasTile)
            {
                TilePlacement.AddToList(X, Y);
            }

            if (Player.TryGetModPlayer(out Junimo junimo))
            {
                junimo.extraBosses += Language.GetTextValue("Mods.PetsOverhaulCalamityAddon.JunimoExtraBosses");
            }
            if (DownedBossSystem.downedProvidence)
                junimo.externalLvlIncr += 5;
            if (DownedBossSystem.downedDoG)
                junimo.externalLvlIncr += 5;
        }
        public override void PostUpdateEquips()
        {
            if (Player.TryGetModPlayer(out Moonling moonling))
            {
                moonling.ExternalTooltips.Add(RogueClassText);
                if (moonling.PetIsEquipped() && moonling.currentClass == moonling.Tooltips.FindIndex(x => x == RogueClassText))
                {
                    Player.Calamity().rogueVelocity += rogueVelo;
                    Player.Calamity().rogueStealthMax += stealthMax;
                    Player.GetDamage<RogueDamageClass>() += rogueDmg;
                    Player.Calamity().stealthGenMoving += stealthGain;
                    Player.Calamity().stealthGenStandstill += stealthGain;
                }
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (Player.Calamity().HasAnyEnergyShield)
            {
                Player.GetModPlayer<MiniPrime>().AddShieldedStatBoosts();
            }
        }
    }
}
