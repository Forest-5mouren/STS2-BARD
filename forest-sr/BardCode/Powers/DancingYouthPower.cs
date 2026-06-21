using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Threading.Tasks;
using Forest_Sr.BardCode;
using STS2RitsuLib.Keywords;
using Forest_Sr.BardCode.Cards.KeyWord;

namespace Forest_Sr.BardCode.Powers;

/// <summary>
/// 舞动青春能力：回合结束时若和声大于3，下回合抽1张牌
/// </summary>
public sealed class DancingYouthPower : BardPower
{
    private bool _shouldDrawNextTurn = false;
    private int _turnHarmony = 0;

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

    // 追踪和声（与遗物同步）
    public override async Task AfterCardPlayed(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature != Owner) return;
        if (cardPlay.Card.HasModKeyword(BardKeywords.Song) ||
            cardPlay.Card.HasModKeyword(BardKeywords.Magic))
        {
            _turnHarmony++;
        }
        await Task.CompletedTask;
    }

    // 回合开始时检查上一回合和声，重置计数
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext ctx, Player player)
    {
        if (player != Owner?.Player) return;

        if (_turnHarmony > 3)
        {
            _shouldDrawNextTurn = true;
        }
        _turnHarmony = 0;
        await Task.CompletedTask;
    }
}


