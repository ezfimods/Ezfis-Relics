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
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace EzfiRelics.EzfiRelicsCode.Relics
{
    [Pool(typeof(SharedRelicPool))]
    public class Scalpel : EzfiRelic
    {
        public override RelicRarity Rarity => RelicRarity.Common;
        public override bool IsAllowed(IRunState runState)
        {
            return MyModConfig.EnableScalpel;
        }
        int cardDraw = 1;
        protected override IEnumerable<DynamicVar> CanonicalVars => [
           new DynamicVar("CardDraw", cardDraw)
       ];

        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            if (player == base.Owner)
            {
                if (PileType.Hand.GetPile(player).Cards.Count() > 0)
                {
                    var hand = PileType.Hand.GetPile(player).Cards;
                    bool hasUnplayable = false;
                    foreach (var card in hand)
                    {
                        if (!card.Keywords.Contains(CardKeyword.Unplayable))
                        {
                            hasUnplayable = true;
                            break;
                        }
                    }

                    if (hasUnplayable)
                    {
                        CardSelectorPrefs prefs = new CardSelectorPrefs(base.SelectionScreenPrompt, 1);
                        CardModel card = (await CardSelectCmd.FromHand(choiceContext, base.Owner, prefs, (CardModel c) => c.Keywords.Contains(CardKeyword.Unplayable),this)).FirstOrDefault();
                        if (card != null)
                        {
                            Flash();
                            await CardCmd.Exhaust(choiceContext, card);
                            await CardPileCmd.Draw(choiceContext, cardDraw, base.Owner);
                        }
                    }

                }
            }
        }
    }
}
