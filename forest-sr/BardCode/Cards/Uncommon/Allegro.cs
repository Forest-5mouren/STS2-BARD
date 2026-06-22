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

/// <summary>
/// 快板｜Allegro
/// 效果：全体友方获得“快板”效果，接下来3回合开始时获得1点能量。
/// 升级：持续时间 3 → 4
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class Allegro : BardCard
{
    private const int energyCost = 1;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Uncommon;
    private const TargetType targetType = TargetType.AllAllies;
    private const bool shouldShowInCardLibrary = true;

    // 基础数值：持续时间
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        ModCardVars.Int("duration", 3)
    ];

    // 关键词
    public override IEnumerable<CardKeyword> CanonicalKeywords => [BardKeywords.Song];

    public Allegro() : base(energyCost, type, rarity, targetType)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int duration = DynamicVars["duration"].IntValue;

        // 获取全体友方（存活且是玩家的生物）
        IEnumerable<Creature> allies = from c in CombatState.GetTeammatesOf(Owner.Creature)
                                       where c != null && c.IsAlive && c.IsPlayer
                                       select c;

        // 对每个友方施加快板能力
        foreach (Creature ally in allies)
        {
            await PowerCmd.Apply<AllegroPower>(choiceContext,
                ally,
                duration,
                Owner.Creature,
                this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["duration"].UpgradeValueBy(1);  // 3 → 4
    }
}
