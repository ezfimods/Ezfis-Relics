using BaseLib.Abstracts;
using BaseLib.Extensions;
using EzfiRelics.EzfiRelicsCode.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models;
using EzfiRelics.EzfiRelicsCode.Enchantments;

namespace EzfiRelics.EzfiRelicsCode.Relics
{
    [Pool(typeof(SharedRelicPool))]
    public class LovelyLocket : EzfiRelic
    {
        public override RelicRarity Rarity => RelicRarity.Shop;

        public override bool IsAllowed(IRunState runState)
        {
            return MyModConfig.EnableLovelyLocket;
        }

        protected override IEnumerable<IHoverTip> ExtraHoverTips
        {
            get => HoverTipFactory.FromEnchantment<Bonded>();
        }

        public override async Task AfterObtained()
        {
            foreach (CardModel item in await CardSelectCmd.FromDeckForEnchantment(prefs: new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1), player: base.Owner, enchantment: ModelDb.Enchantment<Bonded>(), amount: 1))
            {
                CardCmd.Enchant<Bonded>(item, 4m);
                CardCmd.Preview(item);
            }
        }

    }
}
