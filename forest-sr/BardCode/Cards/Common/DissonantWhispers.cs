using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forest_Sr.BardCode.Cards.KeyWord;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Forest_Sr.BardCode.Cards.Common;

/// <summary>
/// 不谐低语｜Dissonant Whispers
/// 效果：对单个敌人造成14/18点伤害，施加2/3层虚弱。魔法。
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class DissonantWhispers : BardCard
{
    private const int energyCost = 2;
    private const CardType type = CardType.Attack;
    private const CardRarity rarity = CardRarity.Common;
    private const TargetType targetType = TargetType.AnyEnemy;
    private const bool shouldShowInCardLibrary = true;

    // 基础数值：伤害 + 虚弱层数
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(14,ValueProp.Move),
        new PowerVar<WeakPower>(2)
    ];

    // 额外提示文本
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<WeakPower>()
    ];

    // 关键词
    public override IEnumerable<CardKeyword> CanonicalKeywords => [BardKeywords.Magic];

    public DissonantWhispers() : base(energyCost, type, rarity, targetType)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        // 播放不谐低语特效
        NCombatRoom.Instance?.PlaySplashVfx(cardPlay.Target, new Color("#8E44AD"));

        // 播放施法动画
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.AttackAnimDelay);

        // 造成伤害
        await CreatureCmd.Damage(
            choiceContext,
            cardPlay.Target,
            DynamicVars.Damage.IntValue,
            ValueProp.Move,
            Owner.Creature,
            this);

        // 施加虚弱
        await PowerCmd.Apply<WeakPower>(choiceContext, 
            cardPlay.Target,
            DynamicVars.Weak.BaseValue,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);   // 14 → 18
        DynamicVars.Weak.UpgradeValueBy(1);     // 2 → 3
    }
}
