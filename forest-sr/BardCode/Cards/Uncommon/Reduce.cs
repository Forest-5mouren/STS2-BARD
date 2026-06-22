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
using STS2RitsuLib.Interop.AutoRegistration;

namespace Forest_Sr.BardCode.Cards.Uncommon;

/// <summary>
/// 缩小术｜Reduce
/// 效果：使一个敌人获得缩小效果（造成的强化攻击伤害减少30%，持续 {duration} 回合）
/// 升级：持续时间 2 → 3
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class Reduce : BardCard
{
    private const string _durationKey = "duration";

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar(_durationKey, 2)   // 持续回合数
    ];

    // 关键词：魔法
    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        BardKeywords.Magic
    ];

    // 额外悬停提示
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.Static(StaticHoverTip.Fatal)
    ];

    public Reduce() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
    }

    // 升级：持续时间 2 → 3
    protected override void OnUpgrade()
    {
        DynamicVars[_durationKey].UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Creature target = cardPlay.Target;
        if (target == null) return;

        // 播放绿色特效
        NCombatRoom.Instance?.PlaySplashVfx(target, new Color("65cf81"));
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

        // 施加缩小能力
        int duration = DynamicVars[_durationKey].IntValue;
        await PowerCmd.Apply<ShrinkPower>(choiceContext,
            target,
            duration,
            Owner.Creature,
            this
        );
    }
}
