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
/// 群体变形术｜MassPolymorph
/// 效果：消耗。选择手牌中任意张牌，将其变为随机法术牌。
/// 升级：变形得到的卡牌自动升级
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class MassPolymorph : BardCard
{
    // 无动态变量
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    // 关键词：消耗、魔法
    protected override IEnumerable<string> RegisteredKeywordIds => [
        BardKeywords.Magic
    ];
    public override List<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    // 额外悬停提示
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.Static(StaticHoverTip.Transform)
    ];

    public MassPolymorph() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 播放施法动画
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

        // 从手牌中选择任意张牌进行变形
        CardSelectorPrefs selectPrefs = new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, 0, 999);
        List<CardModel> selectedCards = (await CardSelectCmd.FromHand(
            choiceContext,
            Owner,
            selectPrefs,
            null,
            this)).ToList();

        if (selectedCards.Count == 0) return;

        // 对每张选中的牌进行变形
        foreach (CardModel card in selectedCards)
        {
            // 使用 TransformToRandom 变形为随机牌
            CardPileAddResult result = await CardCmd.TransformToRandom(
                card,
                Owner.RunState.Rng.CombatCardGeneration
            );

            CardModel transformedCard = result.cardAdded;

            // 如果升级，自动升级变形后的卡牌
            if (transformedCard != null && IsUpgraded)
            {
                CardCmd.Upgrade(transformedCard);
            }
        }
    }

    protected override void OnUpgrade()
    {
        // 升级效果：变形得到的卡牌自动升级（在 OnPlay 中通过 Upgraded 属性处理）
    }
}