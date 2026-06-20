using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Cards.Uncommon;

/// <summary>
/// 虚弱尖叫｜WeaknessShriek
/// 效果：所有敌人本回合降低 {strReduction} 点力量，{dexReduction} 点敏捷。消耗。乐曲。
/// 升级：降低值 3→5
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class WeaknessShriek : BardCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("strReduction", 3),
        new DynamicVar("dexReduction", 3)
    ];

    protected override IEnumerable<string> RegisteredKeywordIds => [BardKeywords.Song];
    public override List<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    public WeaknessShriek() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies) { }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        foreach (var enemy in CombatState.HittableEnemies)
        {
            var power = await PowerCmd.Apply<WeaknessShriekPower>(enemy, 1, Owner.Creature, this);
            power.StrengthReduction = DynamicVars["strReduction"].IntValue;
            power.DexterityReduction = DynamicVars["dexReduction"].IntValue;
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["strReduction"].UpgradeValueBy(2);
        DynamicVars["dexReduction"].UpgradeValueBy(2);
    }
}
