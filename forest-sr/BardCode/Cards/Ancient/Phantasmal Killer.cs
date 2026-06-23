using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Forest_Sr.BardCode.Cards.Ancient;

/// <summary>
/// 幻影杀手｜Phantasmal Killer
/// 效果：造成 {damage} 点无法格挡、无法被能力影响的伤害，并施加 {weak} 层虚弱。
/// 升级：伤害 22 → 28
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class PhantasmalKiller : BardCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(22, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move),
        new PowerVar<WeakPower>(1)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [BardKeywords.Magic];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<WeakPower>()
    ];

    public PhantasmalKiller() : base(1, CardType.Skill, CardRarity.Ancient, TargetType.AnyEnemy) { }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(6);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        // 造成伤害（无法格挡、无法被能力影响）
        await CreatureCmd.Damage(
            ctx,
            cardPlay.Target,
            DynamicVars.Damage.IntValue,
            ValueProp.Unblockable | ValueProp.Unpowered,
            Owner.Creature,
            this
        );

        // 施加虚弱
        await PowerCmd.Apply<WeakPower>(ctx,
            cardPlay.Target,
            DynamicVars.Weak.BaseValue,
            Owner.Creature,
            this
        );
    }
    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        // 如果上一回合打出了此卡，且此卡不在手牌中，则将其加入手牌
        if (player == Owner && (Pile == null || Pile.Type != PileType.Hand))
        {
            bool wasPlayedInCombat = CombatManager.Instance.History.CardPlaysFinished.Any(e => e.CardPlay.Card == this);
            if (wasPlayedInCombat)
            {
                await CardPileCmd.Add(this, PileType.Hand);
            }
        }
    }
}

