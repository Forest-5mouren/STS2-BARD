using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
namespace Forest_Sr.BardCode.Powers;
/// <summary>
/// 绝不认输能力
/// </summary>
public sealed class NeverGiveUpPower : BardPower
{
    // 效果类型：增益效果
    public override PowerType Type => PowerType.Buff;
    // 堆叠类型：无层数（单次触发）
    public override PowerStackType StackType => PowerStackType.Single;
    // 不允许负层数
    public override bool AllowNegative => false;
    private bool _wasTriggered;
    public NeverGiveUpPower()
    {

    }
    /// <summary>
    /// 判断是否应该死亡（参考 LizardTail）
    /// </summary>
    public override bool ShouldDieLate(Creature creature)
    {
        // 防御性检查
        if (creature == null) return true;
        if (Owner == null) return true;
        // 不是自己，正常死亡
        if (creature != Owner) return true;
        // 否则阻止死亡
        return false;
    }
    /// <summary>
    /// 阻止死亡后执行（参考 LizardTail）
    /// </summary>
    public override async Task AfterPreventingDeath(Creature creature)
    {
        // 防御性检查
        if (creature == null)
        {
            return;
        }
        if (creature != Owner) return;
        if (Owner == null || Owner.Player == null)
        {
            return;
        }
        Flash();  // 能力闪烁效果
        // 获取回复量和抽牌数（可以从 CanonicalVars 获取）
        int healAmount = 5;
        // 回复生命
        await CreatureCmd.Heal(creature, healAmount);
        // 消耗此能力
        await PowerCmd.Remove(this);
    }
}
