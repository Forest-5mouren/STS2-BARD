using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        _wasTriggered = false;
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
        // 已经触发过，正常死亡
        if (_wasTriggered) return true;
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
        if (_wasTriggered) return;
        if (Owner == null || Owner.Player == null)
        {
            return;
        }

        try
        {
            Flash();  // 能力闪烁效果
            _wasTriggered = true;

            // 获取回复量和抽牌数（可以从 CanonicalVars 获取）
            int healAmount = 5;
            int drawAmount = 3;

            // 回复生命
            await CreatureCmd.Heal(creature, healAmount);

            // 抽牌（确保 PlayerChoiceContext 不为 null）
            await CardPileCmd.Draw(null, drawAmount, Owner.Player);

            // 消耗此能力
            await PowerCmd.Remove(this);
        }
        catch (Exception e)
        {
            // 记录错误，避免崩溃
            Bard.MainFile.Logger.Error($"NeverGiveUpPower.AfterPreventingDeath error: {e.Message}");
        }
    }
}