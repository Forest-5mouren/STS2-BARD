using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace Forest_Sr.BardCode.Powers;

/// <summary>
/// 隐藏和声计数器能力
/// 挂在 Creature 上，Amount = 当前和声层数
/// 由遗物 AfterCardPlayed 递增、AfterPlayerTurnStart 清零
/// 卡牌通过 Owner.Creature.GetPower&lt;HarmonyPower&gt;()?.Amount 读取
/// </summary>
public sealed class HarmonyPower : PowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override bool IsVisibleInternal => false;
}
