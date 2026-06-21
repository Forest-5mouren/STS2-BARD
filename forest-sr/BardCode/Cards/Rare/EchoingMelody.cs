using System.Collections.Generic;
using System.Threading.Tasks;
using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Forest_Sr.BardCode.Cards.Rare;

/// <summary>
/// 余音绕梁｜EchoingMelody
/// 效果：给予自己 {weak} 层虚弱。每回合开始时，打出上回合最后一张乐曲卡的带消耗复制。
/// 升级：虚弱 2 → 1 层
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class EchoingMelody : BardCard
{

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<WeakPower>( 2)   // 虚弱层数
    ];

    // 额外悬停提示
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<WeakPower>()
    ];

    public EchoingMelody() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
    }

    // 升级：虚弱 2 → 1 层
    protected override void OnUpgrade()
    {
        DynamicVars.Weak.UpgradeValueBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int weakAmount = DynamicVars.Weak.IntValue;

        // 1. 给予自己虚弱
        await PowerCmd.Apply<WeakPower>(choiceContext, 
            Owner.Creature,
            DynamicVars.Weak.BaseValue,
            Owner.Creature,
            this
        );

        // 2. 施加能力Power
        await PowerCmd.Apply<EchoingMelodyPower>(choiceContext, 
            Owner.Creature,
            1,
            Owner.Creature,
            this
        );
    }
}
