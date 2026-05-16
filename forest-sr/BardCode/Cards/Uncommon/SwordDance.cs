using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Cards.Uncommon;

/// <summary>
/// 剑舞｜Sword Dance
/// 效果：造成 {damage} 点伤害。
/// 如果上一张打出的牌是攻击牌，改为攻击2次。
/// 如果上一张打出的牌是技能牌，获得 {block} 点格挡。
/// 升级：伤害 8→10，格挡 4→6
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class SwordDance : BardCard
{
    
    private const string _blockKey = "block";

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar( 8, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),   // 基础伤害
        new BlockVar( 4, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move)     // 格挡值
    ];

    public SwordDance() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
    }

    // 发金光条件：上一张是攻击牌或技能牌（有额外效果）
    protected override bool ShouldGlowGoldInternal => WasLastCardPlayedAttack || WasLastCardPlayedSkill;

    // 判断上一张打出的牌是否是攻击牌
    private bool WasLastCardPlayedAttack
    {
        get
        {
            if (CombatState == null) return false;

            CardPlayStartedEntry entry = CombatManager.Instance.History
                .CardPlaysStarted
                .LastOrDefault(e => e.CardPlay.Card.Owner == Owner
                    && e.HappenedThisTurn(CombatState)
                    && e.CardPlay.Card != this);
            return entry?.CardPlay.Card.Type == CardType.Attack;
        }
    }

    // 判断上一张打出的牌是否是技能牌
    private bool WasLastCardPlayedSkill
    {
        get
        {
            if (CombatState == null) return false;

            CardPlayStartedEntry entry = CombatManager.Instance.History
                .CardPlaysStarted
                .LastOrDefault(e => e.CardPlay.Card.Owner == Owner
                    && e.HappenedThisTurn(CombatState)
                    && e.CardPlay.Card != this);
            return entry?.CardPlay.Card.Type == CardType.Skill;
        }
    }

    // 升级：伤害 8→10，格挡 4→6
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
        DynamicVars.Block.UpgradeValueBy(2);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        // 判断上一张牌的类型
        bool isAttack = WasLastCardPlayedAttack;
        bool isSkill = WasLastCardPlayedSkill;

        

        // 执行攻击
        if (isAttack)
        {
            // 上一张是攻击牌：攻击2次
            await DamageCmd.Attack(DynamicVars.Damage.IntValue)
                .WithHitCount(2)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        }
        else
        {
            // 上一张不是攻击牌：攻击1次
            await DamageCmd.Attack(DynamicVars.Damage.IntValue)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        }

        // 如果上一张是技能牌，获得格挡
        if (isSkill)
        {
            
            await CreatureCmd.GainBlock(Owner.Creature, new BlockVar(DynamicVars.Block.IntValue, ValueProp.Move), cardPlay);
        }
    }
}