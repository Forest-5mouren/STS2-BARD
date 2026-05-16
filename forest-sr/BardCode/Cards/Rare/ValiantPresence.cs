using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Powers;

namespace Forest_Sr.BardCode.Cards.Rare;

/// <summary>
/// 英勇气势｜Valiant Presence
/// 效果：能力牌。每当你失去活力时，获得等量的格挡。
/// 升级：获得固有
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class ValiantPresence : BardCard
{
    private const string _blockForVigorKey = "blockForVigor";

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar(_blockForVigorKey, 1)   // 每层活力转化为1点格挡
    ];

    // 额外悬停提示
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.Static(StaticHoverTip.Block)
    ];


    public ValiantPresence() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
    }

    // 升级：获得固有（已通过 RegisteredKeywordIds 处理）
    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

        int conversionRate = DynamicVars[_blockForVigorKey].IntValue;

        // 施加英勇气势能力
        await PowerCmd.Apply<ValiantPresencePower>(
            Owner.Creature,
            conversionRate,  // 转换比例（1:1）
            Owner.Creature,
            this
        );
    }
}