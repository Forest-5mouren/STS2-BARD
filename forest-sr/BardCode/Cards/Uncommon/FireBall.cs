using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Cards.Uncommon;

/// <summary>
/// 火球术｜Fireball
/// 效果：对所有敌人造成 {damage} 点伤害。
/// 升级：伤害 27 → 33
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class Fireball : BardCard
{
    

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar( 27,ValueProp.Move)
    ];
    protected override IEnumerable<string> RegisteredKeywordIds => [
        BardKeywords.Magic
    ];

    public Fireball() : base(3, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
    {
    }

    // 升级：伤害 27 → 33
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(6);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        

        await DamageCmd.Attack(DynamicVars.Damage.IntValue)
            .FromCard(this)
            .TargetingAllOpponents(CombatState)
            .Execute(choiceContext);
    }
}