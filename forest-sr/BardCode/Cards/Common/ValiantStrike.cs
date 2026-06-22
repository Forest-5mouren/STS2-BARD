using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Forest_Sr.BardCode.Cards.Common;

/// <summary>
/// 英勇打击｜ValiantStrike
/// 效果：造成8点伤害，获得2点活力。
/// 升级：伤害 8→10，活力 2→3
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class ValiantStrike : BardCard
{
    private const int energyCost = 1;
    private const CardType type = CardType.Attack;
    private const CardRarity rarity = CardRarity.Common;
    private const TargetType targetType = TargetType.AnyEnemy;
    private const bool shouldShowInCardLibrary = true;

    // 基础数值：伤害 + 活力层数
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar( 8, ValueProp.Move),
        new PowerVar<VigorPower>(2)
    ];

    // 额外提示文本
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<VigorPower>()
    ];

    public ValiantStrike() : base(energyCost, type, rarity, targetType)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");


        // 1. 造成伤害
        await CreatureCmd.Damage(
            choiceContext,
            cardPlay.Target,
            DynamicVars.Damage.IntValue,
            ValueProp.Move,
            Owner.Creature,
            this);

        // 2. 获得活力
        await PowerCmd.Apply<VigorPower>(choiceContext,
            Owner.Creature,
            DynamicVars["VigorPower"].IntValue,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);  // 8 → 10
        DynamicVars["VigorPower"].UpgradeValueBy(1);   // 2 → 3
    }
}
