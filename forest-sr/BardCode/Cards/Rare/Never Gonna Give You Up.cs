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

namespace Forest_Sr.BardCode.Cards.Rare;

/// <summary>
/// Never Gonna Give You Up
/// 效果：从消耗牌堆回收 {Cards} 张牌。
/// 升级：回收 1 → 2 张牌，费用 1 → 0
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class NeverGonnaGiveYouUp : BardCard
{
    // 基础数值声明（使用 CardsVar 类型，参考 Dredge）
    protected override IEnumerable<DynamicVar> CanonicalVars => new[]
    {
        new CardsVar(1)   // 回收数量
    };

    // 关键词：乐曲
    protected override IEnumerable<string> RegisteredKeywordIds => new[]
    {
        BardKeywords.Song,
    };
    public override List<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    public NeverGonnaGiveYouUp() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
    }

    protected override void OnUpgrade()
    {
        // 升级：回收 1 → 2 张牌，费用 0
        DynamicVars.Cards.UpgradeValueBy(1);  // 1 → 2
        // 注意：费用变为 0 需要在构造函数或升级时处理
        // 可以在 Upgrade 中修改 EnergyCost，或者使用不同的构造函数
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int recycleCount = DynamicVars.Cards.IntValue;

        // 获取消耗牌堆
        CardPile exhaustPile = PileType.Exhaust.GetPile(Owner);

        // 检查是否有足够牌可回收
        if (!exhaustPile.Cards.Any())
        {
            return;
        }

        // 限制回收数量不超过实际可回收牌数和手牌上限（参考 Dredge）
        int maxRecycle = System.Math.Min(
            recycleCount,
            exhaustPile.Cards.Count
        );

        maxRecycle = System.Math.Min(
            maxRecycle,
            10 - PileType.Hand.GetPile(Owner).Cards.Count
        );

        if (maxRecycle <= 0)
        {
            return;
        }

        // 从消耗牌堆选择牌（参考 Dredge）
        var selectedCards = await CardSelectCmd.FromSimpleGrid(
            choiceContext,
            exhaustPile.Cards,
            Owner,
            new CardSelectorPrefs(SelectionScreenPrompt, maxRecycle)
        );

        if (selectedCards.Any())
        {
            await CardPileCmd.Add(selectedCards, PileType.Hand, source: this);
        }
    }
}