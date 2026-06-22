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
/// 女妖之嚎｜BansheeWail
/// 效果：吟唱。下回合开始时，全体敌人获得 1 层虚弱，1 层易伤。消耗。乐曲。
/// 升级：虚弱 1→2，易伤 1→2
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class BansheeWail : BardCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<WeakPower>(1),
        new PowerVar<VulnerablePower>(1)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [BardKeywords.Song, BardKeywords.Chant, CardKeyword.Exhaust];

    public BansheeWail() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        var chant = await PowerCmd.Apply<BansheeWailChant>(ctx, Owner.Creature, 1, Owner.Creature, this);
        chant.WeakAmount = DynamicVars["WeakPower"].IntValue;
        chant.VulnerableAmount = DynamicVars["VulnerablePower"].IntValue;
    }

    protected override void OnUpgrade()
    {
        DynamicVars["WeakPower"].UpgradeValueBy(1);
        DynamicVars["VulnerablePower"].UpgradeValueBy(1);
    }
}
