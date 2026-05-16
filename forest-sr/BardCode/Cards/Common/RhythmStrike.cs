using System;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
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
/// 韵律打击｜RhythmStrike
/// 效果：造成8点伤害。
/// 如果上一张打出的牌是攻击牌，获得1点能量。
/// 如果上一张打出的牌是技能牌，抽1张牌。
/// 升级：伤害+1（8→9），能量+1（1→2），抽牌+1（1→2）
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class RhythmStrike : BardCard
{
    private const int energyCost = 1;
    private const CardType type = CardType.Attack;
    private const CardRarity rarity = CardRarity.Common;
    private const TargetType targetType = TargetType.AnyEnemy;
    private const bool shouldShowInCardLibrary = true;

    // 卡牌标签（打击）
    protected override IEnumerable<string> RegisteredCardTagIds => new[] { "strike" };

    // 基础数值：伤害 + 能量 + 抽牌
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar( 8, ValueProp.Move),
        new EnergyVar(1),
        new CardsVar( 1),
    ];

    // 判断上一张打出的牌是否是攻击牌
    private bool WasLastCardPlayedAttack
    {
        get
        {
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
            CardPlayStartedEntry entry = CombatManager.Instance.History
                .CardPlaysStarted
                .LastOrDefault(e => e.CardPlay.Card.Owner == Owner
                && e.HappenedThisTurn(CombatState)
                && e.CardPlay.Card != this);
            return entry?.CardPlay.Card.Type == CardType.Skill;
        }
    }

    // 手牌高亮条件
    protected override bool ShouldGlowGoldInternal => WasLastCardPlayedAttack || WasLastCardPlayedSkill;

    public RhythmStrike() : base(energyCost, type, rarity, targetType)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");


        bool isAttack = WasLastCardPlayedAttack;
        bool isSkill = WasLastCardPlayedSkill;

        // 造成伤害
        await CreatureCmd.Damage(
            choiceContext,
            cardPlay.Target,
            DynamicVars.Damage.IntValue,
            ValueProp.Move,
            Owner.Creature,
            this);

        // 根据上一张牌类型触发效果
        if (isAttack)
        {
            // 上一张是攻击牌：获得能量
            await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
        }
        else if (isSkill)
        {
            // 上一张是技能牌：抽牌
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1);   // 8 → 9
        DynamicVars.Energy.UpgradeValueBy(1);   // 1 → 2
        DynamicVars.Cards.UpgradeValueBy(1);    // 1 → 2
    }
}