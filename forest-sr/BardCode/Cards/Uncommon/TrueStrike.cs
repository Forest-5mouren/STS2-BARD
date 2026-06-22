using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
namespace Forest_Sr.BardCode.Cards.Uncommon;
/// <summary>
/// 克敌机先｜TrueStrike
/// 效果：造成 {damage} 点伤害。如果目标拥有负面效果，获得 1 层易伤并额外造成 {bonusDamage} 点伤害。
/// 升级：伤害 6→9，额外伤害 6→9
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class TrueStrike : BardCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(6, ValueProp.Move),
        new DynamicVar("bonusDamage", 6),
        new PowerVar<VulnerablePower>(1)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [BardKeywords.Magic];

    public TrueStrike() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.IntValue)
            .FromCard(this)
            .Targeting(cardPlay.Target!)
            .Execute(ctx);

        if (cardPlay.Target != null)
        {
            bool hasDebuff = cardPlay.Target.Powers.Any(p => p.Type == PowerType.Debuff);
            if (hasDebuff)
            {
                await PowerCmd.Apply<VulnerablePower>(ctx, cardPlay.Target, DynamicVars["VulnerablePower"].IntValue, Owner.Creature, this);

                await DamageCmd.Attack(DynamicVars["bonusDamage"].IntValue)
                    .FromCard(this)
                    .Targeting(cardPlay.Target)
                    .Execute(ctx);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
        DynamicVars["bonusDamage"].UpgradeValueBy(3);
    }
}
