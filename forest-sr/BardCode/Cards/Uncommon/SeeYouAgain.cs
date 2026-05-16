using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
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
    private const string _recycleCountKey = "recycleCount";

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar(_recycleCountKey, 2)   // 回收数量
    ];

    // 关键词：乐曲
    protected override IEnumerable<string> RegisteredKeywordIds => [
        BardKeywords.Song
    ];


    public SeeYouAgain() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
    }

    // 升级：回收 2 → 3 张牌
    protected override void OnUpgrade()
    {
        DynamicVars[_recycleCountKey].UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int recycleCount = DynamicVars[_recycleCountKey].IntValue;

        // 1. 从弃牌堆回收牌
        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, recycleCount);
        CardPile discardPile = PileType.Discard.GetPile(Owner);
        CardModel recycledCard = (await CardSelectCmd.FromSimpleGrid(choiceContext, discardPile.Cards, Owner, prefs)).FirstOrDefault();

        if (recycledCard != null)
        {
            await CardPileCmd.Add(recycledCard, PileType.Hand, source: this);
        }

        // 2. 丢弃1张手牌
        await CardCmd.Discard(
            choiceContext,
            await CardSelectCmd.FromHandForDiscard(
                choiceContext,
                Owner,
                new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, 1),
                null,
                this
            )
        );
    }
}