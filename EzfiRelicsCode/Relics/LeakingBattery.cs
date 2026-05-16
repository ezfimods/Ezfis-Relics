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

namespace EzfiRelics.EzfiRelicsCode.Relics
{
    [Pool(typeof(SharedRelicPool))]
    public class LeakingBattery : EzfiRelic
    {
        const int damageEach = 3;
        public override RelicRarity Rarity => RelicRarity.Common;
        protected override IEnumerable<DynamicVar> CanonicalVars => [
            new DynamicVar("DamageAmount", damageEach)
        ];
        public override bool IsAllowed(IRunState runState)
        {
            return MyModConfig.EnableLeakingBattery;
        }

        public override async Task BeforeTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
        {
            if (side == CombatSide.Player)
            {
                int energyRemaining = base.Owner.PlayerCombatState.Energy;

                if (energyRemaining > 0)
                {
                    Flash();
                    for (int i = 0; i < energyRemaining; i++)
                    {
                        Creature creature = base.Owner.RunState.Rng.CombatTargets.NextItem(base.Owner.Creature.CombatState.HittableEnemies);
                        await CreatureCmd.Damage(choiceContext, creature, damageEach, MegaCrit.Sts2.Core.ValueProps.ValueProp.Unpowered, base.Owner.Creature);
                    }
                }
            }
        }
    }
}
