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
    public class HoleInOneClub : EzfiRelic
    {
        public override RelicRarity Rarity => RelicRarity.Uncommon;

        private const int maxHP = 8;
        public override bool IsAllowed(IRunState runState)
        {
            return MyModConfig.EnableHoleInOneClub;
        }

        protected override IEnumerable<DynamicVar> CanonicalVars => [
            new DynamicVar("MaxHP", maxHP),
        ];

        public override async Task AfterCombatVictory(CombatRoom room)
        {
            if (Owner.Creature.CombatState?.RoundNumber == 1)
            {
                Flash();
                await CreatureCmd.GainMaxHp(base.Owner.Creature, maxHP);
            }
        }
    }
}
