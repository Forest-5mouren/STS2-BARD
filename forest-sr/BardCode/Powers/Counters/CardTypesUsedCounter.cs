using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace Forest_Sr.BardCode.Powers.Counters;

/// <summary>
/// 隐藏计数器：记录本场战斗已使用过的卡牌类型（Attack/Skill/Power/Ancient等）
/// Amount = 已记录的不同类型数量
/// RecordedTypes = 已出现过的类型集合
/// </summary>
public class CardTypesUsedCounter : PowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override bool IsVisibleInternal => false;

    /// <summary>
    /// 已记录过的卡牌类型，用于去重判断
    /// </summary>
    public HashSet<CardType> RecordedTypes { get; set; } = new();
}
