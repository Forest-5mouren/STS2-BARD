using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Forest_Sr.BardCode.Cards.Uncommon;

/// <summary>
/// 守卫刻文｜GuardianRune
/// 效果：获得 {block} 点格挡。每当你失去格挡时，对所有敌人造成等量伤害。
/// 升级：格挡 13 → 16
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class GuardianRune : BardCard
{
    private const string _blockKey = "block";

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(13,ValueProp.Move)
    ];

    // 关键词：魔法
    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        BardKeywords.Magic
    ];

    // 此卡获得格挡（用于UI显示）
    public override bool GainsBlock => true;

    public GuardianRune() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
    }

    // 升级：格挡 13 → 16
    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 播放施法动画
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

        // 获得格挡

        await CreatureCmd.GainBlock(Owner.Creature, new BlockVar(DynamicVars.Block.IntValue, ValueProp.Move), cardPlay);

        // 施加守卫刻文能力
        await PowerCmd.Apply<GuardianRunePower>(choiceContext,
            Owner.Creature,
            1,  // 标记层数
            Owner.Creature,
            this
        );
    }
}
