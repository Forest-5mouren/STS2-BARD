using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;
using STS2RitsuLib.Scaffolding.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Cards.Uncommon;  // 注意修改为你的命名空间

/// <summary>
/// 爆发力｜ExplosivePower
/// 效果：消耗 {vigorCost} 点活力，从抽牌堆抽攻击牌、技能牌、能力牌各1张加入手牌。
/// 升级：保留。
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class ExplosivePower : BardCard
{
    private const string _vigorCostKey = "vigorCost";

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar(_vigorCostKey, 3)
    ];

    // 关键词
    protected override IEnumerable<string> RegisteredKeywordIds => [
        "EXHAUST"
    ];

    public ExplosivePower() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
    }

    // 升级：添加保留关键词
    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 获取当前的活力层数
        var vigorPower = Owner.Creature.GetPower<VigorPower>();
        int vigor = vigorPower?.Amount ?? 0;
        int vigorCost = DynamicVars[_vigorCostKey].IntValue;

        // 检查是否有足够的活力
        if (vigor < vigorCost || vigorPower == null)
        {
            return;
        }

        // 消耗指定层数的活力
        await PowerCmd.ModifyAmount( vigorPower, -vigorCost, Owner.Creature, this);

        // 需要抽的牌类型
        CardType[] targetTypes = { CardType.Attack, CardType.Skill, CardType.Power };

        foreach (var targetType in targetTypes)
        {
            // 确保抽牌堆有牌
            await CardPileCmd.ShuffleIfNecessary(choiceContext, Owner);

            // 获取抽牌堆
            var drawPile = PileType.Draw.GetPile(Owner);

            // 查找指定类型的第一张牌（排除不可打出的牌）
            var card = drawPile.Cards.FirstOrDefault(c => c.Type == targetType && !c.HasModKeyword("UNPLAYABLE"));

            // 如果找到了，抽这张牌
            if (card != null)
            {
                await CardPileCmd.RemoveFromCombat(card);
                await CardPileCmd.Add(card, PileType.Hand, source: this);
            }
        }
    }
}