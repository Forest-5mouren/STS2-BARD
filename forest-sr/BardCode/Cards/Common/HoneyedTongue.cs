using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Forest_Sr.BardCode.Cards.Common;

/// <summary>
/// 巧舌如簧｜Honeyed Tongue
/// 效果：获得4点格挡，给予所有敌人1层易伤。
/// 如果上一张打出的牌是技能牌，改为给予所有敌人1层虚弱。
/// 升级：格挡+3（4→7），易伤/虚弱层数+1（1→2）
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class HoneyedTongue : BardCard
{
    private const int energyCost = 1;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Common;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;

    // 基础数值：格挡值 + 易伤层数 + 虚弱层数
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(4,ValueProp.Move),
        new PowerVar<VulnerablePower>(1),
        new PowerVar<WeakPower>(1)
    ];

    // 判断上一张打出的牌是否是技能牌
    private bool WasLastCardPlayedSkill
    {
        get
        {
            CardPlayStartedEntry entry = CombatManager.Instance.History
                .CardPlaysStarted
                .LastOrDefault(e => e.CardPlay.Card.Owner == Owner
                                    && e.HappenedThisTurn(CombatState)
                                    && e.CardPlay.Card != this);
            return entry?.CardPlay.Card.Type == CardType.Skill;
        }
    }

    // 手牌高亮条件
    protected override bool ShouldGlowGoldInternal => WasLastCardPlayedSkill;

    public HoneyedTongue() : base(energyCost, type, rarity, targetType)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {

        // 获得格挡
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block.IntValue, ValueProp.Move, cardPlay);

        // 判断上一张是否是技能牌
        if (WasLastCardPlayedSkill)
        {
            // 改为给予所有敌人虚弱
            await PowerCmd.Apply<WeakPower>(choiceContext,
                CombatState.HittableEnemies,
                DynamicVars.Weak.BaseValue,
                Owner.Creature,
                this);
        }
        else
        {
            // 基础效果：给予所有敌人易伤
            await PowerCmd.Apply<VulnerablePower>(choiceContext,
                CombatState.HittableEnemies,
                DynamicVars.Vulnerable.BaseValue,
                Owner.Creature,
                this);
        }
    }

    protected override void OnUpgrade()
    {
        // 格挡 +3（4 → 7）
        DynamicVars.Block.UpgradeValueBy(3);
        // 可选：易伤/虚弱层数 +1（1 → 2）
        DynamicVars.Vulnerable.UpgradeValueBy(1);
        DynamicVars.Weak.UpgradeValueBy(1);
    }
}
