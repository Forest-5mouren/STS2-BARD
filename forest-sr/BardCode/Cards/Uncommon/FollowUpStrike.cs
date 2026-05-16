using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Cards.Uncommon;

/// <summary>
/// 顺势戳击｜Follow-Up Strike
/// 效果：造成 {damage} 点伤害。如果上一张打出的牌是技能牌，此卡回到手牌。
/// 升级：伤害 4 → 6
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class FollowUpStrike : BardCard
{
    

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar( 4,MegaCrit.Sts2.Core.ValueProps.ValueProp.Move)
    ];

    public FollowUpStrike() : base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
    }

    // 卡牌发光条件
    protected override bool ShouldGlowGoldInternal => WasLastCardPlayedSkill;

    // 判断上一张打出的牌是否是技能牌
    private bool WasLastCardPlayedSkill
    {
        get
        {
            var entry = CombatManager.Instance.History
                .CardPlaysStarted
                .LastOrDefault(e => e.CardPlay.Card.Owner == Owner
                    && e.HappenedThisTurn(CombatState));
            return entry?.CardPlay.Card.Type == CardType.Skill;
        }
    }

    // 升级：伤害 4 → 6
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        // 播放戳击特效
        NCombatRoom.Instance?.PlaySplashVfx(cardPlay.Target, new Color("#2ECC71"));

        // 造成伤害
        
        await DamageCmd.Attack(DynamicVars.Damage.IntValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }

    // 修改卡牌打出后的去向
    public override (PileType, CardPilePosition) ModifyCardPlayResultPileTypeAndPosition(
        CardModel card, bool isAutoPlay, ResourceInfo resources, PileType pileType, CardPilePosition position)
    {
        // 如果不是当前卡牌，使用默认行为
        if (card != this) return (pileType, position);

        // 如果上一张是技能牌，回到手牌顶部
        if (WasLastCardPlayedSkill)
        {
            return (PileType.Hand, CardPilePosition.Top);
        }

        // 否则正常进入弃牌堆
        return (pileType, position);
    }
}