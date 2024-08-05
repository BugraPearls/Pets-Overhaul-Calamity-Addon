using CalamityMod.Items.Pets;
using MonoMod.Utils;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetsOverhaulCalamityAddon.Systems
{
    /// <summary>
    /// Class mostly for making already existing systems in Pets Overhaul work with Calamity's added content aswell.
    /// </summary>
    internal class Compatibility
    {
        public static Dictionary<string, int> CalamityPetItemIDs = new()
        {
            { "ForgottenDragonEgg",CalamityPetIDs.Akato },
            { "AstrophageItem", CalamityPetIDs.Astrophage },
            { "BearsEye", CalamityPetIDs.Bear },
            { "CharredRelic", CalamityPetIDs.Brimling },
            { "CosmicPlushie", CalamityPetIDs.ChibiiDevourer },
            { "TrashmanTrashcan", CalamityPetIDs.DannyDevito },
            { "TheEtomer", CalamityPetIDs.ElectricTroublemaker },
            { "AbyssShellFossil", CalamityPetIDs.EscargidolonSnail },
            { "GeyserShell", CalamityPetIDs.FlakHermit },
            { "FoxDrive", CalamityPetIDs.Fox },
            { "PrimroseKeepsake", CalamityPetIDs.FurtasticDuo },
            { "RomajedaOrchid", CalamityPetIDs.Kendra },
            { "JoyfulHeart", CalamityPetIDs.LadShark },
            { "Levi", CalamityPetIDs.Levi },
            { "RottingEyeball", CalamityPetIDs.MiniHiveMind },
            { "BloodyVein", CalamityPetIDs.MiniPerforator },
            { "Pineapple", CalamityPetIDs.Pineapple},
            { "PlagueCaller", CalamityPetIDs.PlagueBringerBab },
            { "McNuggets", CalamityPetIDs.SonOfYharon },
            { "BrimstoneJewel", CalamityPetIDs.SupremeCalamitas },
            { "HermitsBoxofOneHundredMedicines", CalamityPetIDs.ThirdSage },
        };
        public static void AddPetItemNames()
        {
            PetItemIDs.PetNamesAndItems.AddRange(CalamityPetItemIDs);
        }
    }
}
