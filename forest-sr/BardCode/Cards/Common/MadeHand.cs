using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;
using STS2RitsuLib.Scaffolding.Content;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Cards.Common;

/// <summary>
/// 法师之手｜Made Hand
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class MadeHand : BardCard
{
    private const int energyCost = 1;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Common;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;

    // 基础数值：保留数量 + 抽牌数量
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        ModCardVars.Int("retainAmount", 1),
        new CardsVar(1)
    ];

    // 额外提示文本
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromKeyword(CardKeyword.Retain)
    ];

    // 关键词
    protected override IEnumerable<string> RegisteredKeywordIds => [BardKeywords.Magic];

    public MadeHand() : base(energyCost, type, rarity, targetType)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int retainCount = DynamicVars["retainAmount"].IntValue;

        // 抽牌
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);

        // 从手牌中选择要保留的牌（排除已有保留关键词的牌）
        CardModel[] selectedCards = (await CardSelectCmd.FromHand(
            prefs: new CardSelectorPrefs(SelectionScreenPrompt, retainCount),
            context: choiceContext,
            player: Owner,
            filter: (CardModel c) => !c.HasModKeyword(CardKeyword.Retain.ToString()),
            source: this)).ToArray();

        // 为选中的卡牌添加保留关键词
        foreach (var card in selectedCards)
        {
            CardCmd.ApplyKeyword(card, CardKeyword.Retain);
        }
    }

    protected override void OnUpgrade()
    {
        // 升级：抽牌数量 +1（1 → 2）
        DynamicVars.Cards.UpgradeValueBy(1);
        // 可选：保留数量 +1
        // DynamicVars["retainAmount"].UpgradeValueBy(1);
    }
}