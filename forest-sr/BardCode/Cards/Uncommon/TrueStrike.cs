using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using System;
namespace Forest_Sr.BardCode.Cards.Uncommon;
/// <summary>
/// 克敌机先｜TrueStrike
/// 效果：移除敌人的人工制品，然后施加 {vulnerable} 层易伤。
/// 升级：易伤 2 → 3
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class TrueStrike : BardCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<VulnerablePower>(2)   // 易伤层数
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [BardKeywords.Magic,CardKeyword.Exhaust];

    public TrueStrike() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy) { }
    // 额外悬停提示
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<VulnerablePower>()
    ];
    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        // 如果目标有人工制品，移除它
        if (cardPlay.Target.HasPower<ArtifactPower>())
            if (cardPlay.Target != null)
            {
                await PowerCmd.Remove<ArtifactPower>(cardPlay.Target);
            }
        // 施加易伤
        await PowerCmd.Apply<VulnerablePower>(ctx, cardPlay.Target!, DynamicVars.Vulnerable.BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Vulnerable.UpgradeValueBy(1);
    }
}
