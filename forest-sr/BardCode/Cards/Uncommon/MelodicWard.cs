using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Forest_Sr.BardCode.Cards.Uncommon;

/// <summary>
/// 旋律护身｜Melodic Ward
/// 效果：每当你打出法术牌时，获得 {block} 点格挡。固有。
/// 升级：格挡 2 → 3
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class MelodicWard : BardCard
{
    private const string _blockKey = "block";

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar( 3,MegaCrit.Sts2.Core.ValueProps.ValueProp.Unpowered)
    ];

    // 额外悬停提示
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.Static(StaticHoverTip.Block)
    ];

    public MelodicWard() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(1);  // 3 → 4
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {


        // 施放旋律护身能力
        await PowerCmd.Apply<MelodicWardPower>(choiceContext,
            Owner.Creature,
            DynamicVars.Block.IntValue,
            Owner.Creature,
            this
        );
    }
}
