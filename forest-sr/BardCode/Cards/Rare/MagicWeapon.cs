using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Forest_Sr.BardCode.Cards.Common;

/// <summary>
/// 魔法武器｜MagicWeapon
/// 效果：获得 {strength} 点力量。你的所有非魔法攻击牌获得魔法词条。
/// 升级：力量 1 → 2
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class MagicWeapon : BardCard
{
    private const string _markerKey = "marker";

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar(_markerKey, 1),      // 能力标记
        new PowerVar<StrengthPower>( 1)     // 力量层数
    ];

    // 额外悬停提示
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<StrengthPower>()
    ];

    public MagicWeapon() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
    }

    // 升级：力量 1 → 2
    protected override void OnUpgrade()
    {
        DynamicVars["StrengthPower"].UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 播放特效
        NPowerUpVfx.CreateNormal(Owner.Creature);
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

        int strengthAmount = DynamicVars["StrengthPower"].IntValue;

        // 1. 获得力量
        await PowerCmd.Apply<StrengthPower>(choiceContext,
            Owner.Creature,
            strengthAmount,
            Owner.Creature,
            this
        );

        // 2. 施加魔法武器能力Power
        int markerAmount = DynamicVars[_markerKey].IntValue;
        await PowerCmd.Apply<MagicWeaponPower>(choiceContext,
            Owner.Creature,
            markerAmount,
            Owner.Creature,
            this
        );
    }
}
