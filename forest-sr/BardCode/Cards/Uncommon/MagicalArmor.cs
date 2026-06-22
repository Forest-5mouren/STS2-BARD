using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Forest_Sr.BardCode.Cards.Common;

/// <summary>
/// 魔法护甲｜MagicalArmor
/// 效果：获得 {plating} 层护甲（每次受到攻击时减少伤害）。
/// 升级：护甲 7 → 9
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class MagicalArmor : BardCard
{
    private const string _platingKey = "plating";

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar(_platingKey, 7)
    ];

    public MagicalArmor() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
    }

    // 升级：护甲 7 → 9
    protected override void OnUpgrade()
    {
        DynamicVars[_platingKey].UpgradeValueBy(2);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int platingAmount = DynamicVars[_platingKey].IntValue;

        await PowerCmd.Apply<PlatingPower>(choiceContext,
            Owner.Creature,
            platingAmount,
            Owner.Creature,
            this
        );
    }
}
