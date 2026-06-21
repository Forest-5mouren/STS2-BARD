using System.Collections.Generic;
using System.Threading.Tasks;
using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Forest_Sr.BardCode.Cards.Common;

/// <summary>
/// 蓄势待发｜BuildUpMomentum
/// 效果：获得 {vigor} 层活力。在本回合中保留你的手牌。
/// 升级：费用 2 → 1
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class BuildUpMomentum : BardCard
{
    private const string _vigorKey = "vigor";
    private const string _retainKey = "retain";

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<VigorPower>( 8),     // 活力层数
        new DynamicVar(_retainKey, 1)     // 保留手牌标记
    ];

    // 额外悬停提示
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<VigorPower>(),
        HoverTipFactory.FromKeyword(CardKeyword.Retain)
    ];

    public BuildUpMomentum() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
    }

    // 升级：费用 2 → 1
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 1. 获得活力
         
        await PowerCmd.Apply<VigorPower>(choiceContext, 
            Owner.Creature,
            DynamicVars["VigorPower"].IntValue,
            Owner.Creature,
            this
        );

        // 2. 本回合保留手牌
        int retainAmount = DynamicVars[_retainKey].IntValue;
        await PowerCmd.Apply<RetainHandPower>(choiceContext, 
            Owner.Creature,
            retainAmount,
            Owner.Creature,
            this
        );
    }
}
