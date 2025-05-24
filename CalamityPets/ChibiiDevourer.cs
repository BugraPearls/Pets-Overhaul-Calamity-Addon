using PetsOverhaul.PetEffects;
using PetsOverhaul.Systems;
using PetsOverhaulCalamityAddon.Systems;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PetsOverhaulCalamityAddon.CalamityPets
{
    public sealed class ChibiiDevourerEffect : PetEffect
    {
        public override int PetItemID => CalamityPetIDs.ChibiiDevourer;
        public override PetClasses PetClassPrimary => PetClasses.Supportive;
        public int dmg = 99999;
        public int block = 320;
        public float kb = 100f;
        public bool DiffCheck => Player.difficulty == PlayerDifficultyID.Hardcore;
        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (DiffCheck)
            {
                SoundEngine.PlaySound(SoundID.Meowmere, Player.Center);
                GlobalPet.CircularDustEffect(Player.Center, DustID.PinkFairy, block, 30);
                foreach (var npc in Main.ActiveNPCs)
                {
                    if (Player.Distance(npc.Center) < block && npc.dontTakeDamage == false)
                    {
                        npc.SimpleStrikeNPC(Pet.PetDamage(dmg, DamageClass.Generic), Player.direction, true, kb, DamageClass.Generic, true, Player.luck);
                    }
                }
                foreach (var player in Main.ActivePlayers)
                {
                    if (Player.Distance(player.Center) < block && Player.creativeGodMode == false)
                    {
                        string reason = Main.rand.Next(4) switch
                        {
                            0 => Compatibility.LocVal("PetTooltips.CosmicDeath1"),
                            1 => Compatibility.LocVal("PetTooltips.CosmicDeath2"),
                            2 => Compatibility.LocVal("PetTooltips.CosmicDeath3"),
                            3 => Compatibility.LocVal("PetTooltips.CosmicDeath4"),
                            _ => Compatibility.LocVal("PetTooltips.CosmicDeath1"),
                        };
                        player.Hurt(PlayerDeathReason.ByCustomReason(reason.Replace("<name>", Player.name)), dmg, Player.direction, true, dodgeable: false, scalingArmorPenetration: 1f, knockback: kb);
                    }
                }
            }
        }
    }
    public sealed class CosmicPlushieTooltip : PetTooltip
    {
        public override PetEffect PetsEffect => chibiiDevourer;
        public static ChibiiDevourerEffect chibiiDevourer
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out ChibiiDevourerEffect pet))
                    return pet;
                else
                    return ModContent.GetInstance<ChibiiDevourerEffect>();
            }
        }
        public override string PetsTooltip => Compatibility.LocVal("PetTooltips.CosmicPlushie")
                .Replace("<color>", PetTextsColors.SupportiveClass.Hex3())
                .Replace("<damage>", chibiiDevourer.dmg.ToString())
                .Replace("<block>", Math.Round(chibiiDevourer.block / 16f, 2).ToString());
    }
}
