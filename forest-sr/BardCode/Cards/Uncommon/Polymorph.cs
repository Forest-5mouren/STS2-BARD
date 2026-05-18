using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Cards.Rare;

/// <summary>
/// 变形术｜Polymorph
/// 效果：获得 {block} 点格挡。选择手牌中最多 {transformCount} 张牌，将其变形为随机的诗人牌。本回合变出的牌费用为0。
/// 升级：格挡 6→8，可选择牌数 1→2
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class Polymorph : BardCard
{
    private const string _blockKey = "block";
    private const string _transformCountKey = "transformCount";

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar( 6, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),           // 格挡值
        new DynamicVar(_transformCountKey, 1)   // 可选择牌数
    ];

    // 额外悬停提示
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.Static(StaticHoverTip.Transform)
    ];

    public Polymorph() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
    }

    // 升级：格挡 6→8，可选择牌数 1→2
    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2);           // 6 → 8
        DynamicVars[_transformCountKey].UpgradeValueBy(1);  // 1 → 2
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 获得格挡
        
        await CreatureCmd.GainBlock(Owner.Creature, new BlockVar(DynamicVars.Block.IntValue, ValueProp.Move), null);

        int transformCount = DynamicVars[_transformCountKey].IntValue;

        // 从手牌中选择卡牌进行变形
        CardSelectorPrefs selectPrefs = new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, transformCount);
        List<CardModel> selectedCards = (await CardSelectCmd.FromHand(
            choiceContext,
            Owner,
            selectPrefs,
            null,
            this)).ToList();

        // 对每张选中的牌进行变形
        foreach (CardModel selectedCard in selectedCards)
        {
            // 变形为随机诗人牌
            CardPileAddResult result = await CardCmd.TransformToRandom(
                selectedCard,
                Owner.RunState.Rng.CombatCardGeneration
            );

            // 获取变形后的卡牌
            CardModel transformedCard = result.cardAdded;

            if (transformedCard != null)
            {
                // 本回合0费
                transformedCard.EnergyCost.SetThisTurnOrUntilPlayed(0);
            }
        }
    }
}