using CalamityMod;
using PetsOverhaul.PetEffects;
using PetsOverhaul.Systems;
using System;
using Terraria;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.Systems
{
    public class CalPetModPlayer : ModPlayer
    {
        public float rogueVelo = 0.35f;
        public float rogueDmg = 0.2f;
        public float stealthGain = 0.15f;
        public float stealthMax = 0.2f;
        public string RogueClassText => Compatibility.LocVal("MoonlingRogueTooltip")
                                            .Replace("<rogueVelocity>", Math.Round(rogueVelo * 100, 2).ToString())
                                            .Replace("<rogueDmg>", Math.Round(rogueDmg * 100, 2).ToString())
                                            .Replace("<rogueStealthGain>", Math.Round(stealthGain * 100, 2).ToString())
                                            .Replace("<rogueStealthMax>", Math.Round(stealthMax * 100, 2).ToString());
        public StatModifier CalDamageClasses => Player.GetTotalDamage<RogueDamageClass>();

        internal bool TileBeforeSymbiote = false;
        internal int X => (int)Player.Center.X / 16;
        internal int Y => (int)(Player.Bottom.Y - 1f) / 16;
        public override void Load()
        {
            On_Player.Update += EvenMorePreUpdate;
        }

        private static void EvenMorePreUpdate(On_Player.orig_Update orig, Player self, int i)
        {
            if (self.TryGetModPlayer(out Junimo junimo)) //This is done so its consistently added to junimo's externalLvlIncr field before its utilized to prevent loads of level ups before Player joins into the world.
            {
                junimo.extraBosses += Compatibility.LocVal("JunimoExtraBosses");
                if (DownedBossSystem.downedProvidence)
                    junimo.externalLvlIncr += 5;
                if (DownedBossSystem.downedDoG)
                    junimo.externalLvlIncr += 5;
            }
            orig(self, i);
        }

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
