using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;

namespace Forest_Sr.BardCode.Cards.Uncommon;

/// <summary>
/// 道听途说｜Hearsay
/// 效果：随机获得一张诗人乐曲或法术牌，其本回合0费。
/// 升级：移除消耗
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class Hearsay : BardCard
{
    private CardModel? _mockSelectedCard; // 临时存储选中的卡牌（预览用途）

    // 关键词（升级后会被移除）
    public override List<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust
    ];


    public Hearsay() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardModel selectedCard;

        // 测试用：如果预设了模拟卡牌
        if (_mockSelectedCard != null)
        {
            selectedCard = _mockSelectedCard;
        }
        else
        {
            // 获取所有已解锁的诗人卡牌
            var allUnlockedCards = Owner.Character.CardPool
                .GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint);

            // 过滤出乐曲或法术牌
            var eligibleCards = allUnlockedCards
                .Where(card => card.HasModKeyword(BardKeywords.Song)
                            || card.HasModKeyword(BardKeywords.Magic))
                .ToList();

            // 如果没有符合条件的卡牌，返回
            if (eligibleCards.Count == 0)
            {
                return;
            }

            // 随机选择3张供玩家选择
            List<CardModel> cards = CardFactory.GetDistinctForCombat(
                Owner,
                eligibleCards,
                3,
                Owner.RunState.Rng.CombatCardGeneration)
                .ToList();

            selectedCard = await CardSelectCmd.FromChooseACardScreen(
                choiceContext,
                cards,
                Owner,
                canSkip: true);
        }

        if (selectedCard != null)
        {
            // 设置本回合0费
            selectedCard.EnergyCost.SetThisTurnOrUntilPlayed(0);

            // 添加到手牌（注意：creator 参数）
            await CardPileCmd.AddGeneratedCardToCombat(selectedCard, PileType.Hand, Owner, CardPilePosition.Random);
        }
    }

    // 升级：移除消耗 - 通过 RegisteredKeywordIds 已处理，不需要额外代码
    // 如果需要运行时移除，可以保留此方法但通常不需要
    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
}

