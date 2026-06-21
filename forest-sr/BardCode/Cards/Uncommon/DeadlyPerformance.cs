using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Cards.Uncommon;

/// <summary>
/// 夺命演奏｜Deadly Performance
/// 效果：每当你打出带有法术标签的牌时，对随机敌人造成 {damage} 点伤害。
/// 升级：伤害 3 → 4
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class DeadlyPerformance : BardCard
{
    

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(3,ValueProp.Move)
    ];

    // 额外悬停提示
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.Static(StaticHoverTip.Fatal)
    ];

    public DeadlyPerformance() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
    }

    // 升级：伤害 3 → 4
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        

        // 施放能力，将伤害值传给能力
        await PowerCmd.Apply<DeadlyPerformancePower>(choiceContext, 
            Owner.Creature,
            DynamicVars.Damage.IntValue,
            Owner.Creature,
            this
        );
    }
}
