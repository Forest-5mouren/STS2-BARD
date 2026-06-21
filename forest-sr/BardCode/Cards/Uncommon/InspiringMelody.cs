using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Cards.Uncommon;

/// <summary>
/// 激昂旋律｜InspiringMelody
/// 效果：吟唱。下回合开始时，全体队友获得 {strength} 层力量，{dexterity} 层敏捷。乐曲。
/// 升级：力量 1→2，敏捷 1→2
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class InspiringMelody : BardCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<StrengthPower>(1),
        new PowerVar<DexterityPower>(1)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [BardKeywords.Song, BardKeywords.Chant];

    public InspiringMelody() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        var chant = await PowerCmd.Apply<InspiringMelodyChant>(ctx, Owner.Creature, 1, Owner.Creature, this);
        chant.StrengthAmount = DynamicVars["StrengthPower"].IntValue;
        chant.DexterityAmount = DynamicVars["DexterityPower"].IntValue;
    }

    protected override void OnUpgrade()
    {
        DynamicVars["StrengthPower"].UpgradeValueBy(1);
        DynamicVars["DexterityPower"].UpgradeValueBy(1);
    }
}

