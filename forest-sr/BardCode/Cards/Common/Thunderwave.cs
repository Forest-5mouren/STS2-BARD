using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Powers;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Cards.Common;

/// <summary>
/// 鸣雷波｜Thunderwave
/// 效果：对所有敌人造成 4 点伤害，施加 1 层 CrushUnderPower。
/// 升级：伤害 +1（4 → 5），CrushUnderPower +1（1 → 2）
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class Thunderwave : BardCard
{
    private const int energyCost = 1;
    private const CardType type = CardType.Attack;
    private const CardRarity rarity = CardRarity.Common;
    private const TargetType targetType = TargetType.AllEnemies;
    private const bool shouldShowInCardLibrary = true;

    // 基础数值：伤害 + 力量损失层数
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar( 4, ValueProp.Move),
        ModCardVars.Int("strengthLoss", 1),
        new RepeatVar(2)
    ];

    // 关键词
    public override IEnumerable<CardKeyword> CanonicalKeywords => [BardKeywords.Magic];

    public Thunderwave() : base(energyCost, type, rarity, targetType)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int strengthLoss = DynamicVars["strengthLoss"].IntValue;
        IReadOnlyList<Creature> enemies = CombatState.HittableEnemies;

        // 播放闪电溅射特效
        foreach (Creature item in enemies)
        {
            NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NSpikeSplashVfx.Create(item));
        }

        // 对所有敌人造成伤害（2段攻击）
        await DamageCmd.Attack(DynamicVars.Damage.IntValue)
        .FromCard(this)
        .WithHitCount(DynamicVars.Repeat.IntValue)
        .TargetingAllOpponents(CombatState)
        .Execute(choiceContext);

        // 施加 CrushUnderPower 能力
        await PowerCmd.Apply<CrushUnderPower>(choiceContext, 
            enemies,
            strengthLoss,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1);           // 4 → 5
        DynamicVars["strengthLoss"].UpgradeValueBy(1);     // 1 → 2
    }
}
