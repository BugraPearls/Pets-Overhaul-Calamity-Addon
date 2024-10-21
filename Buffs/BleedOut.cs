using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.Buffs
{
    public class BleedOut : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }
        public override LocalizedText Description => Language.GetText("Mods.PetsOverhaulCalamityAddon.Buffs.BleedOutTooltip");
        public override LocalizedText DisplayName => Language.GetText("Mods.PetsOverhaulCalamityAddon.Buffs.BleedOutDisplayName");
    }
    public class BleedOutGlobal : GlobalNPC
    {
        public override void SetDefaults(NPC entity)
        {
            entity.buffImmune[ModContent.BuffType<BleedOut>()] = entity.buffImmune[BuffID.Bleeding];
        }
        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (npc.HasBuff<BleedOut>() && Main.rand.NextBool(12))
            {
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood);
            }
        }
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (npc.HasBuff<BleedOut>())
            {
                damage = 4;
                npc.lifeRegen -= 16;
            }
        }
    }
}