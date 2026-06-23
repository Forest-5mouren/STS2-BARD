using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace Forest_Sr.BardCode.Powers.Counters;

/// <summary>
/// 隐藏计数器：记录本场战斗已打出的乐曲牌数量
/// Amount = 乐曲牌打出次数
/// </summary>
public class SongsPlayedCounter : PowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override bool IsVisibleInternal => false;
}
