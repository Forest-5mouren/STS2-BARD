using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Forest_Sr.BardCode.Cards.Common;

/// <summary>
/// 勇气之歌｜CourageSong
/// 效果：吟唱。下回合开始时获得 1 层力量，2 层活力。乐曲。
/// 升级：力量 1→2，活力 2→3
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class CourageSong : BardCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<StrengthPower>(1),
        new PowerVar<VigorPower>(2)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [BardKeywords.Song, BardKeywords.Chant];

    public CourageSong() : base(0, CardType.Skill, CardRarity.Common, TargetType.Self) { }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        var chant = await PowerCmd.Apply<CourageSongChant>(ctx, Owner.Creature, 1, Owner.Creature, this);
        chant.StrengthAmount = DynamicVars["StrengthPower"].IntValue;
        chant.VigorAmount = DynamicVars["VigorPower"].IntValue;
    }

    protected override void OnUpgrade()
    {
        DynamicVars["StrengthPower"].UpgradeValueBy(1);
        DynamicVars["VigorPower"].UpgradeValueBy(1);
    }
}


