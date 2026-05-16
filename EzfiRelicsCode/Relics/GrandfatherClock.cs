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
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace EzfiRelics.EzfiRelicsCode.Relics
{
    [Pool(typeof(SharedRelicPool))]
    public class GrandfatherClock : EzfiRelic
    {
        private const string _damageTurnKey = "DamageTurn";

        int activateTurn = 12;
        int numOfCards = 12;
        int _cardsPlayedThisTurn = 0;

        public override bool IsAllowed(IRunState runState)
        {
            return MyModConfig.EnableGrandfatherClock;
        }

        protected override IEnumerable<DynamicVar> CanonicalVars => [
            new DynamicVar("ActivateTurn", activateTurn),
            new DynamicVar("NumOfCards", numOfCards)
        ];


        public override RelicRarity Rarity => RelicRarity.Uncommon;


        public override Task AfterSideTurnStart(CombatSide side, CombatState combatState)
        {
            if (side != base.Owner.Creature.Side)
            {
                return Task.CompletedTask;
            }
            if (combatState.RoundNumber == activateTurn)
            {
                base.Status = RelicStatus.Active;
                _cardsPlayedThisTurn = 0;
                Flash();
            }
            return Task.CompletedTask;
        }

        public override Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
        {
            if (cardPlay.Card.Owner == base.Owner)
            {
                _cardsPlayedThisTurn++;
            }
            return Task.CompletedTask;
        }

        public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
        {
            modifiedCost = originalCost;
            if (base.Owner.Creature.CombatState.RoundNumber == activateTurn && _cardsPlayedThisTurn <= numOfCards)
            {
                modifiedCost = 0;
                return true;
            }
            return false;
        }

    }
}
