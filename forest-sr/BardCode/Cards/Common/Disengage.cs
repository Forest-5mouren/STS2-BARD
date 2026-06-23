using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Forest_Sr.BardCode.Cards.Common;

/// <summary>
/// 脱离｜Disengage
/// 效果：获得6点格挡。
/// 如果上一张打出的牌是攻击牌，抽2张牌。
/// 如果上一张打出的牌是技能牌，再获得6点格挡。
/// 升级：格挡+2（6→8），连击格挡+2（6→8）
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class Disengage : BardCard
{
    private const int energyCost = 1;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Common;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;

    // 基础数值：格挡值
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(6,ValueProp.Move),
        new CardsVar(2)
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

    public Disengage() : base(energyCost, type, rarity, targetType)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        bool isAttack = WasLastCardPlayedAttack;
        bool isSkill = WasLastCardPlayedSkill;

        // 获得基础格挡
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block.IntValue, ValueProp.Move, cardPlay);

        // 根据上一张牌类型触发额外效果
        if (isAttack)
        {
            // 上一张是攻击牌：抽2张牌
            await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.IntValue, Owner);
        }
        else if (isSkill)
        {
            // 上一张是技能牌：再获得6点格挡
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block.IntValue, ValueProp.Move, cardPlay);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2);
    }
}