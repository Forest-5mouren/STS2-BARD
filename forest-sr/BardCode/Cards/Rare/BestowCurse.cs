using Forest_Sr.BardCode.Cards.KeyWord;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Forest_Sr.BardCode.Cards.Uncommon;

/// <summary>
/// 降咒｜Bestow Curse
/// 效果：消耗X点能量，造成 {damage} 点伤害X次，施加 {weak} 层虚弱，减少 {strengthLoss} 层临时力量。消耗。
/// 升级：伤害 8 → 10
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class BestowCurse : BardCard
{

    private const string _strengthLossKey = "strengthLoss";

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar( 8,ValueProp.Move),        // 基础伤害（每X）
        new PowerVar<WeakPower>( 1),          // 基础虚弱层数（每X）
        new DynamicVar(_strengthLossKey, 1)   // 基础减力量层数（每X）
    ];

    // 关键词：魔法
    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        BardKeywords.Magic
    ];

    // 额外悬停提示
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<WeakPower>(),
        HoverTipFactory.FromPower<StrengthPower>()
    ];

    // X费卡牌
    protected override bool HasEnergyCostX => true;

    public BestowCurse() : base(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
    }

    // 升级：基础伤害 8 → 10
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
        // 虚弱层数不变（1 → 1）
        // StrengthLoss 不变（1 → 1）
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        // 获取 X 值（消耗的能量）
        int xValue = ResolveEnergyXValue();

        if (xValue <= 0) return;

        // 计算伤害和层数

        int weakAmount = DynamicVars.Weak.IntValue * xValue;
        int strengthLoss = DynamicVars[_strengthLossKey].IntValue * xValue;

        // 播放诅咒特效
        NCombatRoom.Instance?.PlaySplashVfx(cardPlay.Target, new Color("#8E44AD"));

        // 播放施法动画
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.AttackAnimDelay);

        // 造成伤害
        await DamageCmd.Attack(DynamicVars.Damage.IntValue)
            .WithHitCount(xValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        // 施加虚弱
        await PowerCmd.Apply<WeakPower>(choiceContext,
            cardPlay.Target,
            DynamicVars.Weak.BaseValue,
            Owner.Creature,
            this
        );

        // 减少临时力量
        await PowerCmd.Apply<CrushUnderPower>(choiceContext,
            cardPlay.Target,
            strengthLoss,
            Owner.Creature,
            this
        );
    }
}
