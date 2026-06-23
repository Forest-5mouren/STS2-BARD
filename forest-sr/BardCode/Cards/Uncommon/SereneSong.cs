using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Forest_Sr.BardCode.Cards.Uncommon;

/// <summary>
/// 宁静之歌｜SereneSong
/// 效果：吟唱。下回合开始时，全体队友回复 {heal} 点生命。乐曲。
/// 升级：回复 8→11
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class SereneSong : BardCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new HealVar(8)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [BardKeywords.Song, BardKeywords.Chant];

    public SereneSong() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        var chant = await PowerCmd.Apply<SereneSongChant>(ctx, Owner.Creature, 1, Owner.Creature, this);
        chant!.HealAmount = DynamicVars.Heal.IntValue;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Heal.UpgradeValueBy(3);
    }
}


