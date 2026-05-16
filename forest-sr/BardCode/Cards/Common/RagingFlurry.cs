using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
/// 狂怒连击｜RagingFlurry
/// 效果：造成 5 点伤害，重复 2 次。
/// 升级：伤害 +2（5 → 7）
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class RagingFlurry : BardCard
{
    private const int energyCost = 1;
    private const CardType type = CardType.Attack;
    private const CardRarity rarity = CardRarity.Common;
    private const TargetType targetType = TargetType.AnyEnemy;
    private const bool shouldShowInCardLibrary = true;

    // 基础数值：伤害 + 攻击次数
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(5,ValueProp.Move),
        new RepeatVar(2)
    ];

    public RagingFlurry() : base(energyCost, type, rarity, targetType)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");


        // 使用 WithHitCount 实现多段攻击
        await DamageCmd.Attack(DynamicVars.Damage.IntValue)
            .WithHitCount(DynamicVars.Repeat.IntValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        //DynamicVars.Damage.UpgradeValueBy(2);  // 5 → 7
        DynamicVars.Repeat.UpgradeValueBy(1);
    }
}