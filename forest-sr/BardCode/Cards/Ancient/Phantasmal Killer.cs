using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Cards.Basic;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Cards.Ancient;

/// <summary>
/// 幻影杀手｜Phantasmal Killer
/// 效果：造成 {damage} 点无法格挡、无法被能力影响的伤害，并施加 {weak} 层虚弱。
/// 升级：伤害 22 → 28
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class PhantasmalKiller : BardCard
{

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(22, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move),
        new PowerVar<WeakPower>(1)
    ];

    // 关键词：魔法
    protected override IEnumerable<string> RegisteredKeywordIds => [BardKeywords.Magic];

    // 额外悬停提示
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<WeakPower>()
    ];

    public PhantasmalKiller() : base(0, CardType.Skill, CardRarity.Ancient, TargetType.AnyEnemy)
    {
    }

    // 古老牙齿升级：替换为恶言相加
    //public CardModel GetTranscendenceTransformedCard() => ModelDb.Card<ViciousMockery>();

    // 升级：伤害 22 → 28
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(6);
        // 虚弱层数不变（1 → 1）
        // 费用不变（0费）
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        
        int weakAmount = DynamicVars.Weak.IntValue;

        // 造成伤害（无法格挡、无法被能力影响）
        await CreatureCmd.Damage(
            choiceContext,
            cardPlay.Target,
            DynamicVars.Damage.IntValue,
            ValueProp.Unblockable | ValueProp.Unpowered,
            Owner.Creature,
            this
        );

        // 施加虚弱
        await PowerCmd.Apply<WeakPower>(
            cardPlay.Target,
            DynamicVars.Weak.BaseValue,
            Owner.Creature,
            this
        );
    }

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext)
    {
        // 如果上一回合打出了此卡，且此卡不在手牌中，则将其加入手牌
        if (player == Owner && CombatManager.Instance.History.CardPlaysFinished.Any(
            e => e.RoundNumber == player.Creature.CombatState.RoundNumber - 1 && e.CardPlay.Card == this))
        {
            if (Pile == null || Pile.Type != PileType.Hand)
            {
                await CardPileCmd.Add(this, PileType.Hand, source: null);
            }
        }
    }
}