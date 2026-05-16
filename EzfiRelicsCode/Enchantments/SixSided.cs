using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EzfiRelics.EzfiRelicsCode.Extensions;

namespace EzfiRelics.EzfiRelicsCode.Enchantments
{
    public class SixSided : EzfiEnchantment
    {

        public override bool CanEnchant(CardModel card) => base.CanEnchant(card) && card.Type == CardType.Attack;

        protected override string CustomIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".EnchantmentPath();

        private int nextDamageBonus = 0;

        public override decimal EnchantDamageAdditive(decimal originalDamage, ValueProp props)
        {
            if (!props.IsPoweredAttack())
            {
                return 0m;
            }
            return nextDamageBonus;
        }

        public override Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
        {
            nextDamageBonus = Random.Shared.Next(1, 6);
            return Task.CompletedTask;
        }

    }
}
