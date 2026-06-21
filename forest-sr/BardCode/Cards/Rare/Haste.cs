using Forest_Sr.BardCode.Cards.KeyWord;
using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forest_Sr.BardCode.Cards.Rare;

/// <summary>
/// 加速术｜Haste
/// 效果：获得 {duration} 层加速效果。
/// 升级：费用 3 → 2
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class Haste : BardCard
{
    private const string _durationKey = "duration";

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar(_durationKey, 3)   // 持续3回合
    ];

    // 关键词：魔法
    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        BardKeywords.Magic
    ];

    // 额外悬停提示
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<HastePower>()
    ];

    public Haste() : base(3, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
    }

    // 升级：费用 3 → 2
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

        int duration = DynamicVars[_durationKey].IntValue;

        await PowerCmd.Apply<HastePower>(choiceContext, 
            Owner.Creature,
            duration,
            Owner.Creature,
            this
        );
    }
}
