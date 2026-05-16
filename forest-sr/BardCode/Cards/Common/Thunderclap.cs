using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Forest_Sr.BardCode.Cards.Common;

/// <summary>
/// 鸣雷破｜Thunderclap
/// 效果：对所有敌人造成 3 点伤害。
/// 升级：伤害 +3（3 → 6）
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class Thunderclap : BardCard
{
    private const int energyCost = 0;
    private const CardType type = CardType.Attack;
    private const CardRarity rarity = CardRarity.Common;
    private const TargetType targetType = TargetType.AllEnemies;
    private const bool shouldShowInCardLibrary = true;

    // 基础数值：伤害
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar( 3,ValueProp.Move)
    ];

    // 关键词
    protected override IEnumerable<string> RegisteredKeywordIds => [BardKeywords.Magic];

    public Thunderclap() : base(energyCost, type, rarity, targetType)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {

        // 对所有敌人造成伤害
        await CreatureCmd.Damage(
                choiceContext,
                base.CombatState.HittableEnemies,
                DynamicVars.Damage.IntValue,
                ValueProp.Move,
                Owner.Creature,
                this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);  // 3 → 6
    }
}