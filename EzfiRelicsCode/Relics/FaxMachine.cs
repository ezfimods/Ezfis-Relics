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
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Unlocks;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace EzfiRelics.EzfiRelicsCode.Relics
{
    [Pool(typeof(SharedRelicPool))]
    public class FaxMachine : EzfiRelic
    {
        public override RelicRarity Rarity => RelicRarity.Rare;

        public override bool IsAllowed(IRunState runState)
        {
            if (!MyModConfig.EnableFaxMachine)
            {
                return false;
            }
            if (runState.Players.Any(delegate (Player p)
            {
                if (p != null && p.Character is Ironclad)
                {
                    UnlockState unlockState = p.UnlockState;
                    if (unlockState != null)
                    {
                        return unlockState.NumberOfRuns == 0;
                    }
                }
                return false;
            }))
            {
                return false;
            }
            return RelicModel.IsBeforeAct3TreasureChest(runState);
        }

        public override bool TryModifyCardRewardOptions(Player player, List<CardCreationResult> options, CardCreationOptions creationOptions)
        {
            if (base.Owner != player)
            {
                return false;
            }
            if (creationOptions.Source != CardCreationSource.Encounter)
            {
                return false;
            }

            IEnumerable<CardPoolModel> cardPools = player.UnlockState.CharacterCardPools.Where(pool => pool != player.Character.CardPool);
                                                
            CardCreationOptions options2 = new CardCreationOptions(cardPools, CardCreationSource.Other, creationOptions.RarityOdds).WithFlags(CardCreationFlags.NoModifyHooks | CardCreationFlags.NoCardPoolModifications);
            CardModel cardModel = CardFactory.CreateForReward(base.Owner, 1, options2).FirstOrDefault()?.Card;
            if (cardModel != null)
            {
                CardCreationResult cardCreationResult = new CardCreationResult(cardModel);
                cardCreationResult.ModifyCard(cardModel, this);
                options.Add(cardCreationResult);
                Flash();
            }
            return cardModel != null;
        }

    }
}
