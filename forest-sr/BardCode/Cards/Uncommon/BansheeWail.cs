using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Audio;

namespace Forest_Sr.BardCode.Cards.Uncommon;

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
        chant!.WeakAmount = DynamicVars.Weak.IntValue;
        chant.VulnerableAmount = DynamicVars.Vulnerable.IntValue;
                SfxCmd.Play("event:/Bard/sfx/BansheeWail");
    }

 protected override void OnUpgrade()
    {
        DynamicVars.Weak.UpgradeValueBy(1);
        DynamicVars.Vulnerable.UpgradeValueBy(1);
    }
}



