using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Forest_Sr.BardCode.Cards.Ancient;

/// <summary>
/// 韵律风暴｜RhythmStorm
/// 效果：能力。每当你获得活力时，对所有敌人造成 {damage} 点法术伤害。
///       每当你使用法术牌或乐曲牌时，获得 {vigor} 层活力。
/// 升级：获得固有
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class RhythmStorm : BardCard
{


    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar( 3,MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),   // AOE伤害
        new PowerVar<VigorPower>( 2)     // 获得活力层数
    ];

    // 额外悬停提示
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<VigorPower>()
    ];

    // 关键词：魔法、乐曲// 升级：获得固有
    public override IEnumerable<CardKeyword> CanonicalKeywords => [BardKeywords.Magic, BardKeywords.Song];

    public RhythmStorm() : base(1, CardType.Power, CardRarity.Ancient, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

        // 施加能力（层数为1作为标记）
        await PowerCmd.Apply<RhythmStormPower>(choiceContext,
            Owner.Creature,
            1,
            Owner.Creature,
            this
        );
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}
