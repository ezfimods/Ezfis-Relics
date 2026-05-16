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
using EzfiRelics.EzfiRelicsCode.Enchantments;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models;

namespace EzfiRelics.EzfiRelicsCode.Relics
{
    [Pool(typeof(SharedRelicPool))]
    public class SneckoDice : EzfiRelic
    {
        public override RelicRarity Rarity => RelicRarity.Common;
        public override bool IsAllowed(IRunState runState)
        {
            return MyModConfig.EnableSneckoDice;
        }
        protected override IEnumerable<IHoverTip> ExtraHoverTips
        {
            get => HoverTipFactory.FromEnchantment<SixSided>();
        }

        public override async Task AfterObtained()
        {
            foreach (CardModel item in await CardSelectCmd.FromDeckForEnchantment(prefs: new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1), player: base.Owner, enchantment: ModelDb.Enchantment<SixSided>(), amount: 1))
            {
                CardCmd.Enchant<SixSided>(item, 4m);
                CardCmd.Preview(item);
            }
        }
    }
}
