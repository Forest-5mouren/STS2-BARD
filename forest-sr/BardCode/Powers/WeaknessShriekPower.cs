using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Powers;

/// <summary>
/// 虚弱尖叫减益（持续本回合）：降低敌人力量和敏捷
/// </summary>
public sealed class WeaknessShriekPower : BardPower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Single;

    public decimal StrengthReduction { get; set; } = 3;
    public decimal DexterityReduction { get; set; } = 3;

    // 修改力量（减法）
    public override decimal ModifyStrengthAdditive(Creature creature, decimal amount)
    {
        return amount - StrengthReduction;
    }

    // 修改敏捷（减法）
    public override decimal ModifyDexterityAdditive(Creature creature, decimal amount)
    {
        return amount - DexterityReduction;
    }

    // 回合结束时移除
    public override async Task AfterTurnEnd(PlayerChoiceContext ctx, MegaCrit.Sts2.Core.Combat.CombatSide side)
    {
        if (side == CombatSide.Enemy)
        {
            await PowerCmd.Remove(this);
        }
    }
}

