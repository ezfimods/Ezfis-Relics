using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using EzfiRelics.EzfiRelicsCode.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Runs;

namespace EzfiRelics.EzfiRelicsCode.Relics;

[Pool(typeof(SharedRelicPool))]
public class ProteinPowder : EzfiRelic
{
    public override RelicRarity Rarity => RelicRarity.Common;
    public override bool IsAllowed(IRunState runState)
    {
        return MyModConfig.EnableProteinPowder;
    }
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<FrailPower>(),
        HoverTipFactory.FromPower<WeakPower>(),
    ];

    public override async Task BeforeTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side == CombatSide.Enemy)
        {
            Player player = base.Owner;

            decimal frailAmount = player.Creature.GetPowerAmount<FrailPower>();
            decimal weakAmount = player.Creature.GetPowerAmount<WeakPower>();

            if (frailAmount > 0 || weakAmount > 0) {
                Flash();
                if (frailAmount > 0)
                {
                    await PowerCmd.SetAmount<FrailPower>(player.Creature, frailAmount / 2, null, null);
                }

                if (weakAmount > 0)
                {
                    await PowerCmd.SetAmount<WeakPower>(player.Creature, weakAmount / 2, null, null);
                }
            }
        }
    }
}