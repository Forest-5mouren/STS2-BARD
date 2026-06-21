using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forest_Sr.BardCode;

namespace Forest_Sr.BardCode.Cards.Common;

/// <summary>
/// 韵律打击｜RhythmStrike
/// 效果：造成 {damage} 点伤害。如果和声大于3，获得 1 点能量，抽 {Cards:diff()} 张牌。
/// 升级：伤害 8→9，抽牌 1→2
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class RhythmStrike : BardCard
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [BardKeywords.Song];

    protected override HashSet<CardTag> CanonicalTags => new() { CardTag.Strike };

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(8, ValueProp.Move),
        new EnergyVar(1),
        new CardsVar(1)
    ];

    public RhythmStrike() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.IntValue)
            .FromCard(this)
            .Targeting(cardPlay.Target!)
            .Execute(ctx);

        if (HarmonyTracker.Count > 3)
        {
            await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
            await CardPileCmd.Draw(ctx, DynamicVars.Cards.IntValue, Owner);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1);
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}
