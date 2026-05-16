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

namespace Forest_Sr.BardCode.Cards.Rare;

/// <summary>
/// Never Gonna Give You Up
/// 效果：从消耗牌堆回收 {recycleCount} 张牌。
/// 升级：回收 1 → 2 张牌，费用 1 → 0
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class NeverGonnaGiveYouUp : BardCard
{
    private const string _recycleCountKey = "recycleCount";

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar(_recycleCountKey, 1)   // 回收数量
    ];

    // 关键词：乐曲、消耗（升级后移除消耗）
    protected override IEnumerable<string> RegisteredKeywordIds => IsUpgraded
        ? [BardKeywords.Song]           // 升级后：只有乐曲
        : [BardKeywords.Song, "EXHAUST"];  // 未升级：乐曲 + 消耗

    public NeverGonnaGiveYouUp() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
    }

    // 升级：回收 1 → 2 张牌，费用 1 → 0
    protected override void OnUpgrade()
    {
        DynamicVars[_recycleCountKey].UpgradeValueBy(1);  // 1 → 2
        EnergyCost.UpgradeBy(-1);                         // 1 → 0
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int recycleCount = DynamicVars[_recycleCountKey].IntValue;

        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, recycleCount);
        CardPile exhaustPile = PileType.Exhaust.GetPile(Owner);

        var selectedCards = (await CardSelectCmd.FromSimpleGrid(choiceContext, exhaustPile.Cards, Owner, prefs)).ToList();

        foreach (var card in selectedCards)
        {
            await CardPileCmd.Add(card, PileType.Hand, source: this);
        }
    }
}