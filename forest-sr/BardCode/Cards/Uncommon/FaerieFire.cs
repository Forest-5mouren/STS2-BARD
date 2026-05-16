using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Cards.Uncommon;

/// <summary>
/// 妖火｜FaerieFire
/// 效果：对所有敌人造成 {vulnerable} 层易伤。
/// 升级：易伤 3 → 5
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class FaerieFire : BardCard
{
    

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<VulnerablePower>( 3)   // 易伤层数
    ];

    // 关键词
    protected override IEnumerable<string> RegisteredKeywordIds => [
        "EXHAUST",
        BardKeywords.Magic
    ];

    // 额外悬停提示
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<VulnerablePower>()
    ];

    public FaerieFire() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies)
    {
    }

    // 升级：易伤 3 → 5
    protected override void OnUpgrade()
    {
        DynamicVars.Vulnerable.UpgradeValueBy(2);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

        int vulnerableAmount = DynamicVars.Vulnerable.IntValue;

        foreach (Creature enemy in CombatState.HittableEnemies)
        {
            await PowerCmd.Apply<VulnerablePower>(
                enemy,
                DynamicVars.Vulnerable.BaseValue,
                Owner.Creature,
                this
            );
        }
    }
}