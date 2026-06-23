using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Forest_Sr.BardCode.Cards.Common;
//轻快节拍
[RegisterCard(typeof(BardCardPool))]
public sealed class BriskBeat : BardCard
{
    private const int energyCost = 1;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Common;
    private const TargetType targetType = TargetType.AllAllies;
    private const bool shouldShowInCardLibrary = true;

    // 基础数值：持续时间
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        ModCardVars.Int("Duration", 3),
        new CardsVar(1)
    ];

    // 关键词
    public override IEnumerable<CardKeyword> CanonicalKeywords => [BardKeywords.Song];

    public BriskBeat() : base(energyCost, type, rarity, targetType)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int duration = DynamicVars["Duration"].IntValue;

        // 获取全体友方（存活且是玩家的生物）
        IEnumerable<Creature> allies = from c in CombatState!.GetTeammatesOf(Owner.Creature)
                                       where c != null && c.IsAlive && c.IsPlayer
                                       select c;
        // 抽1
        await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.IntValue, Owner);
        // 对每个友方施加轻快节拍能力
        foreach (Creature ally in allies)
        {

            await PowerCmd.Apply<BriskBeatPower>(choiceContext,
                ally,               // 目标
                duration,           // 层数 = 持续回合数
                Owner.Creature,     // 来源
                this);              // 关联卡牌
        }
    }
    protected override void OnUpgrade()
    {
        DynamicVars["Duration"].UpgradeValueBy(1);  // 3 → 4
    }
}
