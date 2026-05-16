using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Forest_Sr.BardCode.Cards.Common;

/// <summary>
/// 弱点打击｜WeakpointStrike
/// 效果：造成3点伤害，施加1层易伤。
/// 升级：伤害 3→4，易伤 1→2
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class WeakpointStrike : BardCard
{
    private const int energyCost = 0;
    private const CardType type = CardType.Attack;
    private const CardRarity rarity = CardRarity.Common;
    private const TargetType targetType = TargetType.AnyEnemy;
    private const bool shouldShowInCardLibrary = true;

    // 基础数值：伤害 + 易伤层数
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar( 3, ValueProp.Move),
        new PowerVar<VulnerablePower>( 1)
    ];

    // 额外提示文本
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<VulnerablePower>()
    ];

    public WeakpointStrike() : base(energyCost, type, rarity, targetType)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");


        // 造成伤害
        await CreatureCmd.Damage(
            choiceContext,
            cardPlay.Target,
            DynamicVars.Damage.IntValue,
            ValueProp.Move,
            Owner.Creature,
            this);

        // 施加易伤
        await PowerCmd.Apply<VulnerablePower>(
            cardPlay.Target,
            DynamicVars.Vulnerable.BaseValue,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1);      // 3 → 4
        DynamicVars.Vulnerable.UpgradeValueBy(1);  // 1 → 2
    }
}