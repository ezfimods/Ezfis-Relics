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

namespace EzfiRelics.EzfiRelicsCode.Relics
{
    [Pool(typeof(SharedRelicPool))]
    public class LongTooth : EzfiRelic
    {
        public override RelicRarity Rarity => RelicRarity.Common;
        int strAmount = 1;
        int turnFrequency = 2;

        public override bool IsAllowed(IRunState runState)
        {
            return MyModConfig.EnableLongTooth;
        }

        protected override IEnumerable<DynamicVar> CanonicalVars => [
            new DynamicVar("StrengthBonus", strAmount),
            new DynamicVar("TurnFrequency", turnFrequency)
        ];

        public override Task AfterSideTurnStart(CombatSide side, CombatState combatState)
        {
            if (side != base.Owner.Creature.Side)
            {
                return Task.CompletedTask;
            }
            if (combatState.RoundNumber % turnFrequency == 0)
            {
                Flash();
                PowerCmd.Apply<StrengthPower>(base.Owner.Creature, strAmount, base.Owner.Creature, null);
            }
            return Task.CompletedTask;
        }
    }
}
