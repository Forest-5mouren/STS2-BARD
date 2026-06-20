using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Keywords;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Powers;

/// <summary>
/// 魔法共鸣
/// 效果：本回合下一张魔法牌费用-1
/// </summary>
public sealed class NextSpellCostReductionPower : BardPower
{
    private bool _used = false;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    /// <summary>
    /// 修改卡牌费用（参考 FreeSkillPower）
    /// </summary>
    public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        modifiedCost = originalCost;

        // 已经使用过了，不再生效
        if (_used) return false;

        // 检查所有者
        if (card.Owner.Creature != base.Owner) return false;

        // 检查卡牌是否在手牌或正在打出
        bool isValidPile;
        switch (card.Pile?.Type)
        {
            case PileType.Hand:
            case PileType.Play:
                isValidPile = true;
                break;
            default:
                isValidPile = false;
                break;
        }
        if (!isValidPile) return false;

        // 检查是否是魔法牌
        if (!card.HasModKeyword(BardKeywords.Magic)) return false;

        // 减少费用
        modifiedCost = originalCost - base.Amount;
        if (modifiedCost < 0) modifiedCost = 0;

        return true;
    }

    /// <summary>
    /// 卡牌打出前触发，用于消耗这个 Power
    /// </summary>
    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        // 检查是否是当前玩家打出的牌
        if (cardPlay.Card.Owner.Creature != base.Owner) return;

        // 检查是否是魔法牌
        if (!cardPlay.Card.HasModKeyword(BardKeywords.Magic)) return;

        // 检查卡牌是否在手牌或正在打出
        bool isValidPile;
        switch (cardPlay.Card.Pile?.Type)
        {
            case PileType.Hand:
            case PileType.Play:
                isValidPile = true;
                break;
            default:
                isValidPile = false;
                break;
        }
        if (!isValidPile) return;

        // 标记已使用并减少层数
        if (!_used)
        {
            _used = true;
            await PowerCmd.Decrement(this);
        }
    }
}
