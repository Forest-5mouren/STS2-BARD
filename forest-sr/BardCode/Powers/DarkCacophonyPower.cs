using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Powers;

/// <summary>
/// 黑暗喧嚣能力：每回合开始时对所有敌人造成伤害
/// </summary>
public sealed class DarkCacophonyPower : BardPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext ctx, Player player)
    {
        if (player != Owner?.Player) return;
        Flash();
        await CreatureCmd.Damage(
            ctx,
            Owner.CombatState.HittableEnemies,
            Amount,
            ValueProp.Unblockable | ValueProp.Unpowered,
            Owner,
            null);
    }
}
