using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Forest_Sr.BardCode.Cards.Uncommon;

/// <summary>
/// 尖锐刺耳｜EarPiercingShriek
/// 效果：吟唱。下回合开始时，对所有敌人造成 {Damage} 点伤害，给予 {WeakPower} 层虚弱。乐曲。
/// 升级：伤害 14→17，虚弱 1→2
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class EarPiercingShriek : BardCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(11, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),
        new PowerVar<WeakPower>(1)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [BardKeywords.Chant, BardKeywords.Song];

    public EarPiercingShriek() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        var chant = await PowerCmd.Apply<EarPiercingShriekChant>(ctx, Owner.Creature, 1, Owner.Creature, this);
        chant.DamageAmount = DynamicVars.Damage.IntValue;
        chant.WeakAmount = DynamicVars.Weak.IntValue;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
        DynamicVars.Weak.UpgradeValueBy(1);
    }
}
