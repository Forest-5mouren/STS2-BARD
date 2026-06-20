using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using System.Collections.Generic;
using System.Threading.Tasks;
using Forest_Sr.BardCode.Cards.KeyWord;

namespace Forest_Sr.BardCode.Cards.Rare;

/// <summary>
/// 隐身｜Invisibility
/// 效果：使一个友方获得 {intangible} 层无实体。
/// 升级：无实体 1 → 2
/// </summary>
[RegisterCard(typeof(BardCardPool))]
public sealed class Invisbility : BardCard
{
    private const string _intangibleKey = "intangible";

    // 基础数值声明
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar(_intangibleKey, 1)   // 无实体层数
    ];

    // 关键词：消耗、魔法
    protected override IEnumerable<string> RegisteredKeywordIds => [
        BardKeywords.Magic
    ];
    public override List<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];


    // 额外悬停提示
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<IntangiblePower>()
    ];

    public Invisbility() : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
    }

    // 升级：无实体 1 → 2
    protected override void OnUpgrade()
    {
        DynamicVars[_intangibleKey].UpgradeValueBy(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Creature target = cardPlay.Target ?? Owner.Creature;
        int intangibleAmount = DynamicVars[_intangibleKey].IntValue;

        await PowerCmd.Apply<IntangiblePower>(
            target,
            intangibleAmount,
            Owner.Creature,
            this
        );
    }
}