using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forest_Sr.BardCode.Cards.KeyWord;
namespace Forest_Sr.BardCode.Cards.Uncommon;
/// <summary>
/// 刺耳尖叫｜EarPiercingShriek
/// 效果：所有敌人本回合降低 {strReduction} 点力量，{dexReduction} 点敏捷。消耗。乐曲。
/// 升级：降低值 3→5
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class EarPiercingShriek : BardCard{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("strReduction", 3),
        new DynamicVar("dexReduction", 3)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [BardKeywords.Song, CardKeyword.Exhaust];

    public EarPiercingShriek() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies) { }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        int strReduction = DynamicVars["strReduction"].IntValue;
        int dexReduction = DynamicVars["dexReduction"].IntValue;
        foreach (var enemy in CombatState.HittableEnemies)
        {
            await PowerCmd.Apply<TemporaryStrengthPower>(ctx, enemy, -strReduction, Owner.Creature, this);
            await PowerCmd.Apply<TemporaryDexterityPower>(ctx, enemy, -dexReduction, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["strReduction"].UpgradeValueBy(2);
        DynamicVars["dexReduction"].UpgradeValueBy(2);
    }
}
