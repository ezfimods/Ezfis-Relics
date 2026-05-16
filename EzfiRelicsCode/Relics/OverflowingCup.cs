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
    public class OverflowingCup : EzfiRelic
    {
        public override RelicRarity Rarity => RelicRarity.Rare;

        public override bool IsAllowed(IRunState runState)
        {
            return MyModConfig.EnableOverflowingCup;
        }
        public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props, Creature target, CardModel? cardSource)
        {
            if (dealer == base.Owner.Creature && result.WasTargetKilled && result.OverkillDamage > 0)
            {
                Flash();
                Creature creature = base.Owner.RunState.Rng.CombatTargets.NextItem(base.Owner.Creature.CombatState.HittableEnemies);
                if (creature != null)
                {
                    DamageVar overkill = new DamageVar(result.OverkillDamage, props);
                    VfxCmd.PlayOnCreatureCenter(creature, "vfx/vfx_attack_blunt");
                    await CreatureCmd.Damage(choiceContext, creature, overkill, dealer);
                }
            }
        }
    }
}
