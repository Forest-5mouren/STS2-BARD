using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using System.Threading.Tasks;
using Forest_Sr.BardCode;

namespace Forest_Sr.BardCode.Powers;

/// <summary>
/// 舞动青春能力：回合结束时若和声大于3，下回合抽1张牌
/// </summary>
public sealed class DancingYouthPower : BardPower
{
    private bool _shouldDrawNextTurn = false;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    // 修改摸牌数量
    public override decimal ModifyHandDraw(Player player, decimal count)
    {
        if (player != Owner?.Player) return count;
        if (!_shouldDrawNextTurn) return count;

        Flash();
        _shouldDrawNextTurn = false;
        return count + 1m;
    }

    // 回合结束时检查和声
    public override async Task AfterTurnEnd(PlayerChoiceContext ctx, CombatSide side)
    {
        if (side == Owner.Side && HarmonyTracker.Count > 3)
        {
            _shouldDrawNextTurn = true;
        }
        await Task.CompletedTask;
    }
}

