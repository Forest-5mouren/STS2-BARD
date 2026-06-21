using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Cards.Uncommon;

/// <summary>
/// See You Again
/// 效果：从弃牌堆回收 {recycleCount} 张牌，然后丢弃1张手牌。
/// 升级：回收 2 → 3 张牌
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class SeeYouAgain : BardCard
{
    // 基础数值声明（使用 CardsVar 类型，参考 Dredge）
    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new CardsVar(2)   // 回收数量
    };

    // 关键词：乐曲
    public override IEnumerable<CardKeyword> CanonicalKeywords => [BardKeywords.Song];

    public SeeYouAgain() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
    }

    // 升级：回收 2 → 3 张牌
    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 获取回收数量
        int recycleCount = DynamicVars.Cards.IntValue;

        // 获取手牌容量限制（参考 Dredge：10 - 手牌数量）
        int maxRecycle = 10 - PileType.Hand.GetPile(Owner).Cards.Count;
        int actualRecycle = System.Math.Min(recycleCount, maxRecycle);

        if (actualRecycle > 0)
        {
            // 1. 从弃牌堆回收牌（参考 Dredge 的实现）
            CardPile discardPile = PileType.Discard.GetPile(Owner);

            var recycledCards = await CardSelectCmd.FromSimpleGrid(
                choiceContext,
                discardPile.Cards,
                Owner,
                new CardSelectorPrefs(SelectionScreenPrompt, actualRecycle)
            );

            if (recycledCards.Any())
            {
                await CardPileCmd.Add(recycledCards, PileType.Hand, CardPilePosition.Random, this);
            }

            // 2. 丢弃1张手牌（如果回收了牌）
            if (recycledCards.Any())
            {
                var cardsToDiscard = await CardSelectCmd.FromHandForDiscard(
                    choiceContext,
                    Owner,
                    new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, 1),
                    null,
                    this
                );

                if (cardsToDiscard.Any())
                {
                    await CardCmd.Discard(choiceContext, cardsToDiscard);
                }
            }
        }
    }
}


