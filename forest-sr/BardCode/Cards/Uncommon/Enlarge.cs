using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Forest_Sr.BardCode.Cards.Uncommon;
/// <summary>
/// 变巨术｜Enlarge
/// 效果：获得 {strength} 层力量和 {vigor} 层活力。法术。
/// 升级：力量 2→3，活力 3→4
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class Enlarge : BardCard{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<StrengthPower>(2),
        new PowerVar<VigorPower>(3)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [BardKeywords.Magic];

    public Enlarge() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await PowerCmd.Apply<StrengthPower>(ctx, Owner.Creature, DynamicVars["StrengthPower"].IntValue, Owner.Creature, this);
        await PowerCmd.Apply<VigorPower>(ctx, Owner.Creature, DynamicVars["VigorPower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["StrengthPower"].UpgradeValueBy(1);
        DynamicVars["VigorPower"].UpgradeValueBy(1);
    }
}
