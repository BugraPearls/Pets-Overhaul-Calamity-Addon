using CalamityMod;
using PetsOverhaul.PetEffects;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.Systems
{
    public class CalPetModPlayer : ModPlayer
    {
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
        }
        public override void PostUpdateEquips()
        {
            if (Player.Calamity().HasAnyEnergyShield)
            {
                Player.GetModPlayer<MiniPrime>().shieldedStatBoostActive = true;
            }
        }
    }
}
