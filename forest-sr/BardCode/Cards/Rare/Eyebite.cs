using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forest_Sr.BardCode.Cards.KeyWord;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Forest_Sr.BardCode.Cards.Rare;

/// <summary>
/// 摄心目光｜Eyebite
/// 效果：使一个敌人眩晕1回合，给予 {weak} 层虚弱，再减少 {strength} 点力量。
/// 升级：虚弱 2→3，减力量 2→3
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class Eyebite : BardCard
{

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<WeakPower>( 2),       // 虚弱层数
        new PowerVar<StrengthPower>( 2)    // 力量减少层数
    ];

    // 关键词：消耗
    public override List<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    // 额外悬停提示
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        StunIntent.GetStaticHoverTip(),
        HoverTipFactory.FromPower<WeakPower>(),
        HoverTipFactory.FromPower<StrengthPower>()
    ];

    public Eyebite() : base(3, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
    {
    }

    // 升级：虚弱 2→3，减力量 2→3
    protected override void OnUpgrade()
    {
        DynamicVars.Weak.UpgradeValueBy(1);
        DynamicVars["StrengthPower"].UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        // 1. 眩晕敌人
        await CreatureCmd.Stun(cardPlay.Target);

        // 2. 给予虚弱
        int weakAmount = DynamicVars.Weak.IntValue;
        await PowerCmd.Apply<WeakPower>(choiceContext, 
            cardPlay.Target,
            DynamicVars.Weak.BaseValue,
            Owner.Creature,
            this
        );

        // 3. 减少力量（给予负数的力量Power）
        int strengthAmount = DynamicVars["StrengthPower"].IntValue;
        await PowerCmd.Apply<StrengthPower>(choiceContext, 
            cardPlay.Target,
            -strengthAmount,  // 负数表示减少力量
            Owner.Creature,
            this
        );
    }
}
