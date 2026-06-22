using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Forest_Sr.BardCode.Cards.Uncommon;

/// <summary>
/// 野蜂飞翔｜FlightOfTheBumblebee
/// 效果：随机造成 {damage} 点伤害 {repeat} 次。
/// 升级：攻击次数 4 → 5
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class FlightOfTheBumblebee : BardCard
{
    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar( 2,ValueProp.Move),   // 单次伤害
        new RepeatVar(4)    // 攻击次数
    ];

    // 关键词：乐曲
    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        BardKeywords.Song
    ];


    public FlightOfTheBumblebee() : base(1, CardType.Attack, CardRarity.Common, TargetType.RandomEnemy)
    {
    }

    // 升级：攻击次数 4 → 5
    protected override void OnUpgrade()
    {
        DynamicVars.Repeat.UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {

        int repeatCount = DynamicVars.Repeat.IntValue;

        await DamageCmd.Attack(DynamicVars.Damage.IntValue)
            .WithHitCount(repeatCount)
            .FromCard(this)
            .TargetingRandomOpponents(CombatState)
            .Execute(choiceContext);
    }
}