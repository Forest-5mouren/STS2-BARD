using Forest_Sr.BardCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Forest_Sr.BardCode.Cards.Common;

/// <summary>
/// 滑步｜SlideStep
/// 效果：抽1张牌，获得1点临时敏捷。
/// 升级：临时敏捷+1（1→2），抽牌+1（1→2）
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class SlideStep : BardCard
{
    private const int energyCost = 0;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Common;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;
    // 基础数值：抽牌 + 临时敏捷层数
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CardsVar(1),
        ModCardVars.Int("dexterity", 1)
    ];

    // 额外提示文本
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<DexterityPower>()
    ];

    public SlideStep() : base(energyCost, type, rarity, targetType)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int dexterityAmount = DynamicVars["dexterity"].IntValue;

        // 施加临时敏捷（使用 SlideStepPower）
        await PowerCmd.Apply<SlideStepPower>(choiceContext,
            Owner.Creature,
            dexterityAmount,
            Owner.Creature,
            this);

        // 抽牌
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
    }

    protected override void OnUpgrade()
    {
        //DynamicVars.Cards.UpgradeValueBy(1);      // 1 → 2
        DynamicVars["dexterity"].UpgradeValueBy(1);  // 1 → 2
    }
}
