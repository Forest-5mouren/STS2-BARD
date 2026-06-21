using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;using MegaCrit.Sts2.Core.Entities.Cards;using MegaCrit.Sts2.Core.Entities.Creatures;using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;using MegaCrit.Sts2.Core.HoverTips;using MegaCrit.Sts2.Core.Models;using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Powers;

/// <summary>
/// 魔法武器能力：所有非魔法攻击牌获得魔法词条
/// </summary>
[RegisterPower]
public sealed class MagicWeaponPower : BardPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;


    /// <summary>
    /// 为攻击牌添加魔法词条
    /// </summary>
    private void ApplyMagicKeywordToAttackCards()
    {
        var allCards = Owner.Player?.PlayerCombatState?.AllCards ?? Array.Empty<CardModel>();

        foreach (var card in allCards)
        {
            if (card.Type == CardType.Attack && !card.HasModKeyword(BardKeywords.Magic))
            {
                card.AddModKeyword(BardKeywords.Magic);
            }
        }
    }

    /// <summary>
    /// 当 Power 层数变化时触发
    /// </summary>
    public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (!(power is MagicWeaponPower)) return Task.CompletedTask;
        if (power.Owner != Owner) return Task.CompletedTask;

        ApplyMagicKeywordToAttackCards();

        return Task.CompletedTask;
    }

    /// <summary>
    /// 卡牌进入战斗时触发
    /// </summary>
    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card.Owner != Owner.Player) return Task.CompletedTask;
        if (card.Type != CardType.Attack) return Task.CompletedTask;
        if (card.HasModKeyword(BardKeywords.Magic)) return Task.CompletedTask;

        // ✅ 修正：使用 AddKeyword
        card.AddModKeyword(BardKeywords.Magic);

        return Task.CompletedTask;
    }

    /// <summary>
    /// 能力被移除时触发
    /// </summary>
    public override Task AfterRemoved(Creature oldOwner)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// 修改卡牌费用
    /// </summary>
    public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        modifiedCost = originalCost;

        if (card.Owner.Creature != Owner) return false;
        if (card.Type != CardType.Attack) return false;

        // ✅ 修正：使用 HasModKeyword 检查和 AddKeyword
        if (!card.HasModKeyword(BardKeywords.Magic))
        {
            card.AddModKeyword(BardKeywords.Magic);
        }

        return false;
    }
}


