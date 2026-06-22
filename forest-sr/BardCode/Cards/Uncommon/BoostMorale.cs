using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Forest_Sr.BardCode.Cards.Uncommon;

/// <summary>
/// 提振士气｜BoostMorale
/// 效果：全体友方获得 {block} 点格挡。消耗所有活力，每有一点活力额外获得 1 点格挡。
/// 升级：基础格挡 5 → 8
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class BoostMorale : BardCard
{

    // 基础数值声明（基础格挡值 5）
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(5,ValueProp.Move)
    ];

    // 额外悬停提示：活力
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<VigorPower>()
    ];

    public BoostMorale() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AllAllies)
    {
    }

    // 升级：基础格挡 5 → 8
    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 获取当前活力层数
        var vigorPower = Owner.Creature.GetPower<VigorPower>();
        int vigor = vigorPower?.Amount ?? 0;

        // 计算总格挡 = 基础格挡 + 活力层数
        int baseBlock = DynamicVars.Block.IntValue;
        int totalBlock = baseBlock + vigor;

        // 获取全体友方
        IEnumerable<Creature> allies = from c in CombatState.GetTeammatesOf(Owner.Creature)
                                       where c != null && c.IsAlive && c.IsPlayer
                                       select c;

        // 对每个友方给予格挡
        foreach (Creature ally in allies)
        {
            await CreatureCmd.GainBlock(ally, new BlockVar(totalBlock, ValueProp.Move), cardPlay);
        }

        // 消耗所有活力
        if (vigorPower != null)
        {
            await PowerCmd.Remove(vigorPower);
        }
    }
}