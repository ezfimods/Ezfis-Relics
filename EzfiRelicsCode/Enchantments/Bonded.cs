using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EzfiRelics.EzfiRelicsCode.Extensions;

namespace EzfiRelics.EzfiRelicsCode.Enchantments
{
    public class Bonded : EzfiEnchantment
    {
        public override bool CanEnchant(CardModel card) => base.CanEnchant(card) && card.Type == CardType.Power;

        protected override void OnEnchant()
        {
            base.Card.AddKeyword(CardKeyword.Innate);
            base.Card.EnergyCost.SetCustomBaseCost(base.Card.EnergyCost.Canonical + 1);
        }

    }
}
