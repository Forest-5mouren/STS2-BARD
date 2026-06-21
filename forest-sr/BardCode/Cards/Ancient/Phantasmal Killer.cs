using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Forest_Sr.BardCode.Cards.Ancient;

/// <summary>
/// 幻影杀手｜Phantasmal Killer
/// 效果：造成 {damage} 点无法格挡、无法被能力影响的伤害，并施加 {weak} 层虚弱。
/// 升级：伤害 22 → 28
/// 打出后，下一个回合开始时回到你的手牌（通过能力实现）
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class PhantasmalKiller : BardCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(22, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move),
        new PowerVar<WeakPower>(1)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [BardKeywords.Magic];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<WeakPower>()
    ];

    public PhantasmalKiller() : base(0, CardType.Skill, CardRarity.Ancient, TargetType.AnyEnemy) { }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(6);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        // 造成伤害（无法格挡、无法被能力影响）
        await CreatureCmd.Damage(
            ctx,
            cardPlay.Target,
            DynamicVars.Damage.IntValue,
            ValueProp.Unblockable | ValueProp.Unpowered,
            Owner.Creature,
            this
        );

        // 施加虚弱
        await PowerCmd.Apply<WeakPower>(ctx, 
            cardPlay.Target,
            DynamicVars.Weak.BaseValue,
            Owner.Creature,
            this
        );

        // 施加"下回合返回手牌"效果
        await PowerCmd.Apply<PhantasmalKillerReturnPower>(ctx, 
            Owner.Creature,
            1,
            Owner.Creature,
            this
        );
    }
}

