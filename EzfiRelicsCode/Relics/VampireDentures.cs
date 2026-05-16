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
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace EzfiRelics.EzfiRelicsCode.Relics
{
    [Pool(typeof(SharedRelicPool))]
    public class VampireDentures : EzfiRelic
    {
        public override RelicRarity Rarity => RelicRarity.Uncommon;

        public override bool IsAllowed(IRunState runState)
        {
            return MyModConfig.EnableVampireDentures;
        }

        decimal overkillAmount = 15;
        decimal healAmount = 5;


        protected override IEnumerable<DynamicVar> CanonicalVars => [
            new DynamicVar("OverkillAmount", overkillAmount),
            new DynamicVar("HealAmount", healAmount)
        ];

        public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props, Creature target, CardModel? cardSource)
        {
            if (dealer == base.Owner.Creature && result.WasTargetKilled && result.OverkillDamage >= 15)
            {
                Flash();
                await CreatureCmd.Heal(base.Owner.Creature, healAmount);
            }
        }
    }
}
