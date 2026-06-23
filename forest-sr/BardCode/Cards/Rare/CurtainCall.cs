using Forest_Sr.BardCode.Powers.Counters;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Forest_Sr.BardCode.Cards.Rare;

/// <summary>
/// 收场｜CurtainCall
/// 效果：造成 {damage} 点伤害。本场战斗每使用过一种卡牌类型，就攻击一次。
/// 升级：基础伤害 6→8
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class CurtainCall : BardCard
{
    private const string _calculatedKey = "CalculatedHits";

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(8m, ValueProp.Move),           // 基础伤害
        new CalculationBaseVar(0m),                  // 计算基数
        new CalculationExtraVar(1m),                 // 额外乘数
        new CalculatedVar(_calculatedKey).WithMultiplier((card, _) =>
        {
            // 从隐藏计数器读取已使用的卡牌类型数
            var power = card.Owner.Creature?.GetPower<CardTypesUsedCounter>();
            if (power == null) return 1; // 至少当前这张牌自身
            // 如果当前卡牌的类型尚未被记录，需要 +1
            return power.RecordedTypes.Contains(card.Type)
                ? (int)power.Amount
                : (int)power.Amount + 1;
        })
    };

    public CurtainCall() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
    }

    // 升级：伤害 6 → 8
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        // 从隐藏计数器读取，包含当前卡牌自身
        var power = Owner.Creature.GetPower<CardTypesUsedCounter>();
        int hitCount = 1; // 至少当前这张牌
        if (power != null)
        {
            hitCount = power.RecordedTypes.Contains(cardPlay.Card.Type)
                ? (int)power.Amount
                : (int)power.Amount + 1;
        }

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .WithHitCount(hitCount)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
    }
}
