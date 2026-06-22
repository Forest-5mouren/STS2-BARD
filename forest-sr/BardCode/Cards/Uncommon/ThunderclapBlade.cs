using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
namespace Forest_Sr.BardCode.Cards.Uncommon;
/// <summary>
/// 鸣雷剑｜ThunderclapBlade
/// 效果：造成 {damage} 点伤害，施加 2 层易伤。攻击，法术。
/// 升级：伤害 12→16
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class ThunderclapBlade : BardCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(12, ValueProp.Move),
        new PowerVar<VulnerablePower>(2)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [BardKeywords.Magic];

    public ThunderclapBlade() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.IntValue)
            .FromCard(this)
            .Targeting(cardPlay.Target!)
            .Execute(ctx);

        if (cardPlay.Target != null)
        {
            await PowerCmd.Apply<VulnerablePower>(ctx, cardPlay.Target, DynamicVars["VulnerablePower"].IntValue, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);
    }
}
