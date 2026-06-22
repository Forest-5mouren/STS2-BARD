using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Forest_Sr.BardCode.Cards.Uncommon;

/// <summary>
/// 战歌｜WarSong
/// 效果：能力。每当使用乐曲卡时，获得 {vigor} 点活力。
/// 升级：费用 1 → 0
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class WarSong : BardCard
{

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<VigorPower>( 3)   // 活力层数
    ];

    // 额外悬停提示
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<VigorPower>()
    ];

    public WarSong() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
    }

    // 升级：费用 1 → 0
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {


        // 施加战歌能力Power
        await PowerCmd.Apply<WarSongPower>(choiceContext,
            Owner.Creature,
            DynamicVars["VigorPower"].IntValue,
            Owner.Creature,
            this
        );
    }
}
