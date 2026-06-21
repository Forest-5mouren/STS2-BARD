using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Powers;

[RegisterPower]
public sealed class BladeWardPower : BardPower
{
    private const string _damageDecreaseKey = "DamageDecrease";

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar(_damageDecreaseKey, 0.5m)
    ];

    /// <summary>
    /// 修改受到的伤害倍率
    /// </summary>
    /// <param name="target">受到伤害的目标</param>
    /// <param name="amount">原始伤害量</param>
    /// <param name="props">伤害属性</param>
    /// <param name="dealer">造成伤害的生物</param>
    /// <param name="cardSource">来源卡牌</param>
    /// <returns>伤害倍率（1.0 = 无变化）</returns>
    public override decimal ModifyDamageMultiplicative(
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        // 只对自己生效
        if (target != Owner)
        {
            return 1m;
        }

        // Unpowered 伤害不减免
        if (props.HasFlag(ValueProp.Unpowered))
        {
            return 1m;
        }

        // 没有攻击来源时，不减免
        if (dealer == null)
        {
            return 1m;
        }

        // 只有当攻击者拥有"虚弱"能力时，才减免伤害
        if (!dealer.HasPower<WeakPower>())
        {
            return 1m;
        }

        // 返回伤害减免系数
        return DynamicVars[_damageDecreaseKey].BaseValue;
    }

    /// <summary>
    /// 回合结束时减少持续时间
    /// </summary>
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext ctx, Player player)
    {
        if (player != Owner?.Player) return;
        await PowerCmd.TickDownDuration(this);
    }
}
