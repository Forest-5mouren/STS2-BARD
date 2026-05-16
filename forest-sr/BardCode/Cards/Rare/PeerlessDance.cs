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
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Cards.Rare;

/// <summary>
/// 无双华舞｜PeerlessDance
/// 效果：选择一张手牌中的攻击牌，使其获得重放1。消耗。
/// 升级：费用 1 → 0
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class PeerlessDance : BardCard
{
    private const string _selectCountKey = "selectCount";

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar(_selectCountKey, 1)   // 选择数量
    ];

    // 关键词：消耗（升级后移除）
    protected override IEnumerable<string> RegisteredKeywordIds => ["EXHAUST"];

    // 额外悬停提示
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.Static(StaticHoverTip.ReplayStatic)
    ];

    public PeerlessDance() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
    }

    // 升级：费用 1 → 0
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);  
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int selectCount = DynamicVars[_selectCountKey].IntValue;

        // 从手牌中选择攻击牌
        foreach (CardModel selectedCard in await CardSelectCmd.FromHand(
            choiceContext,
            Owner,
            new CardSelectorPrefs(SelectionScreenPrompt, selectCount),
            (CardModel c) => c.Type == CardType.Attack,  // 只选攻击牌
            this))
        {
            // 增加额外打出次数
            selectedCard.BaseReplayCount++;
        }
    }
}