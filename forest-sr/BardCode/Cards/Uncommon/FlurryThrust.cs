using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Forest_Sr.BardCode.Cards.Common;

/// <summary>
/// 连续突刺｜FlurryThrust
/// 效果：造成 {damage} 点伤害。
/// 如果本回合打出过攻击牌，额外造成 {damage} 点伤害。
/// 如果本回合打出过技能牌，额外造成 {damage} 点伤害。
/// 升级：伤害 6 → 8
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class FlurryThrust : BardCard
{


    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar( 6,MegaCrit.Sts2.Core.ValueProps.ValueProp.Move)
    ];

    public FlurryThrust() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
    }

    // 判断本回合是否打出过攻击牌（不包括自己）
    private bool HasPlayedAttackThisTurn
    {
        get
        {
            return CombatManager.Instance.History
                .CardPlaysStarted
                .Any(e => e.CardPlay.Card.Owner == Owner
                    && e.HappenedThisTurn(CombatState)
                    && e.CardPlay.Card != this
                    && e.CardPlay.Card.Type == CardType.Attack);
        }
    }

    // 判断本回合是否打出过技能牌
    private bool HasPlayedSkillThisTurn
    {
        get
        {
            return CombatManager.Instance.History
                .CardPlaysStarted
                .Any(e => e.CardPlay.Card.Owner == Owner
                    && e.HappenedThisTurn(CombatState)
                    && e.CardPlay.Card != this
                    && e.CardPlay.Card.Type == CardType.Skill);
        }
    }

    // 卡牌发光条件
    protected override bool ShouldGlowGoldInternal => HasPlayedAttackThisTurn || HasPlayedSkillThisTurn;

    // 升级：伤害 6 → 8
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        int baseDamage = DynamicVars.Damage.IntValue;
        int hitCount = 1;

        // 如果本回合打出过攻击牌，额外伤害
        if (HasPlayedAttackThisTurn)
        {
            hitCount++;
        }

        // 如果本回合打出过技能牌，额外伤害
        if (HasPlayedSkillThisTurn)
        {
            hitCount++;
        }

        // 造成伤害
        await DamageCmd.Attack(baseDamage)
            .WithHitCount(hitCount)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }
}
