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
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Runs;

namespace EzfiRelics.EzfiRelicsCode.Relics
{
    [Pool(typeof(SharedRelicPool))]
    public class HeavyShield : EzfiRelic
    {
        public override RelicRarity Rarity => RelicRarity.Common;
        public override bool IsAllowed(IRunState runState)
        {
            return MyModConfig.EnableHeavyShield;
        }
        public override Task AfterSideTurnStart(CombatSide side, CombatState combatState)
        {
            if (side != base.Owner.Creature.Side)
            {
                return Task.CompletedTask;
            }
            List<Creature> enemies = combatState.HittableEnemies.ToList();
            int blockBonus = 0;

            foreach (Creature enemy in enemies)
            {
                if (enemy.Block > blockBonus) blockBonus = enemy.Block;
            }

            if (blockBonus > 0)
            {
                Flash();
                BlockVar blockBonusVar = new BlockVar(blockBonus / 2, MegaCrit.Sts2.Core.ValueProps.ValueProp.Unpowered);
                CreatureCmd.GainBlock(base.Owner.Creature, blockBonusVar, null);
            }

            return Task.CompletedTask;
        }
    }
}
