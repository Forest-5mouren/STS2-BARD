using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using Forest_Sr.BardCode.Cards.KeyWord;

namespace Forest_Sr.BardCode.Cards.Rare;

/// <summary>
/// 虹光喷射｜Prismatic Spray
/// 效果：对单个敌人攻击 {repeat} 次，每次造成 {damage} 点伤害。
/// 升级：每次伤害 2 → 3
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class PrismaticSpray : BardCard
{

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar( 2,ValueProp.Move),   // 每次伤害
        new RepeatVar( 8)    // 攻击次数
    ];

    // 关键词：魔法
    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        BardKeywords.Magic
    ];

    public PrismaticSpray() : base(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
    }

    // 升级：每次伤害 2 → 3
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1);
        // 攻击次数不变（8 → 8）
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        // 播放七彩特效
        NCombatRoom.Instance?.PlaySplashVfx(cardPlay.Target, new Color("#9B59B6"));

        // 播放施法动画
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.AttackAnimDelay);

        
        int repeatCount = DynamicVars.Repeat.IntValue;

        // 对目标执行多次攻击
        await DamageCmd.Attack(DynamicVars.Damage.IntValue)
            .WithHitCount(repeatCount)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        // 短暂延迟，让动画更清晰
        await Cmd.Wait(0.05f);
    }
}