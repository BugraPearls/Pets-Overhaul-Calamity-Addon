using CalamityMod;
using CalamityMod.Items.Accessories;
using PetsOverhaul.Items;
using PetsOverhaul.PetEffects;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
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
    }
}
