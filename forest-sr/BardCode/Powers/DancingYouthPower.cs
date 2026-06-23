using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Forest_Sr.BardCode.Powers;

/// <summary>
/// 舞动青春能力：回合结束时若和声大于3，下回合抽1张牌
/// </summary>
public sealed class DancingYouthPower : BardPower
{
    private bool _shouldDrawNextTurn = false;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override decimal ModifyHandDraw(Player player, decimal count)
    {
        if (player != Owner?.Player) return count;
        if (!_shouldDrawNextTurn) return count;

        Flash();
        _shouldDrawNextTurn = false;
        return count + 1m;
    }

    // 回合开始时从共享 HarmonyPower 读取上一回合的和声，重置标记
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext ctx, Player player)
    {
        if (player != Owner?.Player) return;

        var harmonyPower = Owner.GetPower<HarmonyPower>();
        if (harmonyPower != null && harmonyPower.Amount > 3)
        {
            _shouldDrawNextTurn = true;
        }
        await Task.CompletedTask;
    }
}
