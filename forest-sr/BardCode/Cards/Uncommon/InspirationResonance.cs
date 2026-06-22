using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Forest_Sr.BardCode.Cards.Uncommon;

/// <summary>
/// 灵感共鸣｜InspirationResonance
/// 效果：虚无。消耗。本回合内，每打出一张乐曲牌，回复 {energy} 点能量。
/// 升级：回复能量 1→2，失去虚无。
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class InspirationResonance : BardCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new EnergyVar(1)
    ];

    public override List<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, CardKeyword.Ethereal];

    public InspirationResonance() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        // 施加灵感共鸣能力（本回合内打乐曲回复能量）
        await PowerCmd.Apply<InspirationResonancePower>(ctx,
            Owner.Creature,
            DynamicVars.Energy.IntValue,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Energy.UpgradeValueBy(1);
        RemoveKeyword(CardKeyword.Ethereal);
    }
}

