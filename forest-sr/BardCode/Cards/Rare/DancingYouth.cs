using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Forest_Sr.BardCode.Cards.Rare;

/// <summary>
/// 舞动青春｜DancingYouth
/// 效果：能力。回合结束时若和声大于3，下回合抽1张牌。
/// 升级：费用 1→0
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class DancingYouth : BardCard
{
    // 无动态变量
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    public DancingYouth() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
    }

    // 升级：费用 1 → 0
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 施加能力Power，在每回合结束时触发
        await PowerCmd.Apply<DancingYouthPower>(choiceContext,
            Owner.Creature,
            1,  // 层数（用于标记）
            Owner.Creature,
            this
        );
    }
}

