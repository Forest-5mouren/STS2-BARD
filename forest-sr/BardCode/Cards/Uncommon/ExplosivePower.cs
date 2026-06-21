using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Cards.Uncommon;

/// <summary>
/// 爆发力｜ExplosivePower
/// 效果：消耗 {vigorCost} 点活力，从抽牌堆抽攻击牌、技能牌、能力牌各1张加入手牌。
/// 升级：保留。
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class ExplosivePower : BardCard
{
    // 基础数值声明（使用 PowerVar 类型）
    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new PowerVar<VigorPower>(3)  // 消耗的活力数量
    };

    // 关键词：消耗
    public override IEnumerable<CardKeyword> CanonicalKeywords => new[]
    {
        CardKeyword.Exhaust
    };

    public ExplosivePower() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
    }

    // 升级：添加保留关键词（参考 Anointed）
    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 获取消耗数量
        int vigorCost = DynamicVars["VigorPower"].IntValue;

        // 获取当前的活力层数（参考 Anointed 的查询方式）
        var vigorPower = Owner.Creature.GetPower<VigorPower>();
        int currentVigor = vigorPower?.Amount ?? 0;

        // 检查是否有足够的活力
        if (currentVigor < vigorCost || vigorPower == null)
        {
            return;
        }

        // 消耗指定层数的活力
        await PowerCmd.ModifyAmount(choiceContext, vigorPower, -vigorCost, Owner.Creature, this, false);

        // 需要抽取的牌类型（参考 Anointed 使用 Where + ToList）
        CardType[] targetTypes = { CardType.Attack, CardType.Skill, CardType.Power };

        foreach (var targetType in targetTypes)
        {
            // 获取抽牌堆中符合条件的牌（参考 Anointed）
            var cards = PileType.Draw.GetPile(Owner).Cards
                .Where(c => c.Type == targetType )
                .ToList();

            // 如果有符合条件的牌，抽取第一张
            if (cards.Any())
            {
                var card = cards.First();
                await CardPileCmd.RemoveFromCombat(card);
                await CardPileCmd.Add(card, PileType.Hand, CardPilePosition.Random, this);
            }
        }
    }
}
